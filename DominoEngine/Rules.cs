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
public abstract class Finisher<T> : IFinisher<T>
{
    private Partida<T>? _partida;

    public Finisher() {}

    public abstract bool GameOver();

    public void SetPartida(Partida<T> partida)
    {
        _partida = partida;
    }
}

public class ClassicFinisher : Finisher<int>
{
    public override bool GameOver()
    {
        throw new NotImplementedException();
    }
}

public abstract class DominoMatcher<T> : IMatcher<T>
{
    protected Partida<T>? _partida;
    protected List<int>? _validsTurns;

    public DominoMatcher() {}

    public abstract bool CanMatch(Move<T> move);

    public void SetPartida(Partida<T> partida)
    {
        _partida = partida;
    }

    public abstract void ValidsTurn();
}

public class ClassicMatcher : DominoMatcher<int>
{
    public override bool CanMatch(Move<int> move)
    {
        ValidsTurn();

        if (move is BaseMove<int> mov && _validsTurns!.Contains(mov.Turn))
        {
            return mov.Head == _partida!._board[mov.Turn].
        }
    }

    public override void ValidsTurn()
    {
        _validsTurns = new List<int>();
        _validsTurns.Add(-1);
        _validsTurns.Add(0);

        for (int i = 0; i < _partida!._board.Count; i++)
        {
            if (_partida!._board[i] is BaseMove<int> move)
            {
                _validsTurns.Remove(move.Turn);
                _validsTurns.Add(i);
            }
        }
    }
}

public abstract class Turner<T> : ITurner<T>
{
    protected Player<T>[] _players;
    protected int _currplayer = 0;

    public Turner(List<Player<T>> players)
    {
        _players = players.ToArray();
    }
    public abstract Player<T> NextTurn();
}

public class ClassicTurner : Turner<int>
{
    public ClassicTurner(List<Player<int>> players) : base(players) {}

    public override Player<int> NextTurn()
    {
        _currplayer = (_currplayer) % _players.Length;
        return _players[_currplayer++];
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
