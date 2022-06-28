using DominoEngine;
using Players;

public class Program
{
    public static void Main()
    {
        List<string> names = new List<string>() {"Alex", "Omar", "Jky", "Anthuan"};
        List<Team<int>> teams = new();
        foreach (var name in names) 
            teams.Add(new Team<int>(new List<Player<int>>(){new Botagorda<int>(name)}));
        Game<int> game = new Game<int>(new ClassicJudge(), teams);

        foreach (var GameEstate in game)
        {
            System.Console.WriteLine(GameEstate);
            // Console.ReadLine();
        }

        Console.ReadLine();
    }
}