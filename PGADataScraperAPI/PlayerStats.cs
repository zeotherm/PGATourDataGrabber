using System;
using System.Collections.Generic;
using System.Text;

namespace PGADataScraper.API {
	public class StatItem {
		public string Name { get; set; }
		public StatValue Info { get; set;}

		public override string ToString() {
			return String.Format("Name: {0}, {1}", Name, Info == null ? "null" : Info.ToString());
		}
	}

	public class StatValue
	{
		public int Rank { get; set; }
		private string _value;
		public string Value {
			get { return _value; }
			set {
				_value = value;
				SetSortValue(value);
			}
		}

		private void SetSortValue(string entry) {
			double temp = 0.0;
			if (double.TryParse(entry, out temp)) {
				SortValue = temp;
				return;
			} else if (entry.Contains("$")) {
				SortValue = double.Parse(entry.Replace("$", string.Empty));
				return;
			} else if (entry.Contains("'")) { // it is a X'Y" needs to be converted to X.Y ft
				var ft_inch = entry.Split(new char[] { '\"', '\'' }, StringSplitOptions.RemoveEmptyEntries);
				var ft = double.Parse(ft_inch[0]);
				var inch = double.Parse(ft_inch[1]);
				SortValue = ft + inch / 12.0;
				return;
			} else if (entry.Contains("%")) { // return the value / 100
				SortValue = double.Parse(entry.Replace("%", string.Empty));
				return;
			} else if (entry.Contains("T")) { // strip off the T and return their place
				SortValue = double.Parse(entry.Replace("T", string.Empty));
				return;
			} else {
				SortValue = double.NaN;
				return;
			}
		}

		public double SortValue { get; private set; }

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
		public double Salary { get; private set; }
		public double Points { get; private set; }
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
