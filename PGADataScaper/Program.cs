using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace PGADataScaper {
    class Program {
        static void Main(string[] args) {
            XDocument playerList = XDocument.Load(@"C:\Users\Matt\Desktop\PlayerList.xml");

            var players = playerList.Descendants("option").Where( e => !String.IsNullOrEmpty(e.Attribute("value").Value));

            var player_ids = players.Select(p => new Player(p.Value.Split(','), p.Attribute("value").Value.Split('.')[1]));
            var Desktop = @"C:\Users\Matt\Desktop\PGA Stats";
            var path = Path.Combine(Desktop, DateTime.Now.ToString("yyyyMMdd"));
            var di = Directory.CreateDirectory(path);
            var wc = new System.Net.WebClient();
            foreach (var player in player_ids) {
                var webpath = String.Format(@"http://www.pgatour.com/data/players/{0}/2017stat.json", player.ID);
                var file_path = Path.Combine(di.FullName, player.FileName);
                Console.WriteLine( "Downloading data for player {0}", player.FullName);
                try {
                    var playerJSON = wc.DownloadString(webpath);
                    using (var player_file = new StreamWriter(file_path)) {
                        Console.WriteLine("Writing file {0}", file_path);
                        player_file.WriteLine(playerJSON);
                    }
                } catch (WebException) {
                    Console.WriteLine(String.Format("An error occured the program could not download player data: {0}", player.FullName));
                    using (var error_file = new StreamWriter(Path.Combine(di.FullName, "error.log"), true)) {
                        error_file.WriteLine(player.FullName);
                    }
                }
            }
        }
    }
}
