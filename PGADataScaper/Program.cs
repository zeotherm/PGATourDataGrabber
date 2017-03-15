using PGADataScaper.API;
using PGADataScaper.API.Interfaces;
using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace PGADataScaper
{
	class Program {
		static void Main(string[] args) {
			//
			var Desktop = $@"C:\Users\{Environment.UserName}\Desktop";
			var DataDir = Path.Combine(Desktop, "PGA Stats");
			var WorkingDirectory = Path.Combine(DataDir, $"Week_{DateTime.Now.Iso8601WeekOfYear().ToString("D2")}");
			var PlayerInfo = new PlayerGather(DataDir);
			var Serializer = new StatDictSerializer(DataDir);
			var StatMaker = new StatPopulator(WorkingDirectory, Serializer, PlayerInfo);


			MainAsync(DataDir, PlayerInfo).Wait();  //Populate data dir

			Console.WriteLine("Press enter key when DKSalaries.csv is put into this week's directory...");
			var foo = Console.ReadLine();


			if (!File.Exists(Path.Combine(DataDir, "statCats.json")))
			{
				// Process data directory
				var statCats = new StatisticsDictionary(WorkingDirectory).GenerateSkeleton();
				Serializer.Serialize(statCats);
			}

			var all_player_stats = StatMaker.PopulateAllPlayerStats();

			using (var output = new StreamWriter(Path.Combine(WorkingDirectory, "results-dkonly.csv")))
			{
				output.WriteLine(all_player_stats[0].ToCSVHeaderLine());
				foreach (var p in all_player_stats.Where(p => p.IsDKSet()).Select(p => p))
				{
					output.WriteLine(p.ToCSVLineEntry());
				}
			}
		}

		static async Task MainAsync(string DataDir, IPlayerGather pg) {
			
			var web_gather = new PlayerWebDataGather(DataDir, pg);

			await web_gather.GatherAndWriteAllPlayerStats();
		}
	}
}
