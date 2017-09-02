using System.Collections.Generic;
using System.Threading.Tasks;

namespace PGADataScraper.API.Interfaces
{
	public interface IPlayerWebDataGather
	{
		Task GatherAndWriteAllPlayerStats();
	}
}
