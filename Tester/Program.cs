using DominoEngine;
using Players;

public class Program
{
    public static void Main()
    {
        List<Player<int>> players = new();
        for (int i = 0; i < 4; i++) players.Add(new RandomPlayer<int>(i+1));
        ClassicGenerator generator = new ClassicGenerator(10);
        ClassicDealer<int> dealer = new ClassicDealer<int>(10);
        ClassicMatcher<int> matcher = new ClassicMatcher<int>();
        ClassicScorer scorer = new ClassicScorer();
        ClassicTurner<int> turner = new ClassicTurner<int>();
        ClassicFinisher<int> finisher = new ClassicFinisher<int>();
        Judge<int> judge = new Judge<int>(generator, dealer, turner, matcher, scorer, finisher);
        Game<int> game = new Game<int>(judge, players);

        foreach (var GameEstate in game)
        {
            System.Console.WriteLine(GameEstate);
            Console.ReadLine();
            // Console.Clear();

        }

        Console.ReadLine();
    }
}