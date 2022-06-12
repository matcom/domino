namespace DominoEngine;

public class RulesBook<T> : IRules<T>
{
    IGenerator<T> _generator;
    IDealer<T> _dealer;
    IMatcher<T> _matcher;
    ITurner<T> _turner;
    IFinisher<T> _finisher;
    IScorer<T> _scorer;

    public RulesBook(IGenerator<T> generator, IDealer<T> dealer, IMatcher<T> matcher, 
                    ITurner<T> turner, IFinisher<T> finisher, IScorer<T> scorer)
    {
        _generator = generator;
        _dealer = dealer;
        _matcher = matcher;
        _turner = turner;
        _finisher = finisher;
        _scorer = scorer;
    }

    public bool CanMatch(Move<T> move)
    {
        return _matcher.CanMatch(move);
    }

    public void Dealing(IList<Ficha<T>> fichas)
    {
        _dealer.Dealing(fichas);
    }

    public bool GameOver()
    {
        return _finisher.GameOver();
    }

    public List<Ficha<T>> Generate()
    {
        return _generator.Generate();
    }

    public IEnumerable<Player<T>> NextTurn()
    {
        throw new NotImplementedException();
    }

    public double Scorer(Move<T> move)
    {
        throw new NotImplementedException();
    }
}

public abstract class DominoScorer<T> : IScorer<T>
{
    protected Partida<T>? _partida;

    public abstract double Scorer(Move<T> move);

    public void SetPartida(Partida<T> partida)
    {
        _partida = partida;
    }
}

public class ClassicScorer : DominoScorer<int>
{
    public ClassicScorer() { }

    public override double Scorer(Move<int> move)
    {
        return move.Head + move.Tail;
    }
}

public abstract class DominoFinisher<T> : IFinisher<T>
{
    protected Partida<T>? _partida;

    public abstract bool GameOver();

    public void SetPartida(Partida<T> partida)
    {
        _partida = partida;
    }
}

public class ClassicFinisher : DominoFinisher<int>
{
    public override bool GameOver()
    {
        return AllCheck() || PlayerEnd();
    }

    public bool AllCheck()
    {
        bool all_checks = true;

        foreach (var player in _partida!.Players())
        {
            List<Move<int>> moves = _partida.PlayersMoves(player);
            if (!(moves[moves.Count-1].Check)) all_checks = false;
        }

        return all_checks;
    }

    public bool PlayerEnd()
    {
        foreach (var player in _partida!.Players())
        {
            if (_partida.Hands[player].Count == 0) return true;
        }

        return false;
    }
}

public abstract class DominoMatcher<T> : IMatcher<T>
{
    protected Partida<T>? _partida;
    protected List<int>? _validsTurns;

    public abstract bool CanMatch(Move<T> move);

    public void SetPartida(Partida<T> partida)
    {
        _partida = partida;
        _validsTurns = new List<int>();
    }

    public abstract void ValidsTurn();
}

public class ClassicMatcher : DominoMatcher<int>
{
    public override bool CanMatch(Move<int> move)
    {
        if (!move.Check && move.Turn >= -1)
        {
            if (!_validsTurns!.Contains(move.Turn)) return false;
            return (move.Turn == -1)? move.Head == _partida!.Board[0].Head : move.Head == _partida!.Board[move.Turn].Tail;
        }
        else return true;
    }

    public override void ValidsTurn()
    {
        _validsTurns = new List<int>();

        for (int i = 0; i < _partida!.Board.Count; i++)
        {
            Move<int> move = _partida!.Board[i];
            if (!move.Check)
            {
                if (move.Turn == -2)
                {
                    _validsTurns.Add(-1);
                    _validsTurns.Add(0);
                }
                else
                {
                    _validsTurns.Remove(move.Turn);
                    _validsTurns.Add(i);
                }
            }
        }
    }
}

public abstract class DominoTurner<T> : ITurner<T>
{
    protected Partida<T>? _partida;

    public void SetPartida(Partida<T> partida)
    {
        _partida = partida;
    }

    public abstract IEnumerable<Player<T>> NextTurn();
}

public class ClassicTurner : DominoTurner<int>
{
    public ClassicTurner() { }

    public override IEnumerable<Player<int>> NextTurn()
    {
        while (true)
        {
            foreach (var player in _partida!.Players())
            {
                yield return player;
            }
        }
    }
}

public abstract class Dealer<T> : IDealer<T>
{
    protected int _piecesForPlayers;
    protected IEnumerable<Hand<T>>? _hands;

    public abstract void Dealing(IList<Ficha<T>> fichas);
}

public class ClassicDealer : Dealer<int>
{
    public ClassicDealer(int piecesForPlayers, IEnumerable<Hand<int>> hands)
    {
        _piecesForPlayers = piecesForPlayers;
        _hands = hands;
    }

    public override void Dealing(IList<Ficha<int>> fichas)
    {
        bool[] mask = new bool[fichas.Count];
        Random r = new Random();

        foreach (var hand in _hands!)
        {
            for (int j = 0; j < _piecesForPlayers; j++)
            {
                int m = r.Next(fichas.Count);

                while (mask[m])
                {
                    m = r.Next(fichas.Count);
                }

                hand.Add(fichas[m]);
                mask[m] = true;
            }
        }
    }
}

public abstract class Generator<T> : IGenerator<T>
{
    protected int _number;
    public abstract List<Ficha<T>> Generate();
}

public class ClassicGenerator : Generator<int>
{
    public ClassicGenerator(int number)
    {
        _number = number;
    }

    public override List<Ficha<int>> Generate()
    {
        List<Ficha<int>> fichas = new List<Ficha<int>>();

        for (int i = 0; i < _number; i++)
        {
            for (int j = i; j < _number; j++)
            {
                fichas.Add(new Ficha<int>(i,j));
            }
        }

        return fichas;
    }
}
