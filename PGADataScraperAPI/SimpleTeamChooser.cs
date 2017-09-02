using PGADataScraper.API.Interfaces;
using System.Collections.Generic;
using System.Linq;

namespace PGADataScraper.API {
	public class SimpleTeamChooser : ITeamChooser {
		private int MaxSalary = 50000;

		public IEnumerable<IEnumerable<ScorablePlayer>> ComputeTopTeams(IEnumerable<ScorablePlayer> sps, IEnumerable<ScorablePlayer> keepers) {
			var AvailablePlayers = sps.Except(keepers);
			var remainingSalary = MaxSalary - keepers.Sum(t => t.Salary);
			var PotentialTeams = sps.CombinationsTakeN(6 - keepers.Count());
			var ValidTeams = PotentialTeams.Where(t => t.Select(p => p.Salary).Sum() < remainingSalary);
			return ValidTeams.Select(vt => vt.Concat(keepers)).OrderByDescending(t => t.Select(p => p.Points).Sum()).Take(5);
		}
	}
}
