using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PGADataScaper.API.Interfaces
{
	public interface IStatisticsDictionary
	{
		Dictionary<string, StatItem> GenerateSkeleton();
		Dictionary<string, StatItem> PopulateWithValuesForPlayer(string player);
	}
}
