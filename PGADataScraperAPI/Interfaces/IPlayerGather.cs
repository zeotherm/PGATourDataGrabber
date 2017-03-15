using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PGADataScaper.API.Interfaces
{
	public interface IPlayerGather
	{
		IEnumerable<Player> ReadPlayerList();
	}
}
