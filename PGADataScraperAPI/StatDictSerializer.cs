using Newtonsoft.Json;
using PGADataScaper.API.Interfaces;
using System.Collections.Generic;
using System.IO;

namespace PGADataScaper.API
{
	public class StatDictSerializer : IJSONSerializer
	{
		private string _file;
		private static string fileName = "statCats.json";
		public StatDictSerializer(string p)
		{
			_file = Path.Combine(p, fileName);
		}

		public Dictionary<string, StatItem> Deserialize()
		{
			Dictionary<string, StatItem> emptyStats;
			using (var dictFile = new StreamReader(_file))
			{
				emptyStats = JsonConvert.DeserializeObject<Dictionary<string, StatItem>>(dictFile.ReadToEnd());
			}
			return emptyStats;
		}

		public void Serialize(Dictionary<string, StatItem> stats)
		{
			using (var file = new StreamWriter(_file))
			{
				file.WriteLine(JsonConvert.SerializeObject(stats, Formatting.Indented));
			}
			return;
		}
	}
}
