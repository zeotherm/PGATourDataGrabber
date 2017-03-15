using System.Collections.Generic;
using System.Threading.Tasks;

namespace PGADataScaper.API.Interfaces
{
	public interface IPlayerWebDataGather
	{
		Task GatherAndWriteAllPlayerStats();
	}
}
