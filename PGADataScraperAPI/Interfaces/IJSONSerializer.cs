using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PGADataScaper.Interfaces
{
	public interface IJSONSerializer
	{
		Dictionary<string, StatItem> Deserialize();
		void Serialize(Dictionary<string, StatItem> stats);
	}
}
