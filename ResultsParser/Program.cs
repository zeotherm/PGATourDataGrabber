using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ResultsParser {
	class Program {
		static void Main(string[] args) {
			using (var outcsv_file = new StreamWriter(@"C:\\Users\Matt\Desktop\PGATourDataGrabber\PLAYERSResults.csv")) {
				outcsv_file.WriteLine("Name, Position, Course Par, Status, Total Score, Round 1, Round 2, Round 3, Round 4");
				using (var jsonfile = new StreamReader(@"C:\Users\Matt\Desktop\PGATourDataGrabber\PLAYERSResults.json")) {

					//OnPopulationAttempt(new PopulatingStatsEventArgs { Message = $"Reading Stats from player file {Path.GetFileName(filepath)}", TimePopulated = DateTime.Now });
					JObject results_obj = (JObject)JToken.ReadFrom(new JsonTextReader(jsonfile));
					var players = results_obj["leaderboard"]["players"];
					var course_par = results_obj["leaderboard"]["courses"][0]["par_total"].Value<string>();
					var num_players = players.Count();
					for (int i = 0; i < num_players; ++i) {
						var first_name = players[i]["player_bio"]["first_name"].Value<string>();
						var last_name = players[i]["player_bio"]["last_name"].Value<string>();
						var position = players[i]["current_position"].Value<string>();
						var status = players[i]["status"].Value<string>();
						var score_total = players[i]["total"].Value<int?>();
						var round1_score = players[i]["rounds"][0]["strokes"].Value<int?>();
						var round2_score = players[i]["rounds"][1]["strokes"].Value<int?>();
						var round3_score = players[i]["rounds"][2]["strokes"].Value<int?>();
						var round4_score = players[i]["rounds"][3]["strokes"].Value<int?>();
						var full_name = String.Concat(first_name, " ", last_name);
						outcsv_file.WriteLine($"{full_name}, {position}, {course_par}, {status}, {score_total}, {round1_score}, {round2_score}, {round3_score}, {round4_score}");
					}

				}
			}
		}
	}
}
