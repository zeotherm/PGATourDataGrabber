<Query Kind="Program" />

void Main()
{
	List<Player> AllPlayers = new List<Player>();
	using( var inputData = new StreamReader(@"C:\Users\Matt\Desktop\PGA Stats\Week_19\players.csv")) {
		string line;
		while( (line = inputData.ReadLine()) != null) {
			var info = line.Split(',');
			AllPlayers.Add( new Player {Name = info[0], 
										Salary = Int32.Parse(info[1]), 
										BasePts = Double.Parse(info[2]), 
										Multi = Int32.Parse(info[3]), 
										Tournaments = Int32.Parse(info[5]),
										CutsMade = Int32.Parse(info[4]),
										Top10s = Int32.Parse(info[6]),
										Score = info[7]});
		}
	}
	
	foreach( var p in AllPlayers) {
		p.ModifyScore();
	}
	
	//players.Dump("Everyone");
						
	var Keepers = new Player[] {}; //AllPlayers.Where(p=>p.Name == "Dustin Johnson"); // 
	var AvailablePlayers = AllPlayers.Except(Keepers);
	var remainingSalary = 50000 - Keepers.Sum(t=>t.Salary);
	//var PotentialTeams = AvailablePlayers.Where(p=>p.Multi).DifferentCombinations(6-Keepers.Count());
	var PotentialTeams = AvailablePlayers.DifferentCombinations(6-Keepers.Count());
	var ValidTeams = PotentialTeams.Where(t => t.Select(p=>p.Salary).Sum() < remainingSalary);
	var BestTeams = ValidTeams.Select(vt => vt.Concat(Keepers)).OrderByDescending(t => t.Select(p=>p.Pts).Sum()).Take(5);
	
	BestTeams.Dump();
	
	//BestTeams.Dump();
	
	
	//results.Dump();
}

public class TestVal {
	public string Name {get; set;}
	public int Val {get; set;}
}

public class Player {
	private static double MultiListBonus = 0.05; // 5% bonus for each list in > 1
	private static double CutsMadeThreshhold = 0.85;
	private static double CutsMadeBonusValue = 0.075; // 7.5% bonus if make more than 85% of their cuts
	private static double CutsMissedPenaltyVal = 0.05; // 5% penalty for each missed cut below 85% threshold
	private static double Top10BonusValue = 0.08; // 8% bonus for each Top 10s made
	public string Name {get; set;}
	public int Salary {get; set;}
	public double Pts {get; set;}
	public double BasePts {get; set;}
	public int Multi {get; set;}
	public int Tournaments {get; set;}
	public int CutsMade {get; set;}
	public int Top10s {get; set;}
	public string Score {get; set;}
	
	public void ModifyScore() {
		Pts = BasePts;
		MultiBonus();
		CutsMadeBonus();
		CutsMissedPenalty();
		Top10Bonus();
		return;
	}
	
	public void Top10Bonus() {
		Pts += BasePts * (Math.Pow(1.0+Top10BonusValue,Top10s)-1.0);
	}
	
	public void CutsMissedPenalty() {
		var cutratio = (double)CutsMade/(double)Tournaments;
		if( cutratio < CutsMadeThreshhold) {
			var missed = Convert.ToInt32(CutsMadeThreshhold * Tournaments) - CutsMade; // this is number of cuts below the 85% threshold
			//Name.Dump("Player Gets Cut Missed Penalty");
			//missed.Dump("Below 85% Threshold");
			//CutsMade.Dump("Cuts Made");
			Pts += BasePts * (Math.Pow(1.0+CutsMissedPenaltyVal, -missed) - 1.0);
		}
		
	}
	
	public void CutsMadeBonus() {
		var cutratio = (double)CutsMade/(double)Tournaments;
		if( cutratio >= CutsMadeThreshhold) {
			//Name.Dump("Player Gets Cut Made Bonus");
			//cutratio.Dump();
			Pts += BasePts*(CutsMadeBonusValue);
		}
		if( CutsMade == Tournaments) Pts += 0.1*BasePts;
	}
	public void MultiBonus() {
		Pts += BasePts * (Math.Pow(1.0+MultiListBonus, Multi-1) - 1.0);
	}
}

// Define other methods and classes here
public static class Ex
{
    public static IEnumerable<IEnumerable<T>> DifferentCombinations<T>(this IEnumerable<T> elements, int k)
    {
        return k == 0 ? new[] { new T[0] } :
          elements.SelectMany((e, i) =>
            elements.Skip(i + 1).DifferentCombinations(k - 1).Select(c => (new[] {e}).Concat(c)));
    }
}

void ModifyPlayerScore( Player p) {
	
}