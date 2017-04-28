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
			StatMaker.PopulatingAttempt += WritePlayerStats;
			StatMaker.PopulatingError += WritePlayerStatsError;

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
			web_gather.DownloadingPlayer += DownloadPlayerNotice;
			web_gather.DownloadPlayerError += WritePlayerError;
			web_gather.WritingPlayer += WritePlayerNotice;

			await web_gather.GatherAndWriteAllPlayerStats();
		}

		static void DownloadPlayerNotice( object sender, DownloadingPlayerEventArgs e)
		{
			Console.WriteLine($"{e.TimeGrabbed}: {e.Message}");
			return;
		}

		static void WritePlayerNotice( object sender, WritingPlayerEventArgs e)
		{
			Console.WriteLine($"{e.TimeWritten}: {e.Message}");
			return;
		}

		static void WritePlayerError(object sender, DownloadingPlayerErrorEventArgs e)
		{
			var old_color = Console.ForegroundColor;
			Console.ForegroundColor = ConsoleColor.Yellow;
			var message = $"{ e.TimeError}: { e.Message}";
			Console.WriteLine(message);
			using (var error_file = new StreamWriter(Path.Combine(e.ErrorDirectory.FullName, "error.log"), true))
			{
				error_file.WriteLine(message);
			}
			Console.ForegroundColor = old_color;
		}

		static void WritePlayerStats( object sender, PopulatingStatsEventArgs e)
		{
			Console.WriteLine($"{e.TimePopulated}: {e.Message}");
			return;
		}

		static void WritePlayerStatsError(object sender, PopulatingStatsErrorEventArgs e)
		{
			var old_color = Console.ForegroundColor;
			Console.ForegroundColor = ConsoleColor.Red;
			Console.WriteLine($"{e.TimeError}: {e.Message}");
			Console.ForegroundColor = old_color;
		}
	}
}
