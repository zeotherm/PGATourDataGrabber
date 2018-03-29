using HtmlAgilityPack;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace ResultsParser {
	public class PlayerSimpleData {
		public string Name { get; set; }
		public int Events { get; set; }
		public int CutsMade { get; set; }
		public int Top10s { get; set; }
		public int Wins { get; set; }
		public int CupPoints { get; set; }
		public decimal Earnings { get; set; }
	}
	public class Program {
		static void Main(string[] args) {
			//using (var outcsv_file = new StreamWriter(@"C:\\Users\Matt\Desktop\PGATourDataGrabber\PLAYERSResults.csv")) {
			//	outcsv_file.WriteLine("Name, Position, Course Par, Status, Total Score, Round 1, Round 2, Round 3, Round 4");
			//	using (var jsonfile = new StreamReader(@"C:\Users\Matt\Desktop\PGATourDataGrabber\PLAYERSResults.json")) {

			//		//OnPopulationAttempt(new PopulatingStatsEventArgs { Message = $"Reading Stats from player file {Path.GetFileName(filepath)}", TimePopulated = DateTime.Now });
			//		JObject results_obj = (JObject)JToken.ReadFrom(new JsonTextReader(jsonfile));
			//		var players = results_obj["leaderboard"]["players"];
			//		var course_par = results_obj["leaderboard"]["courses"][0]["par_total"].Value<string>();
			//		var num_players = players.Count();
			//		for (int i = 0; i < num_players; ++i) {
			//			var first_name = players[i]["player_bio"]["first_name"].Value<string>();
			//			var last_name = players[i]["player_bio"]["last_name"].Value<string>();
			//			var position = players[i]["current_position"].Value<string>();
			//			var status = players[i]["status"].Value<string>();
			//			var score_total = players[i]["total"].Value<int?>();
			//			var round1_score = players[i]["rounds"][0]["strokes"].Value<int?>();
			//			var round2_score = players[i]["rounds"][1]["strokes"].Value<int?>();
			//			var round3_score = players[i]["rounds"][2]["strokes"].Value<int?>();
			//			var round4_score = players[i]["rounds"][3]["strokes"].Value<int?>();
			//			var full_name = String.Concat(first_name, " ", last_name);
			//			outcsv_file.WriteLine($"{full_name}, {position}, {course_par}, {status}, {score_total}, {round1_score}, {round2_score}, {round3_score}, {round4_score}");
			//		}

			//	}
			//}
			var startOffsets = Enumerable.Range(0, 5).Select(x => x * 40);
			var results = new List<PlayerSimpleData>();
			foreach (var offset in startOffsets) {
				var webscrape = GetESPNData(offset);

				var espn = new HtmlDocument();
				espn.LoadHtml(webscrape);
				//var data = espn.DocumentNode.Descendants("div").Where(e => e.Attributes["class"].Value == "mod-content");
				//var nodes = espn.DocumentNode.Descendants("div").Select(e => e.InnerHtml).ToList();
				////.Where(x => x.Attributes["class"].Value == "mod-content").First();
				//// element 17 of the list has what I want

				//var findclasses = espn.DocumentNode.Descendants("div").Where(d =>
				//		d.Attributes.Contains("class") && d.Attributes["class"].Value.Contains("mod-content")
				//);
				var testchunk = espn.DocumentNode.SelectSingleNode("/html[1]/body[1]/div[1]/div[2]/div[1]/div[2]/div[3]/div[1]/div[1]/div[1]/table[1]");
				//var tableentries = testchunk.SelectNodes("table[1]");///tr[2]");
				var values = testchunk./*First().*/ChildNodes;

				foreach (var node in values.Skip(1)) {
					//Console.WriteLine("What");
					var name = node.ChildNodes[1].InnerText;
					if (!Int32.TryParse(node.ChildNodes[3].InnerText, out int events)) continue;
					if (!Int32.TryParse(node.ChildNodes[5].InnerText, out int cutsmade)) continue;
					if (!Int32.TryParse(node.ChildNodes[6].InnerText, out int top10s)) continue;
					if (!Int32.TryParse(node.ChildNodes[7].InnerText, out int wins)) continue;
					if (!Int32.TryParse(node.ChildNodes[8].InnerText, NumberStyles.AllowThousands, CultureInfo.InvariantCulture, out int points)) continue;
					if (!Decimal.TryParse(node.ChildNodes[9].InnerText, NumberStyles.AllowCurrencySymbol | NumberStyles.AllowDecimalPoint | NumberStyles.AllowThousands, new CultureInfo("en-US"), out decimal earns)) continue;
					results.Add(new PlayerSimpleData {
						Name = name,
						Events = events,
						CutsMade = cutsmade,
						Top10s = top10s,
						Wins = wins,
						CupPoints = points,
						Earnings = earns
					});
					//results.Add(node.ChildNodes.Select(n => n.InnerText).ToList());
				}
			}
			//var player = 
			Console.WriteLine("Breakpoint");

		}

		static public string GetESPNData(int startPlayer) {
			var wc = new WebClient();
			// 2017 data: var webpath = String.Format(@"http://www.pgatour.com/data/players/{0}/2017stat.json", p.ID);
			var webpath = String.Format($@"http://www.espn.com/golf/statistics/_/count/{startPlayer.ToString()}");
			//OnPlayerDownloadAttempt(new DownloadingPlayerEventArgs { Message = $"Downloading data for player {p.FullName}", TimeGrabbed = DateTime.Now });
			return wc.DownloadString(webpath);
			//return await wc.DownloadStringTaskAsync(webpath);
		}
	}
}
