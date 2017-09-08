using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PGADataScraper.API.Interfaces {
	
	public interface ITeamChooser {
		Task<IEnumerable<ScorablePlayer>[]> ComputeTopTeams(IEnumerable<ScorablePlayer> sps, IEnumerable<ScorablePlayer> keepers);
	}
}
