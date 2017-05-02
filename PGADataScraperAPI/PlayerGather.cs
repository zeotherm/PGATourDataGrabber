using PGADataScaper.API.Interfaces;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Xml.Linq;

namespace PGADataScaper.API
{
	public class PlayerGather : IPlayerGather
	{
		private readonly DirectoryInfo root_di;
		private readonly XDocument playerList;
		private readonly int _count;
		private readonly IEnumerable<XElement> _players;
		public PlayerGather(string data_dir)
		{
			root_di = new DirectoryInfo(data_dir);
			playerList = XDocument.Load(Path.Combine(root_di.FullName, "PlayerList.xml"));
			_players = playerList.Descendants("option").Where(e => !String.IsNullOrEmpty(e.Attribute("value").Value));
			_count = _players.Count();
		}

		public IEnumerable<Player> ReadPlayerList()
		{ 
			return _players.Select(p => new Player(p.Value.Split(','), p.Attribute("value").Value.Split('.')[1]));
		}

		public int Count()
		{
			return _count; 
		}
	}
}
