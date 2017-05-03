using PGADataScaper.API;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace PGAAnalyticsWinForms
{
	public partial class StatSelectionForm : Form
	{
		private IEnumerable<PlayerStats> _ps;
		public StatSelectionForm(IEnumerable<PlayerStats> ps) : base()
		{
			InitializeComponent();
			this.Icon = Properties.Resources.golf;
			_ps = ps;

			foreach ( var statname in _ps.Take(1).First().stats.Select(s => s.Name).OrderBy(s => s))
			{
				statsCheckBox.Items.Add(statname, false);
			}
		}

		private void statsCheckBox_ItemCheck(object sender, ItemCheckEventArgs e)
		{
			if( e.CurrentValue == CheckState.Unchecked)
			{
				listView1.Items.Add(new ListViewItem(statsCheckBox.Items[e.Index].ToString()));
			}
		}
	}
}
