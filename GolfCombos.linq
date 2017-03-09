<Query Kind="Program" />

void Main()
{
	var Bill_Hass = new Player {Name = "Bill Haas", Salary = 9200, AvgPts = 78.875};
	var Martin_Laird = new Player {Name = "Martin Laird", Salary = 7400, AvgPts = 72.6};
	var Kyle_Stanley = new Player {Name = "Kyle Stanley", Salary = 7000, AvgPts = 64.8};
	var Henrik_Stenson = new Player {Name = "Henrik Stenson", Salary = 11700, AvgPts = 81.625};
	var Luke_List = new Player {Name = "Luke List", Salary = 7100, AvgPts = 55.583};
	var Justin_Thomas = new Player { Name = "Justin Thomas", Salary = 11900, AvgPts = 90.583};
	var Gary_Woodland = new Player { Name = "Gary Woodland", Salary = 8900, AvgPts = 79.417};
	var Ollie_Schneiderjans = new Player {Name = "Ollie Schneiderjans", Salary = 7100, AvgPts = 74.333};
	var Chris_Kirk = new Player {Name = "Chris Kirk", Salary = 6900, AvgPts = 64.5};
	
	var AllPlayers = new[] {Bill_Hass, Martin_Laird, Kyle_Stanley, Henrik_Stenson, Luke_List, Justin_Thomas, Gary_Woodland, Ollie_Schneiderjans, Chris_Kirk};
	var PotentialTeams = AllPlayers.DifferentCombinations(6);
	var ValidTeams = PotentialTeams.Where(t => t.Select(p=>p.Salary).Sum() < 50000);
	var BestTeams = ValidTeams.OrderByDescending(t => t.Select(p=>p.AvgPts).Sum()).Take(5);
	
	BestTeams.Dump();
	
	
	//results.Dump();
}

public class TestVal {
	public string Name {get; set;}
	public int Val {get; set;}
}

public class Player {
	public string Name {get; set;}
	public int Salary {get; set;}
	public double AvgPts {get; set;}
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