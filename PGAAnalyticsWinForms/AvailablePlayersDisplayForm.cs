using PGADataScraper.API;
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
	public partial class AvailablePlayersDisplayForm : Form {
		private readonly IEnumerable<ScorablePlayer> _sps;
		private Task<IEnumerable<ScorablePlayer>[]> teams;

		public AvailablePlayersDisplayForm(IEnumerable<ScorablePlayer> sps) {
			this.Icon = Properties.Resources.golf;
			InitializeComponent();
			_sps = sps;
			foreach( var player in _sps) {
				int n = dataGridView1.Rows.Add();
				dataGridView1.Rows[n].Cells[0].Value = player.Name;
				dataGridView1.Rows[n].Cells[1].Value = player.Salary;
				dataGridView1.Rows[n].Cells[2].Value = player.Points;
				dataGridView1.Rows[n].Cells[3].Value = player.Multi;
				dataGridView1.Rows[n].Cells[4].Value = player.Tournaments;
				dataGridView1.Rows[n].Cells[5].Value = player.CutsMade;
				dataGridView1.Rows[n].Cells[6].Value = player.Top10s;
			}
		}

		private void button1_Click(object sender, EventArgs e) {
			teams = new SimpleTeamChooser().ComputeTopTeams(_sps, new List<ScorablePlayer>());
			var plzWait = new OptimizingWaitForm();
			plzWait.Show();
			teams.ContinueWith(((best_teams) => {
				var best = best_teams.Result;
				plzWait.Hide();
				this.Hide();
				var Summary = new SummaryForm(best);
				Summary.Closed += (s, args) => this.Close();
				Summary.Show();
			}), TaskScheduler.FromCurrentSynchronizationContext());

			
		}
	}
}
