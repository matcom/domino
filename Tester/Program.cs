using DominoEngine;
using Players;

public class Program
{
    public static void Main()
    {
        // List<string> names = new List<string>() {"Alex", "Omar", "Jky", "Anthuan"};
        List<Team<int>> teams = new() {
            new Team<int>(new List<Player<int>>(){
                new SmartPlayer<int>("Alex"),
                new SupportPlayer<int>("Jky"),
                new Botagorda<int>("Daniela")
            }),
            new Team<int>(new List<Player<int>>(){
                new DestroyerPlayer<int>("Omar"),
                new SelfishPlayer<int>("Anthuan")
            }),
            new Team<int>(new List<Player<int>>(){
                new Botagorda<int>("Anabel"),
                new Botagorda<int>("Sherlyn")
            }),
            new Team<int>(new List<Player<int>>(){
                new Botagorda<int>("Ciclanejo"),
                new Botagorda<int>("Raudel")
            })
        };
        // foreach (var name in names) 
        //     teams.Add(new Team<int>(new List<Player<int>>(){new Botagorda<int>(name)}));
        // Game<int> game = new Game<int>(new ClassicJudge(), teams);

        var judge = new ClassicJudge();
        var tournament = new AllVsAllTournament<int>().Compose(new AllVsAllTournament<int>());

        foreach (var game in tournament.SetJudge(judge).SetTeams(teams))
            foreach (var GameState in game) {
                // Console.Clear();
                System.Console.WriteLine(GameState);
                Console.ReadLine();
            }

        Console.ReadLine();
    }
}