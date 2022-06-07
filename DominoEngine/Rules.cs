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
    
    public Player<T> NexTurn() => _turner.NextTurn();
    public bool IsEnd() => _finisher.IsEnd();
}

public abstract class Finisher<T> : IFinisher<T>
{
    protected Dictionary<Player<T>, Hand<T>> _hands;
    public Finisher(Dictionary<Player<T>, Hand<T>> hands)
    { 
        _hands = hands;
    }
    public abstract bool IsEnd();
}

public class ClassicFinisher : Finisher<int>
{
    public ClassicFinisher(Dictionary<Player<int>, Hand<int>> hands) : base(hands)
    {
    }

    public override bool IsEnd()
    {
        return PlayerWin();
    }

    private bool PlayerWin()
    {
        foreach (var item in _hands.Values)
        {
            if (item.Count == 0) return true;
        }

        return false;
    }
}

public class Matcher<T> : IMatcher<T>
{
    protected Board<T> _board;
    protected Node<T> _left;
    protected Node<T> _rigth;

    public Matcher(Board<T> board)
    {
        _board = board;
        _left = board.Left;
        _rigth = board.Right;
    }

    public virtual bool CanMatch(T toParent, bool right) => toParent!.Equals(right? _rigth.ToChild : _left.ToChild);
}

public class ClassicMatcher : Matcher<int>
{
    public ClassicMatcher(Board<int> board) : base(board) {}

    public override bool CanMatch(int toParent, bool right) => toParent == (right? _rigth.ToChild : _left.ToChild);
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
