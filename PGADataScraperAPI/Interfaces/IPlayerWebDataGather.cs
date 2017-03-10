using System.Collections.Generic;
using System.Threading.Tasks;

namespace PGADataScaper.Interfaces
{
	public interface IPlayerWebDataGather
	{
		Task GatherAndWriteAllPlayerStats();
	}
}
