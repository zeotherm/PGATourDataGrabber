using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PGADataScaper
{
	public class StatItem {
		public string Name { get; set; }
		//[JsonIgnore]
		public StatValue Info { get; set;}

		public override string ToString() {
			return String.Format("Name: {0}, {1}", Name, Info == null ? "null" : Info.ToString());
		}
	}

	public class StatValue
	{
		public int Rank { get; set; }
		public string Value { get; set; }

		public StatValue() {
			Rank = -99;
			Value = "N/A";
		}

		public override string ToString() {
			return String.Format("Value = {0}, Rank = {1}", Value, Rank);
		}
	}

	public class PlayerStats
	{
		public Player player { get; set; }
		public List<StatItem> stats { get; set; }
		double Salary;
		double Points;
		private bool DKSet = false;
		public void AddDraftKingsStats( double s, double p) { Salary = s; Points = p; DKSet = true;  return; }
		public bool IsDKSet() { return DKSet; }
		public string ToCSVLineEntry() {
			StringBuilder sb = new StringBuilder();
			sb.Append(player.FullName);
			sb.Append(',');
			foreach (var stat in stats) {
				sb.Append(stat.Info.Value.Replace(",", String.Empty));
				sb.Append(',');
			}
			sb.Append(Salary);
			sb.Append(',');
			sb.Append(Points);
			sb.Append(',');
			if (Salary != 0) {
				sb.Append(Points / (Salary / 1000));
			} else {
				sb.Append(0.0);
			}
			return sb.ToString();
		}

		public string ToCSVHeaderLine() {
			StringBuilder sb = new StringBuilder();
			sb.Append("Name");
			sb.Append(',');
			foreach (var stat in stats) {
				sb.Append(stat.Name);
				sb.Append(',');
			}
			sb.Append("DK Salary");
			sb.Append(',');
			sb.Append("DK Points");
			sb.Append(',');
			sb.Append("DK Points/$1000");
			return sb.ToString();
		}

	}
}
