using System.Collections.Generic;

namespace PGADataScaper.API.Interfaces
{
	public interface IStatPopulator
	{
		List<PlayerStats> PopulateAllPlayerStats();
	}
}
