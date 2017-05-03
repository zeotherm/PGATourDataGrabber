using PGADataScaper.API;
using PGADataScaper.API.Interfaces;
using System;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace PGAAnalyticsWinForms
{
	public partial class MainForm : Form
	{
		private string DataDir;
		private CancellationTokenSource cts;
		public MainForm()
		{
			InitializeComponent();
			this.Icon = Properties.Resources.golf;
			cts = new CancellationTokenSource();
		}

		private void MainForm_Load(object sender, EventArgs e)
		{
			var Desktop = $@"C:\Users\{Environment.UserName}\Desktop";
			DataDir = Path.Combine(Desktop, @"PGA Stats\");
			baseDirTxtBox.Text = DataDir;
		}


		void DownloadPlayerNotice(object sender, DownloadingPlayerEventArgs e)
		{
			this.outputBox.AppendText($"{e.TimeGrabbed}: {e.Message}" + Environment.NewLine);
			return;
		}

		void WritePlayerNotice(object sender, WritingPlayerEventArgs e)
		{
			this.outputBox.AppendText($"{e.TimeWritten}: {e.Message}" + Environment.NewLine);
			return;
		}

		void WritePlayerError(object sender, DownloadingPlayerErrorEventArgs e)
		{
			int length = outputBox.TextLength;  // at end of text
			var message = $"{ e.TimeError}: { e.Message}" + Environment.NewLine;
			outputBox.AppendText(message);
			outputBox.SelectionStart = length;
			outputBox.SelectionLength = message.Length;
			outputBox.SelectionColor = Color.DarkOrange;
			outputBox.DeselectAll();

			using (var error_file = new StreamWriter(Path.Combine(e.ErrorDirectory.FullName, "error.log"), true))
			{
				error_file.WriteLine(message);
			}
		}

		void WritePlayerStats(object sender, PopulatingStatsEventArgs e)
		{
			outputBox.AppendText($"{e.TimePopulated}: {e.Message}" + Environment.NewLine);
			return;
		}

		void WritePlayerStatsError(object sender, PopulatingStatsErrorEventArgs e)
		{
			int length = outputBox.TextLength;  // at end of text
			var message = $"{ e.TimeError}: { e.Message}" + Environment.NewLine;
			outputBox.AppendText(message);
			outputBox.SelectionStart = length;
			outputBox.SelectionLength = message.Length;
			outputBox.SelectionColor = Color.Red;
			outputBox.DeselectAll();

		}
		
		private async void startBtn_Click(object sender, EventArgs e)
		{
			var WorkingDirectory = Path.Combine(DataDir, $"Week_{DateTime.Now.Iso8601WeekOfYear().ToString("D2")}");
			var PlayerInfo = new PlayerGather(DataDir);
			var Serializer = new StatDictSerializer(DataDir);
			var StatMaker = new StatPopulator(WorkingDirectory, Serializer, PlayerInfo);
			stopButton.Enabled = true;
			StatMaker.PopulatingAttempt += WritePlayerStats;
			StatMaker.PopulatingError += WritePlayerStatsError;

			try {
				var web_gather = new PlayerWebDataGather(DataDir, PlayerInfo,
					new Progress<int>(v => {
						progressBar1.Value = v * 100 / PlayerInfo.Count();
						progressLabel.Text = $"{v}/{PlayerInfo.Count()}";
					}), cts.Token);
				web_gather.DownloadingPlayer += DownloadPlayerNotice;
				web_gather.DownloadPlayerError += WritePlayerError;
				web_gather.WritingPlayer += WritePlayerNotice;

				await web_gather.GatherAndWriteAllPlayerStats();

			} catch( OperationCanceledException) {
				ReportErrorToTextBox("Operation cancelled at user request");
				return;
			}

			DialogResult done = MessageBox.Show("Press OK when DKSalaries.csv is put into this week's directory...", "Salary Input", MessageBoxButtons.OKCancel, MessageBoxIcon.Exclamation);
			if( done == DialogResult.Cancel)
			{
				ReportErrorToTextBox("DKSalaries.csv is needed before continuing");
			}

			if (!File.Exists(Path.Combine(DataDir, "statCats.json")))
			{
				// Process data directory
				var statCats = new StatisticsDictionary(WorkingDirectory).GenerateSkeleton();
				Serializer.Serialize(statCats);
			}

			var all_player_stats = StatMaker.PopulateAllPlayerStats();

			nextButton.Enabled = true;
			
			using (var output = new StreamWriter(Path.Combine(WorkingDirectory, "results-dkonly.csv")))
			{
				output.WriteLine(all_player_stats[0].ToCSVHeaderLine());
				foreach (var p in all_player_stats.Where(p => p.IsDKSet()).Select(p => p))
				{
					output.WriteLine(p.ToCSVLineEntry());
				}
			}
			stopButton.Enabled = false;
		}

		private void stopButton_Click(object sender, EventArgs e)
		{
			cts.Cancel();
		}

		private void ReportErrorToTextBox( string message)
		{
			int length = outputBox.TextLength;  // at end of text
			outputBox.AppendText(message);
			outputBox.SelectionStart = length;
			outputBox.SelectionLength = message.Length;
			outputBox.SelectionColor = Color.Red;
			outputBox.DeselectAll();
			stopButton.Enabled = false;
			return;
		}
	}
}
