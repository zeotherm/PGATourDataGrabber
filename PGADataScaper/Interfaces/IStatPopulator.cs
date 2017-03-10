using System.Collections.Generic;

namespace PGADataScaper.Interfaces
{
	public interface IStatPopulator
	{
		List<PlayerStats> PopulateAllPlayerStats();
	}
}
