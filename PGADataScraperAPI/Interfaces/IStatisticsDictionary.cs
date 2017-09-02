using System.Collections.Generic;

namespace PGADataScraper.API.Interfaces {
	public interface IStatisticsDictionary
	{
		Dictionary<string, StatItem> GenerateSkeleton();
		Dictionary<string, StatItem> PopulateWithValuesForPlayer(string player);
	}
}
