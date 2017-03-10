using PGADataScaper.Interfaces;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Xml.Linq;

namespace PGADataScaper
{
	public class PlayerGather : IPlayerGather
	{
		private DirectoryInfo root_di;

		public PlayerGather(string data_dir)
		{
			root_di = new DirectoryInfo(data_dir);
		}

		public IEnumerable<Player> ReadPlayerList()
		{
			var playerList = XDocument.Load(Path.Combine(root_di.FullName, "PlayerList.xml"));
			var players = playerList.Descendants("option").Where(e => !String.IsNullOrEmpty(e.Attribute("value").Value));
			var player_ids = players.Select(p => new Player(p.Value.Split(','), p.Attribute("value").Value.Split('.')[1]));
			return player_ids;
		}
	}
}
