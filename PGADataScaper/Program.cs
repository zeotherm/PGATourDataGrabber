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
			var WorkingDirectory = Path.Combine(DataDir, String.Format("Week_{0}", DateTime.Now.Iso8601WeekOfYear().ToString("D2")));


			var statCats = PopulateStatisticsDictionary(WorkingDirectory);
			UpdateStatsFile(DataDir, statCats);


			List<PlayerStats> player_stats = PopulateAllPlayerStats(WorkingDirectory, DataDir);
			using (var output = new StreamWriter(Path.Combine(WorkingDirectory, "results.csv"))) {
				output.WriteLine(player_stats[0].ToCSVHeaderLine());
				foreach( var p in player_stats) {
					output.WriteLine(p.ToCSVLineEntry());
				}
			}

			using (var output = new StreamWriter(Path.Combine(WorkingDirectory, "results-dkonly.csv"))) {
				output.WriteLine(player_stats[0].ToCSVHeaderLine());
				foreach (var p in player_stats.Where(p=>p.IsDKSet()).Select(p=> p)) {
					output.WriteLine(p.ToCSVLineEntry());
				}
			}
		}
		private static void ProcessStatDictionary(string filepath, Action<Dictionary<string, StatItem>, JObject> f, Dictionary<string, StatItem> d) {
			using (var jsonfile = new StreamReader(filepath)) {
				try {
					Console.WriteLine("Reading Stats from player file {0}", Path.GetFileName(filepath));
					JObject player_obj = (JObject)JToken.ReadFrom(new JsonTextReader(jsonfile));
					var tourCode = player_obj["plrs"][0]["years"][0]["tours"][0]["tourCodeUC"].ToString();
					if (tourCode == "R") {
						var all_stats = player_obj["plrs"][0]["years"][0]["tours"][0]["statCats"];
						var length = all_stats.Count();
						for (int i = 0; i < length; ++i) {
							foreach (JObject stat in all_stats[i]["stats"]) {
								f(d, stat);
							}
						}
					}
				} catch (JsonReaderException) {
					Console.WriteLine("File was not in propoer JSON format: {0}", Path.GetFileName(filepath));
				}
			}
			return;
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

					Action<Dictionary<string, StatItem>, JObject> process = (d, o) =>
					{
						var ID = o["statID"].ToString();
						d[ID].Info.Value = o["value"].ToString();
						d[ID].Info.Rank = (int)o["rank"]; // TODO: Yes, yes, potential bad cast
						if (ID == "02394") {
							var numTop10s = o["additionals"][1]["value"].ToString();
							var numberWins = o["additionals"][0]["value"].ToString();
							d[Top10s].Info.Value = (String.IsNullOrEmpty(numTop10s) ? "0" : numTop10s);
							d[Top10s].Info.Rank = -1; // Data not provided
							d[NumWins].Info.Value = (String.IsNullOrEmpty(numberWins) ? "0" : numberWins);
							d[NumWins].Info.Rank = -1; // Data not provided
						}
					};
					ProcessStatDictionary(Path.Combine(path, player.FileName), process, all_stats_dict);
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
				Action<Dictionary<string, StatItem>, JObject> process = (d, o) =>
				{
					var ID = o["statID"].ToString();
					var name = o["name"].ToString();
					if (!d.ContainsKey(ID)) {
						d.Add(ID, new StatItem() { Name = name, Info = new StatValue() });
						if (ID == "02394") {
							d.Add(Top10s, new StatItem()
							{
								Name = o["additionals"][1]["title"].ToString(),
								Info = new StatValue()
							});
							d.Add(NumWins, new StatItem()
							{
								Name = o["additionals"][0]["title"].ToString(),
								Info = new StatValue()
							});
						}
					}
				};
				ProcessStatDictionary(fi.FullName, process, StatDictionary);
			}
			return StatDictionary;
		}

		static async Task MainAsync() {
			var CurrentUser = Environment.UserName;
			var Desktop = String.Format(@"C:\Users\{0}\Desktop", CurrentUser);
			var DataDir = String.Format(Path.Combine(Desktop, "PGA Stats"));
			var gather = new PlayerDataGather(DataDir);

			await gather.GatherAndWriteAllPlayerStats();
		}
	}
}
