namespace DominoEngine;

public class Game
{
    public void LetsPlay()
    {
        List<Hand<int>> hands = new List<Hand<int>>();
        for (int i = 0; i < 4; i++)
        {
            hands.Add(new Hand<int>());
        }
        ClassicGenerator generator = new ClassicGenerator(10);
        ClassicDealer dealer = new ClassicDealer(10, hands, generator.Generate());
        List<Player<int>> players = new List<Player<int>>();
        foreach (var item in hands)
        {
            players.Add(new RandomPlayer<int>(item));
        }
        ITurner<int> turner = new ClassicTurner(players);
        Dictionary<Player<int>, Hand<int>> Hands = new Dictionary<Player<int>, Hand<int>>();
        foreach (var item in players)
        {
            Hands.Add(item, item.Hand.Clone());
        }
        IFinisher<int> finisher = new ClassicFinisher(Hands);
        PlayInfo<int> playInfo = new PlayInfo<int>(Hands, new List<Player<int>>());
        Rules<int> rules = new Rules<int>(turner, finisher);
        Judge<int> judge = new Judge<int>(playInfo, rules);
        judge.Play();
    }

}