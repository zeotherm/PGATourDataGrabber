using PGADataScaper.API.Interfaces;
using System;
using System.Threading.Tasks;
using System.IO;
using System.Net;
using System.Threading;
using System.Collections.Generic;

namespace PGADataScaper.API
{
	public class PlayerWebDataGather : IPlayerWebDataGather
	{
		private readonly WebClient wc;
		private DirectoryInfo root_di, data_di;
		private readonly IPlayerGather _pg;
		private readonly IProgress<int> _progress;
		private readonly CancellationToken _ct;
		private readonly List<string> _error_names;
		public event EventHandler<DownloadingPlayerEventArgs> DownloadingPlayer;
		public event EventHandler<WritingPlayerEventArgs> WritingPlayer;
		public event EventHandler<DownloadingPlayerErrorEventArgs> DownloadPlayerError;
		public PlayerWebDataGather( string root, IPlayerGather pg) { // Root is the Desktop\PGA Stats directory which holds Players.xml and the statCat.json file
			root_di = new DirectoryInfo(root);
			wc = new WebClient();
			_pg = pg;
			_progress = null;
			_ct = CancellationToken.None;
			_error_names = new List<string>();
		}

		public PlayerWebDataGather(string root, IPlayerGather pg, IProgress<int> prog, CancellationToken ct)
		{ // Root is the Desktop\PGA Stats directory which holds Players.xml and the statCat.json file
			root_di = new DirectoryInfo(root);
			wc = new WebClient();
			_pg = pg;
			_progress = prog;
			_ct = ct;
			_error_names = new List<string>();
		}

		public async Task GatherAndWriteAllPlayerStats() {
			var path = Path.Combine(root_di.FullName, String.Format("Week_{0}", DateTime.Now.Iso8601WeekOfYear().ToString("D2")));
			if (!Directory.Exists(path)) {
				data_di = Directory.CreateDirectory(path);
			} else {
				data_di = new DirectoryInfo(path);
			}
			var playersProcessed = 0;
			PopulateErrorList();
			foreach (var player in _pg.ReadPlayerList()) {
				try {
					if (!File.Exists(Path.Combine(data_di.FullName, player.FileName)) && !_error_names.Contains(player.FullName)) { // TODO: Check if an error file exists and parse out missing players to do similar checks
						await WritePlayerData(player, await GatherPlayerData(player));
					}
				} catch (WebException) {
					OnPlayerDownloadFailed( new DownloadingPlayerErrorEventArgs { Message = $"An error occured; the program could not download player data: {player.FullName}", TimeError = DateTime.Now, ErrorDirectory = data_di });
				} finally {
					_progress?.Report(++playersProcessed);
				}
				_ct.ThrowIfCancellationRequested();
			}
		}

		private void PopulateErrorList()
		{
			var errfile = Path.Combine(data_di.FullName, "error.log");
			if ( File.Exists(errfile)) {
				var lines = File.ReadAllLines(errfile);
				foreach( var line in lines)
				{
					if( !String.IsNullOrWhiteSpace(line)) _error_names.Add(line.Substring(line.LastIndexOf(':')+1).Trim());
				}
			}
			return;
		}

		private async Task WritePlayerData( Player p, string statData) {
			var file_path = Path.Combine(data_di.FullName, p.FileName);
			using (var player_file = new StreamWriter(file_path)) {
				OnPlayerWriteAttempt(new WritingPlayerEventArgs { Message = $"Writing file {file_path}", TimeWritten = DateTime.Now });
				await player_file.WriteLineAsync(statData);
			}
		}

		private async Task<string> GatherPlayerData(Player p) {
			var webpath = String.Format(@"http://www.pgatour.com/data/players/{0}/2017stat.json", p.ID);
			OnPlayerDownloadAttempt(new DownloadingPlayerEventArgs { Message = $"Downloading data for player {p.FullName}", TimeGrabbed = DateTime.Now });
			return await wc.DownloadStringTaskAsync(webpath);
		}

		protected virtual void OnPlayerDownloadAttempt(DownloadingPlayerEventArgs e)
		{
			DownloadingPlayer?.Invoke(this, e);
		}

		protected virtual void OnPlayerWriteAttempt(WritingPlayerEventArgs e )
		{
			WritingPlayer?.Invoke(this, e);
		}

		protected virtual void OnPlayerDownloadFailed(DownloadingPlayerErrorEventArgs e)
		{
			DownloadPlayerError?.Invoke(this, e);
		}
	}

	public class DownloadingPlayerEventArgs : EventArgs
	{
		public string Message { get; set; }
		public DateTime TimeGrabbed { get; set; }
	}

	public class WritingPlayerEventArgs : EventArgs
	{
		public string Message { get; set; }
		public DateTime TimeWritten { get; set; }
	}

	public class DownloadingPlayerErrorEventArgs : EventArgs
	{
		public string Message { get; set; }
		public DateTime TimeError { get; set; }
		public DirectoryInfo ErrorDirectory { get; set; }
	}
}
