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
    public partial class PlayerSelectionDialog : Form {
        public string SelectedPlayer { get; private set; }
        public PlayerSelectionDialog(IEnumerable<PlayerStats> ps) {
            this.Icon = Properties.Resources.golf;
            InitializeComponent();
            foreach( var player in ps) {
                listBox1.Items.Add(player.player.FullName);
            }
        }

        public PlayerSelectionDialog(IEnumerable<ScorablePlayer> sps) {
            this.Icon = Properties.Resources.golf;
            InitializeComponent();
            foreach (var player in sps) {
                listBox1.Items.Add(player.Name);
            }
        }

        private void button1_Click(object sender, EventArgs e) {
            SelectedPlayer = listBox1.SelectedItem.ToString();
            this.DialogResult = DialogResult.OK;
            this.Hide();
        }

        private void cancelBtn_Click(object sender, EventArgs e) {
            this.DialogResult = DialogResult.Cancel;
            this.Hide();
        }
    }
}
