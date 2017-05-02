using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace PGAAnalyticsWinForms
{
	public partial class MainForm : Form
	{
		public MainForm()
		{
			InitializeComponent();
			this.Icon = Properties.Resources.golf;
		}

		private void MainForm_Load(object sender, EventArgs e)
		{
			var Desktop = $@"C:\Users\{Environment.UserName}\Desktop";
			var DataDir = Path.Combine(Desktop, @"PGA Stats\");
			baseDirTxtBox.Text = DataDir;
		}
	}
}
