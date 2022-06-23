namespace DominoEngine;

public class ClassicScorer : IScorer<int>
{
    public ClassicScorer() { }

    public double Scorer(Partida<int> partida, Move<int> move)
    {
        return move.Head + move.Tail;
    }

    public double TokenScorer(Ficha<int> token) => token.Head + token.Tail;

    public Team<int> Winner(Partida<int> partida)
    {
        foreach (var player in partida.Players().Where(x => partida.Hands[x].IsEmpty()))
            return partida.TeamOf(player);
        return partida.TeamOf(partida.Hands.Select(x => (x.Key, x.Value.Sum(x => TokenScorer(x)))).MinBy(x => x.Item2).Key);
    }
}

public class ClassicFinisher<T> : IFinisher<T>
{
    public ClassicFinisher() { }

    public bool GameOver(Partida<T> partida)
    {
        return AllCheck(partida) || PlayerEnd(partida);
    }

    public bool AllCheck(Partida<T> partida)
    {
        foreach (var player in partida.Players()){
            var lastmove = new Move<T>(partida.PlayerId(player));
            foreach (var move in partida.Board.Where(x => x.PlayerId == partida.PlayerId(player)))
                lastmove = move;

            if (!lastmove.Check || partida.Board.Count < partida.Players().Count()) return false;
        }

        return true;
    }

    public bool PlayerEnd(Partida<T> partida)
    {
        foreach (var player in partida.Players())
            if (partida.Hand(player).Count() == 0) return true;

        return false;
    }
}

public class ClassicMatcher<T> : IMatcher<T>
{
    List<int>? validsTurns;

    public ClassicMatcher() { }

    public IEnumerable<Move<T>> CanMatch(Partida<T> partida, IEnumerable<Move<T>> enumerable)
    {
        ValidsTurn(partida);
        var enume = enumerable.Where(x => !x.Check).Where(x => CanMatch(partida, x));
        return (enume.IsEmpty())? enumerable.Where(x => x.Check) : enume;
    }

    private bool CanMatch(Partida<T> partida, Move<T> move)
    {
        if (move.Check) return true;
        foreach (var validturn in validsTurns!.Where(x => x == move.Turn)) {
            if (validturn == -1) return partida.Board[0].Head!.Equals(move.Head);
            return partida.Board[validturn].Tail!.Equals(move.Head);
        }
        return false;
    }

    private void ValidsTurn(Partida<T> partida)
    {
        validsTurns = new List<int>();

        foreach (var (i,move) in partida.Board.Enumerate().Where(x => !x.Item2.Check)){
            if (i == 0){
                validsTurns.Add(-1);
                validsTurns.Add(0);
            }
            else{
                validsTurns.Remove(move.Turn);
                validsTurns.Add(i);
            }
        }
    }
}

public class ClassicTurner<T> : ITurner<T>
{
    public ClassicTurner() { }

    public IEnumerable<Player<T>> Players(Partida<T> partida)
    {
        while (true)
        {
            foreach (var player in partida!.Players())
            {
                yield return player;
            }
        }
    }
}

public class ClassicDealer<T> : IDealer<T>
{
    int _pieceForPlayers;
    public ClassicDealer(int piecesForPlayers)
    {
        _pieceForPlayers = piecesForPlayers;
    }

    public Dictionary<Player<T>, Hand<T>> Deal(Partida<T> partida, IEnumerable<Ficha<T>> fichas)
    {
        Dictionary<Player<T>, Hand<T>> hands = new();
        Random r = new Random();
        var enumerator = fichas.OrderBy(x => r.NextDouble()-0.5).GetEnumerator();

        foreach (var player in partida.Players()){
            var hand = new Hand<T>();
            var count = 0;
            while (count++ < _pieceForPlayers && enumerator.MoveNext())
                hand.Add(enumerator.Current);
            hands.Add(player, hand);
        }

        return hands;
    }
}

public class ClassicGenerator : IGenerator<int>
{
    int _number;

    public ClassicGenerator(int number)
    {
        _number = number;
    }

    IEnumerable<Ficha<int>> IGenerator<int>.Generate()
    {
        List<Ficha<int>> fichas = new List<Ficha<int>>();

        for (int i = 0; i < _number; i++)
            for (int j = i; j < _number; j++)
                fichas.Add(new Ficha<int>(i,j));

        return fichas;
    }
}
