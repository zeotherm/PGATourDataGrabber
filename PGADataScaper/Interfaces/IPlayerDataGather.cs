using System.Collections.Generic;
using System.Threading.Tasks;

namespace PGADataScaper.Interfaces
{
	public interface IPlayerDataGather
	{
		IEnumerable<Player> ReadPlayerList();
		Task GatherAndWriteAllPlayerStats();
	}
}
