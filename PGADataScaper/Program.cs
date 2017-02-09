using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Microsoft.VisualBasic.FileIO;

namespace PGADataScaper {
	class Program {
		static string NumWins = "NumWins";
		static string Top10s = "Top10s";

		static void Main(string[] args) {
			//MainAsync().Wait();
			var CurrentUser = Environment.UserName;
			var Desktop = String.Format(@"C:\Users\{0}\Desktop", CurrentUser);
			var DataDir = String.Format(Path.Combine(Desktop, "PGA Stats"));
			var path = Path.Combine(DataDir, DateTime.Now.ToString("yyyyMMdd"));


			var statCats = PopulateStatisticsDictionary(path);
			UpdateStatsFile(DataDir, statCats);


			List<PlayerStats> player_stats = PopulateAllPlayerStats(path, DataDir);
			using (var output = new StreamWriter(Path.Combine(path, "results.csv"))) {
				output.WriteLine(player_stats[0].ToCSVHeaderLine());
				foreach( var p in player_stats) {
					output.WriteLine(p.ToCSVLineEntry());
				}
			}

			using (var output = new StreamWriter(Path.Combine(path, "results-dkonly.csv"))) {
				output.WriteLine(player_stats[0].ToCSVHeaderLine());
				foreach (var p in player_stats.Where(p=>p.IsDKSet()).Select(p=> p)) {
					output.WriteLine(p.ToCSVLineEntry());
				}
			}
		}

		private static List<PlayerStats> PopulateAllPlayerStats(string path, string DataDir) {
			var playerList = XDocument.Load(Path.Combine(DataDir, "PlayerList.xml"));
			var players = playerList.Descendants("option").Where(e => !String.IsNullOrEmpty(e.Attribute("value").Value));
			var player_ids = players.Select(p => new Player(p.Value.Split(','), p.Attribute("value").Value.Split('.')[1]));
			var stat_list = new List<PlayerStats>();

			foreach (var player in player_ids) {
				if (File.Exists(Path.Combine(path, player.FileName))) {
					var all_stats_dict = DeserializeStatCats(Path.Combine(DataDir, "statCats.json"));
					// This entire following chunk of code can be abstracted and have a lambda passed into the center portion which will keep redundancy down
					using (var jsonfile = new StreamReader(Path.Combine(path, player.FileName))) {
						try {
							Console.WriteLine("Reading Stats from player file {0}", player.FileName);
							JObject player_obj = (JObject)JToken.ReadFrom(new JsonTextReader(jsonfile));
							var tourCode = player_obj["plrs"][0]["years"][0]["tours"][0]["tourCodeUC"].ToString();
							if (tourCode == "R") {
								var all_stats = player_obj["plrs"][0]["years"][0]["tours"][0]["statCats"];
								var length = all_stats.Count();
								for (int i = 0; i < length; ++i) {
									foreach (JObject stat in all_stats[i]["stats"]) {
										var ID = stat["statID"].ToString();
										all_stats_dict[ID].Info.Value = stat["value"].ToString();
										all_stats_dict[ID].Info.Rank = (int)stat["rank"]; // TODO: Yes, yes, potential bad cast
										if (ID == "02394") {
											var numTop10s = stat["additionals"][1]["value"].ToString();
											var numberWins = stat["additionals"][0]["value"].ToString();
											all_stats_dict[Top10s].Info.Value = (String.IsNullOrEmpty(numTop10s) ? "0" : numTop10s);
											all_stats_dict[Top10s].Info.Rank = -1; // Data not provided
											all_stats_dict[NumWins].Info.Value = (String.IsNullOrEmpty(numberWins) ? "0" : numberWins);
											all_stats_dict[NumWins].Info.Rank = -1; // Data not provided
										}
									}
								}
							}
						} catch (JsonReaderException) {
							Console.WriteLine("File was not in propoer JSON format: {0}", player.FileName);
						}
					}
					stat_list.Add(new PlayerStats() { player = player, stats = all_stats_dict.Select(item => item.Value).OrderBy(n => n.Name).ToList() });

				}
			}

			using (var dksalfile = new TextFieldParser(Path.Combine(path, "DKSalaries.csv"))) {
				dksalfile.TextFieldType = FieldType.Delimited;
				dksalfile.SetDelimiters(",");
				dksalfile.ReadLine(); // read the header line
				while(!dksalfile.EndOfData) {
					var fields = dksalfile.ReadFields();
					var name = fields[1];
					var salary = Double.Parse(fields[2]);
					var fpts = Double.Parse(fields[4]);
					//stat_list.Where(p => p.player.FullName == name.Replace('-', ' ')).First()?.AddDraftKingsStats(salary, fpts);
					var pl = stat_list.Where(p => p.player.FullName == name.Replace('-', ' '));
					if (pl.Any()) {
						pl.First().AddDraftKingsStats(salary, fpts);
					}
				}
			}
			return stat_list;
		}

		static Dictionary<string, StatItem> DeserializeStatCats( string path) {
			Dictionary<string, StatItem> emptyStats;
			using (var dictFile = new StreamReader(path)) {
				emptyStats = JsonConvert.DeserializeObject<Dictionary<string, StatItem>>(dictFile.ReadToEnd());
			}
			return emptyStats;
		}

		static void UpdateStatsFile( string path, Dictionary<string, StatItem> statCats) {
			using (var file = new StreamWriter(Path.Combine(path, "statCats.json"))) {
				file.WriteLine(JsonConvert.SerializeObject(statCats, Formatting.Indented));
			}
			return;
		}

		static Dictionary<string, StatItem> PopulateStatisticsDictionary( string path) {
			var di = new DirectoryInfo(path);
			Dictionary<string, StatItem> StatDictionary = new Dictionary<string, StatItem>();

			foreach (var fi in di.EnumerateFiles().Where(f => f.Extension.ToLower() == ".json")) {
				using (var jsonfile = new StreamReader(fi.FullName)) {
					try {
						Console.WriteLine("Reading Stats from player file {0}", fi.Name);
						JObject player_obj = (JObject)JToken.ReadFrom(new JsonTextReader(jsonfile));
						var tourCode = player_obj["plrs"][0]["years"][0]["tours"][0]["tourCodeUC"].ToString();
						if (tourCode == "R") {
							var all_stats = player_obj["plrs"][0]["years"][0]["tours"][0]["statCats"];
							var length = all_stats.Count();
							for (int i = 0; i < length; ++i) {
								foreach (JObject stat in all_stats[i]["stats"]) {
									var ID = stat["statID"].ToString();
									var name = stat["name"].ToString();
									if (!StatDictionary.ContainsKey(ID)) {
										StatDictionary.Add(ID, new StatItem() { Name = name, Info = new StatValue() });
										if (ID == "02394") {
											StatDictionary.Add(Top10s, new StatItem()
											{
												Name = stat["additionals"][1]["title"].ToString(),
												Info = new StatValue()
											});
											StatDictionary.Add(NumWins, new StatItem()
											{
												Name = stat["additionals"][0]["title"].ToString(),
												Info = new StatValue()
											});
										}
									}

								}
							}
						}
					} catch (JsonReaderException) {
						Console.WriteLine("File was not in propoer JSON format: {0}", fi.Name);
					}
				}
			}
			return StatDictionary;
		}

		static async Task MainAsync() {
			var CurrentUser = Environment.UserName;
			var Desktop = String.Format(@"C:\Users\{0}\Desktop", CurrentUser);
			var DataDir = String.Format(Path.Combine(Desktop, "PGA Stats"));
			var playerList = XDocument.Load(Path.Combine(DataDir, "PlayerList.xml"));
			var players = playerList.Descendants("option").Where(e => !String.IsNullOrEmpty(e.Attribute("value").Value));
			var player_ids = players.Select(p => new Player(p.Value.Split(','), p.Attribute("value").Value.Split('.')[1]));
			
			var path = Path.Combine(DataDir, DateTime.Now.ToString("yyyyMMdd"));
			DirectoryInfo di;
			if (!Directory.Exists(path)) {
				di = Directory.CreateDirectory(path);
			} else {
				di = new DirectoryInfo(path);
			}
			var wc = new System.Net.WebClient();
			foreach (var player in player_ids) {
				try {
					
					await WritePlayerData(player, di, await GatherPlayerData(player, di, wc));
				} catch (WebException) {
					Console.WriteLine(String.Format("An error occured the program could not download player data: {0}", player.FullName));
					using (var error_file = new StreamWriter(Path.Combine(di.FullName, "error.log"), true)) {
						error_file.WriteLine(player.FullName);
					}
				}
			}
		}

		static async Task<string> GatherPlayerData( Player p, DirectoryInfo di, WebClient wc) {
			var webpath = String.Format(@"http://www.pgatour.com/data/players/{0}/2017stat.json", p.ID);
			Console.WriteLine("Downloading data for player {0}", p.FullName);
			return await wc.DownloadStringTaskAsync(webpath);
		}

		static async Task WritePlayerData( Player p, DirectoryInfo di, string data) {
			var file_path = Path.Combine(di.FullName, p.FileName);
			using (var player_file = new StreamWriter(file_path)) {
				Console.WriteLine("Writing file {0}", file_path);
				await player_file.WriteLineAsync(data);
			}
		}
	}
}
