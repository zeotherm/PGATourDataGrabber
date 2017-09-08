using PGADataScraper.API.Interfaces;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PGADataScraper.API {
	public class SimpleTeamChooser : ITeamChooser {
		private int MaxSalary = 50000;

		public Task<IEnumerable<ScorablePlayer>[]> ComputeTopTeams(IEnumerable<ScorablePlayer> sps, IEnumerable<ScorablePlayer> keepers) {
			return Task<IEnumerable<ScorablePlayer>[]>.Factory.StartNew(() => {

				var AvailablePlayers = sps.Except(keepers);
				var remainingSalary = MaxSalary - keepers.Sum(t => t.Salary);
				var PotentialTeams = sps.CombinationsTakeN(6 - keepers.Count());
				var ValidTeams = PotentialTeams.Where(t => t.Select(p => p.Salary).Sum() < remainingSalary);
				var Top5 = ValidTeams.Select(vt => vt.Concat(keepers)).OrderByDescending(t => t.Select(p => p.Points).Sum()).Take(5);
				return Top5.ToArray();
			});
		}
	}
}
