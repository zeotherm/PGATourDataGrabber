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

namespace PGADataScaper {
	class Program {
		static void Main(string[] args) {
			MainAsync().Wait();
		}

		static async Task MainAsync() {
			var CurrentUser = Environment.UserName;
			var Desktop = String.Format(@"C:\Users\{0}\Desktop", CurrentUser);
			var playerList = XDocument.Load(Path.Combine(Desktop, "PlayerList.xml"));
			var players = playerList.Descendants("option").Where(e => !String.IsNullOrEmpty(e.Attribute("value").Value));
			var player_ids = players.Select(p => new Player(p.Value.Split(','), p.Attribute("value").Value.Split('.')[1]));
			var DataDir = String.Format(Path.Combine(Desktop, "PGA Stats"));
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
