using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using PGADataScaper.API;

namespace PGAAnalyticsWinForms
{
	public partial class Top10Lists : Form
	{
		private List<Top10ListElement> t10 = new List<Top10ListElement>();
		private IEnumerable<PlayerStats> _ps;
		public Top10Lists(IEnumerable<PlayerStats> ps, IEnumerable<string> cats)
		{
			InitializeComponent();
			this.Icon = Properties.Resources.golf;
			foreach(var cat in cats) {
				t10.Add(new Top10ListElement(cat));
			}
			_ps = ps;
		}

		private void Top10Lists_Load(object sender, EventArgs e)
		{

			int tabindex = 0;
			foreach ( var cat in t10) {
				tabControl1.TabPages[tabindex++].Text = cat.StatName;
				if( tabindex >= tabControl1.TabCount) {
					MessageBox.Show($"A max of {tabControl1.TabCount} Top10 categories is allowed, discarding extras", "Notice", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
					break;
				}
			}
			// clear out unused tabs
			for( int indx = tabControl1.TabCount-1; indx >= tabindex; indx--) {
				tabControl1.TabPages.RemoveAt(indx);
			}
		}
	}

	internal class Top10ListElement {
		public string StatName {
			get;
			private set;
		}

		public SortDir Direction {
			get;
			private set;
		}

		public Top10ListElement(string input) {
			var delim_index = input.IndexOf('{');
			if (delim_index != -1) {
				StatName = input.Substring(0, delim_index).Trim();
				switch (input[delim_index + 1]) {
					case 'L': // low to high sorting
						Direction = SortDir.ASC;
						break;
					case 'H': // high to low sorting
						Direction = SortDir.DEC;
						break;
					default:
						throw new InvalidConstraintException("That is not an acceptable sorting direction");
				}
			} else {
				StatName = input;
				Direction = SortDir.ASC;
			}
		}
	}

}
