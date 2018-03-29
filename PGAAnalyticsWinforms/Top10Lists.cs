using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using PGADataScraper.API;
using System.Net;
using HtmlAgilityPack;
using System.Globalization;

namespace PGAAnalyticsWinForms
{
	public class PlayerESPNData {
		public string Name { get; set; }
		public int Events { get; set; }
		public int CutsMade { get; set; }
		public int Top10s { get; set; }
		public int Wins { get; set; }
		public int CupPoints { get; set; }
		public decimal Earnings { get; set; }
	}
	public partial class Top10Lists : Form
	{
		private List<Top10ListElement> t10 = new List<Top10ListElement>();
		private IEnumerable<PlayerStats> _ps;
		private Dictionary<string, List<NameValuePair>> _backing_data = new Dictionary<string, List<NameValuePair>>();
		private DataGridView[] dgvslots;
        private List<string> ManuallyAddedPlayers;
		private List<PlayerESPNData> espnPlayerData;
		private int slot;
		public Top10Lists(IEnumerable<PlayerStats> ps, IEnumerable<string> cats)
		{
			InitializeComponent();
			this.Icon = Properties.Resources.golf;
			foreach(var cat in cats) {
				t10.Add(new Top10ListElement(cat));
			}
			_ps = ps;
            ManuallyAddedPlayers = new List<string>();
            dgvslots = new DataGridView[tabControl1.TabCount + 1];

			dgvslots[0] = null;
			dgvslots[1] = dataGridView1;
			dgvslots[2] = dataGridView2;
			dgvslots[3] = dataGridView3;
			dgvslots[4] = dataGridView4;
			dgvslots[5] = dataGridView5;
			dgvslots[6] = dataGridView6;
			slot = 0;

			// Populate ESPN results data set
			var startOffsets = Enumerable.Range(0, 5).Select(x => x * 40);
			espnPlayerData = new List<PlayerESPNData>();
			foreach (var offset in startOffsets) {
				var webscrape = GetESPNData(offset);

				var espn = new HtmlAgilityPack.HtmlDocument();
				espn.LoadHtml(webscrape);
				var values = espn.DocumentNode.SelectSingleNode("/html[1]/body[1]/div[1]/div[2]/div[1]/div[2]/div[3]/div[1]/div[1]/div[1]/table[1]").ChildNodes;
				foreach (var node in values.Skip(1)) {
					//Console.WriteLine("What");
					var name = node.ChildNodes[1].InnerText;
					if (!Int32.TryParse(node.ChildNodes[3].InnerText, out int events)) continue;
					if (!Int32.TryParse(node.ChildNodes[5].InnerText, out int cutsmade)) continue;
					if (!Int32.TryParse(node.ChildNodes[6].InnerText, out int top10s)) continue;
					if (!Int32.TryParse(node.ChildNodes[7].InnerText, out int wins)) continue;
					if (!Int32.TryParse(node.ChildNodes[8].InnerText, NumberStyles.AllowThousands, CultureInfo.InvariantCulture, out int points)) continue;
					if (!Decimal.TryParse(node.ChildNodes[9].InnerText, NumberStyles.AllowCurrencySymbol | NumberStyles.AllowDecimalPoint | NumberStyles.AllowThousands, new CultureInfo("en-US"), out decimal earns)) continue;

					espnPlayerData.Add(new PlayerESPNData {
						Name = name,
						Events = events,
						CutsMade = cutsmade,
						Top10s = top10s,
						Wins = wins,
						CupPoints = points,
						Earnings = earns
					});
				}
			}

		}

		private void Top10Lists_Load(object sender, EventArgs e)
		{
			foreach( var cat in t10) {
				var currStat = cat.StatName;
				if (slot >= tabControl1.TabCount) {
					MessageBox.Show($"A max of {tabControl1.TabCount} Top10 categories is allowed, discarding extras", "Notice", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
					break;
				}

				tabControl1.TabPages[slot].Text = currStat;
				slot += 1;
				var player_single_stat = _ps.Select(p => new {
					Name = p.player.FullName,
					Stat = p.stats.Where(s => s.Name == cat.StatName).Select(s => s.Info).First()
				});
				if (cat.Direction == SortDir.ASC) {
					_backing_data.Add(cat.StatName, 
									  player_single_stat.Where(s => s.Stat.Value != "N/A")
														.OrderBy(t => t.Stat.SortValue)
														.Take(15)
														.Select(n => new NameValuePair(n.Name, n.Stat.Value))
														.ToList());
				} else {
					_backing_data.Add(cat.StatName, 
									  player_single_stat.Where(s => s.Stat.Value != "N/A")
														.OrderByDescending(t => t.Stat.SortValue)
														.Take(15)
														.Select(n => new NameValuePair(n.Name, n.Stat.Value))
														.ToList());
				}
				UpdateGridView(slot, cat.StatName);

			}
			// clear out unused tabs
			for (int indx = tabControl1.TabCount - 1; indx >= slot; indx--) {
				tabControl1.TabPages.RemoveAt(indx);
			}
		}

		private void UpdateGridView(int s, string statname) {
			dgvslots[s].Rows.Clear();

			foreach (var ndpd in _backing_data[statname]) {
				int n = dgvslots[s].Rows.Add();
				dgvslots[s].Rows[n].Cells[0].Value = ndpd.Name;
				dgvslots[s].Rows[n].Cells[1].Value = ndpd.Value;
				if (n >= 9) break; // Only display 10, but allow for extras to be in the backing data
			}
			return;
		}

		private void dataGridView1_RowHeaderMouseDoubleClick(object sender, DataGridViewCellMouseEventArgs e) {
			PromptForRemoval(1, e.RowIndex);
			return;
		}

		private void PromptForRemoval( int i, int r) {
			if (MessageBox.Show($"Remove {dgvslots[i].Rows[r].Cells[0].Value} from the list", "Remove?", MessageBoxButtons.OKCancel, MessageBoxIcon.Information) == DialogResult.OK) {
				var statname = tabControl1.TabPages[i-1].Text;
				_backing_data[statname].RemoveAt(r);
				UpdateGridView(i, statname);
			}
		}

		private void dataGridView2_RowHeaderMouseDoubleClick(object sender, DataGridViewCellMouseEventArgs e) {
			PromptForRemoval(2, e.RowIndex);
		}

		private void dataGridView3_RowHeaderMouseDoubleClick(object sender, DataGridViewCellMouseEventArgs e) {
			PromptForRemoval(3, e.RowIndex);
		}

		private void dataGridView4_RowHeaderMouseDoubleClick(object sender, DataGridViewCellMouseEventArgs e) {
			PromptForRemoval(4, e.RowIndex);
		}

		private void dataGridView5_RowHeaderMouseDoubleClick(object sender, DataGridViewCellMouseEventArgs e) {
			PromptForRemoval(5, e.RowIndex);
		}

		private void dataGridView6_RowHeaderMouseDoubleClick(object sender, DataGridViewCellMouseEventArgs e) {
			PromptForRemoval(6, e.RowIndex);
		}

		private void button1_Click(object sender, EventArgs e) {
			this.Hide();
			Dictionary<string, ScorablePlayer> avail_players = new Dictionary<string, ScorablePlayer>();
			foreach( var stat in _backing_data.Keys) {
				var players_in_stat = _backing_data[stat].Take(10);
				foreach( var player in players_in_stat) {
					if (avail_players.ContainsKey(player.Name)) {
						avail_players[player.Name].IncreaseMulti();
					} else {
                        AddPlayerByName(avail_players, player.Name);
					}
					
				}
			}
            // Add in any manually selected players
            foreach( var manPlayer in ManuallyAddedPlayers) {
                AddPlayerByName(avail_players, manPlayer);
            }
			var AvailablePlayers = new AvailablePlayersDisplayForm(avail_players.Values.ToList());
			AvailablePlayers.Closed += (s, args) => this.Close();
			AvailablePlayers.Show();
		}
		private string GetESPNData(int startPlayer) {
			var wc = new WebClient();
			// 2017 data: var webpath = String.Format(@"http://www.pgatour.com/data/players/{0}/2017stat.json", p.ID);
			var webpath = String.Format($@"http://www.espn.com/golf/statistics/_/count/{startPlayer.ToString()}");
			//OnPlayerDownloadAttempt(new DownloadingPlayerEventArgs { Message = $"Downloading data for player {p.FullName}", TimeGrabbed = DateTime.Now });
			return wc.DownloadString(webpath);
		}
		private void AddPlayerByName(Dictionary<string, ScorablePlayer> a_players, string name) {
            var pstat = _ps.Where(p => p.player.FullName == name).First();
            var salary = (int)pstat.Salary;
            var points = pstat.Points;
            var multi = 1;
			var playerNames = espnPlayerData.Select(p => p.Name).OrderBy(p => p).ToList();
			var espnPlayerQuery = espnPlayerData.Where(epd => epd.Name == name);
			//PlayerESPNData espnPlayer;
			PlayerESPNData espnPlayer = espnPlayerQuery.Any() ? espnPlayerQuery.First() : null;
			int cutsMade;
			int tournaments;
			int top10s;
			decimal earning;
			int cup_points;
			if (espnPlayer != null) {
				cutsMade = espnPlayer.CutsMade;
				tournaments = espnPlayer.Events;
				top10s = espnPlayer.Top10s;
				earning = espnPlayer.Earnings;
				cup_points = espnPlayer.CupPoints;
			} else {
				cutsMade = tournaments = top10s = cup_points = 0;
				earning = 0.00M;
			}
				
				//pstat.stats.Where(s => s.Name == @"# of Top 10's").Select(t => t.Info.Value).First();
    //        if (!int.TryParse(nt10s, out int top10s)) {
    //            top10s = 0;
    //        }
            a_players.Add(name,
                             new ScorablePlayer(name,
                                                salary,
                                                points,
                                                multi,
                                                tournaments,
                                                cutsMade,
                                                top10s,
												earning,
												cup_points));
            return;
        }

        private void button2_Click(object sender, EventArgs e) {
            var psd = new PlayerSelectionDialog(_ps);
            if( psd.ShowDialog() == DialogResult.OK) {
                ManuallyAddedPlayers.Add(psd.SelectedPlayer);
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

	internal sealed class NameValuePair {
		public string Name { get; private set; }
		public string Value { get; private set; }
		public NameValuePair( string n, string v) {
			Name = n;
			Value = v;
		}
	}

}
