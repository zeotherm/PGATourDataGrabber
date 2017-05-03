using Microsoft.VisualBasic.FileIO;
using Newtonsoft.Json;
using PGADataScaper.API.Interfaces;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace PGADataScaper.API
{
	public class StatPopulator : IStatPopulator
	{
		private readonly string _working_directory;
		private readonly IJSONSerializer _serializer;
		private readonly IPlayerGather _pg;

		public event EventHandler<PopulatingStatsEventArgs> PopulatingAttempt;
		public event EventHandler<PopulatingStatsErrorEventArgs> PopulatingError;

		public StatPopulator(string w, IJSONSerializer s, IPlayerGather pg)
		{
			_working_directory = w;
			_serializer = s;
			_pg = pg;
		}

		public List<PlayerStats> PopulateAllPlayerStats()
		{
			var player_ids = _pg.ReadPlayerList();
			var stat_list = new List<PlayerStats>();

			foreach (var player in player_ids)
			{
				if (File.Exists(Path.Combine(_working_directory, player.FileName)))
				{
					var statDict = new StatisticsDictionary(_working_directory, _serializer);
					statDict.PopulatingPlayer += PopulatingAttempt;
					try
					{
						stat_list.Add(new PlayerStats() { player = player, stats = statDict.PopulateWithValuesForPlayer(player.FileName).Select(item => item.Value).OrderBy(n => n.Name).ToList() });
					} catch (JsonReaderException)
					{
						OnPopulationFailure(new PopulatingStatsErrorEventArgs { Message = $"File was not in proper JSON format: {player.FileName}", TimeError = DateTime.Now, ErrorDirectory = new DirectoryInfo(_working_directory) });
					}
				}
			}

			using (var dksalfile = new TextFieldParser(Path.Combine(_working_directory, "DKSalaries.csv")))
			{
				dksalfile.TextFieldType = FieldType.Delimited;
				dksalfile.SetDelimiters(",");
				dksalfile.ReadLine(); // read the header line
				while (!dksalfile.EndOfData)
				{
					var fields = dksalfile.ReadFields();
					var name = fields[1];
					var salary = Double.Parse(fields[2]);
					var fpts = Double.Parse(fields[4]);
					var pl = stat_list.Where(p => p.player.FullName == name.Replace('-', ' '));
					if (pl.Any())
					{
						pl.First().AddDraftKingsStats(salary, fpts);
					}
				}
			}
			return stat_list;
		}

		protected virtual void OnPopulationFailure(PopulatingStatsErrorEventArgs e)
		{
			PopulatingError?.Invoke(this, e);
		}
	}
}
