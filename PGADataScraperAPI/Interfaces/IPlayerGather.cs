using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PGADataScraper.API.Interfaces
{
	public interface IPlayerGather
	{
		IEnumerable<Player> ReadPlayerList();
		int Count();
	}
}
