using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using PGADataScaper.API.Interfaces;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace PGADataScaper.API
{

	public enum StatDictState { EMPTY, SHELL, FULL };
	public class StatisticsDictionary : IStatisticsDictionary
	{
		private readonly Action<Dictionary<string, StatItem>, JObject> _make_empty;
		private readonly Action<Dictionary<string, StatItem>, JObject> _populate;
		private readonly string _working_dir;
		private Dictionary<string, StatItem> _data;
		private StatDictState _state;
		private static string NumWins = "NumWins";
		private static string Top10s = "Top10s";

		public EventHandler<PopulatingStatsEventArgs> PopulatingPlayer;

		private StatisticsDictionary()
		{
			_populate = (d, o) =>
			{
				var ID = o["statID"].ToString();
				d[ID].Info.Value = o["value"].ToString();
				d[ID].Info.Rank = (int)o["rank"]; // TODO: Yes, yes, potential bad cast
				if (ID == "02394")
				{
					var numTop10s = o["additionals"][1]["value"].ToString();
					var numberWins = o["additionals"][0]["value"].ToString();
					d[Top10s].Info.Value = (String.IsNullOrEmpty(numTop10s) ? "0" : numTop10s);
					d[Top10s].Info.Rank = -1; // Data not provided
					d[NumWins].Info.Value = (String.IsNullOrEmpty(numberWins) ? "0" : numberWins);
					d[NumWins].Info.Rank = -1; // Data not provided
				}
			};

			_make_empty = (d, o) =>
			{
				var ID = o["statID"].ToString();
				var name = o["name"].ToString();
				if (!d.ContainsKey(ID))
				{
					d.Add(ID, new StatItem() { Name = name, Info = new StatValue() });
					if (ID == "02394")
					{
						d.Add(Top10s, new StatItem()
						{
							Name = o["additionals"][1]["title"].ToString(),
							Info = new StatValue()
						});
						d.Add(NumWins, new StatItem()
						{
							Name = o["additionals"][0]["title"].ToString(),
							Info = new StatValue()
						});
					}
				}
			};
		}

		public StatisticsDictionary(string working) : this()
		{
			_working_dir = working;

			_data = new Dictionary<string, StatItem>();
			_state = StatDictState.EMPTY;
		}

		public StatisticsDictionary(string working, IJSONSerializer ser) : this()
		{
			_working_dir = working;

			_data = ser.Deserialize();
			_state = StatDictState.SHELL;
		}

		public Dictionary<string, StatItem> GenerateSkeleton()
		{
			switch (_state) {
				case StatDictState.SHELL:
					return _data;
				case StatDictState.EMPTY:
					_state = StatDictState.SHELL;
					return AuxPopulateEmpty();
				case StatDictState.FULL:
				default:
					_data = new Dictionary<string, StatItem>();
					_state = StatDictState.SHELL;
					return AuxPopulateEmpty();
			}
		}

			
		private Dictionary<string, StatItem> AuxPopulateEmpty()
		{
			var di = new DirectoryInfo(_working_dir);

			foreach (var fi in di.EnumerateFiles().Where(f => f.Extension.ToLower() == ".json"))
			{
				ProcessStatDictionary(fi.FullName, _make_empty);
			}
			return _data;
		}

		public Dictionary<string, StatItem> PopulateWithValuesForPlayer(string playerFile)
		{
			if( _state == StatDictState.SHELL)
			{
				ProcessStatDictionary(Path.Combine(_working_dir, playerFile), _populate);
				return _data;
			}
			throw new Exception("Fuck it, I will fix this later");
		}

		private void ProcessStatDictionary(string filepath, Action<Dictionary<string, StatItem>, JObject> f)
		{
			using (var jsonfile = new StreamReader(filepath))
			{

				OnPopulationAttempt(new PopulatingStatsEventArgs { Message = $"Reading Stats from player file {Path.GetFileName(filepath)}", TimePopulated = DateTime.Now });
				JObject player_obj = (JObject)JToken.ReadFrom(new JsonTextReader(jsonfile));
				var tourCode = player_obj["plrs"][0]["years"][0]["tours"][0]["tourCodeUC"].ToString();
				if (tourCode == "R")
				{
					var all_stats = player_obj["plrs"][0]["years"][0]["tours"][0]["statCats"];
					var length = all_stats.Count();
					for (int i = 0; i < length; ++i)
					{
						foreach (JObject stat in all_stats[i]["stats"])
						{
							f(_data, stat);
						}
					}
				}

			}
			return;
		}

		protected virtual void OnPopulationAttempt(PopulatingStatsEventArgs e)
		{
			EventHandler<PopulatingStatsEventArgs> handler = PopulatingPlayer;
			if (handler != null)
			{
				handler(this, e);
			}
		}
	}

	public class PopulatingStatsEventArgs : EventArgs
	{
		public string Message { get; set; }
		public DateTime TimePopulated { get; set; }
	}

	public class PopulatingStatsErrorEventArgs : EventArgs
	{
		public string Message { get; set; }
		public DateTime TimeError { get; set; }
		public DirectoryInfo ErrorDirectory { get; set; }
	}
}
