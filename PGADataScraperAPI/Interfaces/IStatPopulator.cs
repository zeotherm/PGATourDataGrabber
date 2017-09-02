using System.Collections.Generic;

namespace PGADataScraper.API.Interfaces
{
	public interface IStatPopulator
	{
		List<PlayerStats> PopulateAllPlayerStats();
	}
}
