using PGADataScaper.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Net;
using System.Xml.Linq;

namespace PGADataScaper
{
	class PlayerWebDataGather : IPlayerWebDataGather
	{
		private readonly WebClient wc;
		private DirectoryInfo root_di, data_di;
		private readonly IPlayerGather _pg;

		public PlayerWebDataGather( string root, IPlayerGather pg) { // Root is the Desktop\PGS Stats directory which holds Players.xml and the statCat.json file
			root_di = new DirectoryInfo(root);
			wc = new WebClient();
			_pg = pg;
		}

		public async Task GatherAndWriteAllPlayerStats() {
			var path = Path.Combine(root_di.FullName, String.Format("Week_{0}", DateTime.Now.Iso8601WeekOfYear().ToString("D2")));
			if (!Directory.Exists(path)) {
				data_di = Directory.CreateDirectory(path);
			} else {
				data_di = new DirectoryInfo(path);
			}
			foreach (var player in _pg.ReadPlayerList()) {
				try {
					await WritePlayerData(player, await GatherPlayerData(player));
				} catch (WebException) {
					Console.WriteLine(String.Format("An error occured the program could not download player data: {0}", player.FullName));
					using (var error_file = new StreamWriter(Path.Combine(data_di.FullName, "error.log"), true)) {
						error_file.WriteLine(player.FullName);
					}
				}
			}
		}

		private async Task WritePlayerData( Player p, string statData) {
			var file_path = Path.Combine(data_di.FullName, p.FileName);
			using (var player_file = new StreamWriter(file_path)) {
				Console.WriteLine("Writing file {0}", file_path);
				await player_file.WriteLineAsync(statData);
			}
		}

		private async Task<string> GatherPlayerData(Player p) {
			var webpath = String.Format(@"http://www.pgatour.com/data/players/{0}/2017stat.json", p.ID);
			Console.WriteLine("Downloading data for player {0}", p.FullName);
			return await wc.DownloadStringTaskAsync(webpath);
		}
	}
}
