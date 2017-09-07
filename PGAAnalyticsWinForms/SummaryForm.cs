using PGADataScraper.API;
using PGADataScraper.API.Interfaces;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace PGAAnalyticsWinForms {
	public partial class SummaryForm : Form {
		private Task<IEnumerable<IEnumerable<ScorablePlayer>>> teams;
		private DataGridView[] dgvslots;
		private readonly IEnumerable<ScorablePlayer> sps;
		private readonly IEnumerable<ScorablePlayer> keepers;
		private readonly ITeamChooser chooser;
		private int slot;
		public SummaryForm(IEnumerable<ScorablePlayer> sps, IEnumerable<ScorablePlayer> keepers, ITeamChooser itc) {
			this.Icon = Properties.Resources.golf;
			InitializeComponent();
			chooser = itc;
			this.sps = sps;
			this.keepers = keepers;
			//TODO: This needs a splash screen
			
			dgvslots = new DataGridView[tabControl1.TabCount + 1];

			dgvslots[0] = null;
			dgvslots[1] = dataGridView1;
			dgvslots[2] = dataGridView2;
			dgvslots[3] = dataGridView3;
			dgvslots[4] = dataGridView4;
			dgvslots[5] = dataGridView5;
			slot = 0;
		}

		private void button1_Click(object sender, EventArgs e) {
			this.Close();
		}

		private void SummaryForm_Load(object sender, EventArgs e) {
			progressBar1.Show();
			teams = chooser.ComputeTopTeams(sps, keepers);
			teams.ContinueWith(((best_teams) => {
				var best = best_teams.Result;
				
				foreach (var team in best) {
					slot += 1;
					foreach (var player in team) {
						int n = dgvslots[slot].Rows.Add();
						dgvslots[slot].Rows[n].Cells[0].Value = player.Name;
						dgvslots[slot].Rows[n].Cells[1].Value = player.Salary;
						dgvslots[slot].Rows[n].Cells[2].Value = player.Points;
					}
					var team_points = team.Sum(p => p.Points);
					tabControl1.TabPages[slot - 1].Text += $" - {team_points}";
				}
			}), TaskScheduler.FromCurrentSynchronizationContext());
			progressBar1.Hide();
		}
	}
}
