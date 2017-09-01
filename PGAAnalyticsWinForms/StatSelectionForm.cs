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
				unselectedList.Items.Add(statname);
			}
			selectButton.Enabled = false;
			deselectButton.Enabled = false;
		}

		private void statsCheckBox_ItemCheck(object sender, ItemCheckEventArgs e)
		{
			
		}

		private void unselectedList_SelectedIndexChanged(object sender, EventArgs e)
		{
			selectButton.Enabled = true;
		}

		private void selectedList_SelectedIndexChanged(object sender, EventArgs e)
		{
			deselectButton.Enabled = true;
		}

		private void selectButton_Click(object sender, EventArgs e)
		{
			var index = unselectedList.SelectedIndex;
			selectedList.Items.Add(unselectedList.Items[index]);
			unselectedList.Items.RemoveAt(index);
			selectButton.Enabled = false;
		}

		private void deselectButton_Click(object sender, EventArgs e)
		{
			var index = selectedList.SelectedIndex;
			unselectedList.Items.Add(selectedList.Items[index]);
			selectedList.Items.RemoveAt(index);
			deselectButton.Enabled = false;
		}

		private void btnCalculate_Click(object sender, EventArgs e)
		{
			this.Hide();
			var vals = new List<string>();
			foreach( var sel in selectedList.Items) {
				vals.Add(sel.ToString());
			}
			var Top10ListSummary = new Top10Lists(_ps, vals);
			Top10ListSummary.Closed += (s, args) => this.Close();
			Top10ListSummary.Show();
		}

		private void selectedList_MouseDoubleClick(object sender, MouseEventArgs e) {
			int index = this.selectedList.IndexFromPoint(e.Location);
			if (index != ListBox.NoMatches) {
				var sortDir = new AscendingDescendingSelector();
				if( sortDir.ShowDialog() == DialogResult.OK) {
					selectedList.Items[index] += sortDir.Direction == SortDir.ASC ? " {L->H}" : " {H->L}";
				}
				sortDir.Close();
			}
		}
	}
}
