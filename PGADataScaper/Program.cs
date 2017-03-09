using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.VisualBasic.FileIO;
using PGADataScaper.Interfaces;

namespace PGADataScaper
{
	class Program {
		static void Main(string[] args) {
			//
			var CurrentUser = Environment.UserName;
			var Desktop = String.Format(@"C:\Users\{0}\Desktop", CurrentUser);
			var DataDir = String.Format(Path.Combine(Desktop, "PGA Stats"));
			var WorkingDirectory = Path.Combine(DataDir, String.Format("Week_{0}", DateTime.Now.Iso8601WeekOfYear().ToString("D2")));
			var PlayerInfo = new PlayerGather(DataDir);

			MainAsync(DataDir).Wait();  //Populate data dir

			Console.WriteLine("Press any key when DKSalaries.csv is put into this week's directory...");
			var foo = Console.ReadLine();

			// Process data directory
			var serializer = new StatDictSerializer(DataDir);
			var statCats = new StatisticsDictionary(WorkingDirectory).GenerateSkeleton();
			serializer.Serialize(statCats);


			List<PlayerStats> player_stats = PopulateAllPlayerStats(WorkingDirectory, PlayerInfo, serializer);

			using (var output = new StreamWriter(Path.Combine(WorkingDirectory, "results-dkonly.csv")))
			{
				output.WriteLine(player_stats[0].ToCSVHeaderLine());
				foreach (var p in player_stats.Where(p => p.IsDKSet()).Select(p => p))
				{
					output.WriteLine(p.ToCSVLineEntry());
				}
			}
		}

		private static List<PlayerStats> PopulateAllPlayerStats(string path, IPlayerGather pg, IJSONSerializer ser) {

			var player_ids = pg.ReadPlayerList();
			var stat_list = new List<PlayerStats>();
			
			foreach (var player in player_ids) {
				if (File.Exists(Path.Combine(path, player.FileName))) {
					var statDict = new StatisticsDictionary(path, ser).PopulateWithValuesForPlayer(player.FileName);
					stat_list.Add(new PlayerStats() { player = player, stats = statDict.Select(item => item.Value).OrderBy(n => n.Name).ToList() });
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
					var pl = stat_list.Where(p => p.player.FullName == name.Replace('-', ' '));
					if (pl.Any()) {
						pl.First().AddDraftKingsStats(salary, fpts);
					}
				}
			}
			return stat_list;
		}

		static async Task MainAsync(string DataDir) {
			var play_gather = new PlayerGather(DataDir);
			var web_gather = new PlayerWebDataGather(DataDir, play_gather);

			await web_gather.GatherAndWriteAllPlayerStats();
		}
	}
}
