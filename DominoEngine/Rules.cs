namespace DominoEngine;

public class Rules<T>
{
    ITurner<T> _turner;
    // IScorer<T> _scorer;
    IFinisher<T> _finisher;

    public Rules(ITurner<T> turner, IFinisher<T> finisher)
    {
        _turner = turner;
        // _scorer = scorer;
        _finisher = finisher;
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
            List<IMove<int>> moves = _partida.PlayersMoves(player);
            if (!(moves[moves.Count-1] is Check<int>)) all_checks = false;
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

    public abstract bool CanMatch(IMove<T> move);

    public void SetPartida(Partida<T> partida)
    {
        _partida = partida;
        _validsTurns = new List<int>();
    }

    public abstract void ValidsTurn();
}

public class ClassicMatcher : DominoMatcher<int>
{
    public override bool CanMatch(IMove<int> move)
    {
        if (_validsTurns!.Count == 0) ValidsTurn();
        
        if (move is BaseMove<int> mov)
        {
            if (!_validsTurns!.Contains(mov.Turn)) return false;
            return (mov.Turn == -1)? mov.Head == _partida!.Board[0].Head : mov.Head == _partida!.Board[mov.Turn].Tail;
        }
        else return true;
    }

    public override void ValidsTurn()
    {
        _validsTurns = new List<int>();
        _validsTurns.Add(-1);
        _validsTurns.Add(0);

        for (int i = 0; i < _partida!.Board.Count; i++)
        {
            if (_partida!.Board[i] is BaseMove<int> move)
            {
                _validsTurns.Remove(move.Turn);
                _validsTurns.Add(i);
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
    protected Dealer(int piecesForPlayers, IEnumerable<Hand<T>> hands, IList<Ficha<T>> fichas)
    {
        Dealing(piecesForPlayers, hands, fichas);
    }

    public abstract void Dealing(int piecesForPlayers, IEnumerable<Hand<T>> hands, IList<Ficha<T>> fichas);
}

public class ClassicDealer : Dealer<int>
{
    public ClassicDealer(int piecesForPlayers, IEnumerable<Hand<int>> hands, IList<Ficha<int>> fichas) 
                        : base(piecesForPlayers, hands, fichas) {}

    public override void Dealing(int piecesForPlayers, IEnumerable<Hand<int>> hands, IList<Ficha<int>> fichas)
    {
        bool[] mask = new bool[fichas.Count];
        Random r = new Random();

        foreach (var hand in hands)
        {
            for (int j = 0; j < piecesForPlayers; j++)
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
    protected Generator(int number)
    {
        _number = number;
    }

    public abstract List<Ficha<T>> Generate();
}

public class ClassicGenerator : Generator<int>
{
    public ClassicGenerator(int number) : base(number)
    {
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
