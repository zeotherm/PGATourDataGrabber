namespace PGADataScraper.API {
	public class ScorablePlayer {
		public string Name {get; private set;}
		public int Salary { get; private set; }
		public double Points { get; private set; }
		public int Multi { get; private set; }
		public int Tournaments { get; private set; }
		public int CutsMade { get; private set; }
		public int Top10s { get; private set; }
		public ScorablePlayer(string n, int s, double p, int m, int t, int c, int t10) {
			Name = n;
			Salary = s;
			Points = p;
			Multi = m;
			Tournaments = t;
			CutsMade = c;
			Top10s = t10;
		}

		public void IncreaseMulti() { Multi += 1; return; }
	}
}
