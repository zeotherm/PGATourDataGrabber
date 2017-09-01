using System;
using System.Windows.Forms;

namespace PGAAnalyticsWinForms
{
	public enum SortDir { ASC, DEC };
	public partial class AscendingDescendingSelector : Form
	{
		private SortDir _dir;

		public SortDir Direction { get { return _dir; } }

		public AscendingDescendingSelector() {
			this.Icon = Properties.Resources.golf;
			InitializeComponent();
		}

		private void radioButton1_CheckedChanged(object sender, EventArgs e) {
			if (radioButton1.Checked) {
				radioButton2.Checked = false;
				_dir = SortDir.ASC;
			}
		}

		private void radioButton2_CheckedChanged(object sender, EventArgs e) {
			if (radioButton2.Checked) {
				radioButton1.Checked = false;
				_dir = SortDir.DEC;
			}
		}

		private void button1_Click(object sender, EventArgs e) {
			this.DialogResult = DialogResult.OK;
			this.Hide();
		}
	}
}
