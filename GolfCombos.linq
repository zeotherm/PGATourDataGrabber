<Query Kind="Program" />

void Main()
{
	var A_Hadwin = new Player {Name = "Adam Hadwin", Salary = 8400, AvgPts = 87.333};
	var A_Lahiri = new Player {Name = "Anirban Lahiri", Salary = 6700, AvgPts = 77.143};
	var B_Koepka = new Player {Name = "Brooks Koepka", Salary = 6900, AvgPts = 47.333};
	var B_An = new Player {Name = "Byeong Hun An", Salary = 7000, AvgPts = 72.583};
	var C_Hoffman = new Player {Name = "Charley Hoffman", Salary = 6600, AvgPts = 65.429};
	var D_Willett = new Player {Name = "Danny Willett", Salary = 7300, AvgPts = 59};
	var F_Molinari = new Player {Name = "Francesco Molinari", Salary = 8600, AvgPts = 68.6};
	var G_Owen = new Player {Name = "Greg Owen", Salary = 7200, AvgPts = 64.75};
	var H_Stenson = new Player {Name = "Henrik Stenson", Salary = 11500, AvgPts = 81.5};
	var H_Matsuyama = new Player {Name = "Hideki Matsuyama", Salary = 10300, AvgPts = 89.571};
	var H_Swafford = new Player {Name = "Hudson Swafford", Salary = 7200, AvgPts = 58.333};
	var J_Day = new Player {Name = "Jason Day", Salary = 10600, AvgPts = 69.625};
	var J_Rose = new Player {Name = "Justin Rose", Salary = 9500, AvgPts = 74.357};
	var K_Kisner = new Player {Name = "Kevin Kisner", Salary = 7600, AvgPts = 83.1};
	var K_Stanley = new Player {Name = "Kyle Stanley", Salary = 7400, AvgPts = 64.333};
	var L_Oosthuizen = new Player {Name = "Louis Oosthuizen", Salary = 9300, AvgPts = 79.7};
	var L_Glover = new Player {Name = "Lucas Glover", Salary = 6700, AvgPts = 72.5};
	var L_List = new Player {Name = "Luke List", Salary = 7200, AvgPts = 57.826};
	var M_Kaymer = new Player {Name = "Martin Kaymer", Salary = 7400, AvgPts = 83.7};
	var P_Perez = new Player {Name = "Pat Perez", Salary = 7000, AvgPts = 67};
	var P_Casey = new Player {Name = "Paul Casey", Salary = 8800, AvgPts = 63.583};
	var R_Fowler = new Player {Name = "Rickie Fowler", Salary = 9900, AvgPts = 82};
	var R_Mcilroy = new Player {Name = "Rory McIlroy", Salary = 12000, AvgPts = 113.25};
	var S_Ohair = new Player {Name = "Sean O'Hair", Salary = 7300, AvgPts = 65.286};
	var T_Pieters = new Player {Name = "Thomas Pieters", Salary = 8700, AvgPts = 68.2};
	var T_Fleetwood = new Player {Name = "Tommy Fleetwood", Salary = 7200, AvgPts = 74.167};
	
	
	var AllPlayers = new[] {A_Hadwin, A_Lahiri, B_Koepka, B_An, C_Hoffman, D_Willett, F_Molinari, G_Owen, H_Stenson,
							H_Matsuyama, H_Swafford, J_Day, J_Rose, K_Kisner, K_Stanley, L_Oosthuizen, L_Glover, 
							L_List, M_Kaymer, P_Perez, P_Casey, R_Fowler, R_Mcilroy, S_Ohair, T_Pieters, T_Fleetwood};
							
	var Keepers = new [] {R_Mcilroy};
	var AvailablePlayers = AllPlayers.Except(Keepers);
	var remainingSalary = 50000 - Keepers.Sum(t=>t.Salary);
	var PotentialTeams = AvailablePlayers.DifferentCombinations(6-Keepers.Count());
	var ValidTeams = PotentialTeams.Where(t => t.Select(p=>p.Salary).Sum() < remainingSalary);
	var BestTeams = ValidTeams.Select(vt => vt.Concat(Keepers)).OrderByDescending(t => t.Select(p=>p.AvgPts).Sum()).Take(5);
	
	BestTeams.Dump();
	
	//BestTeams.Dump();
	
	
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