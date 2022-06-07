namespace DominoEngine;

public class Rules<T>
{
    ITurner<T> _turner;
    IScorer<T> _scorer;
    IFinisher<T> _finisher;
    IMatcher<T> _matcher;

    public Rules(ITurner<T> turner, IScorer<T> scorer, IFinisher<T> finisher, IMatcher<T> matcher)
    {
        _turner = turner;
        _scorer = scorer;
        _finisher = finisher;
        _matcher = matcher;
    }

    public bool CanMatch(T toParent, bool rigth) => _matcher.CanMatch(toParent, rigth);
    public Player<T> NexTurn() => _turner.NextTurn();
    public bool IsEnd() => _finisher.IsEnd();
}

public abstract class Finisher<T> : IFinisher<T>
{
    protected List<bool> _turns;
    protected Dictionary<Player<T>, List<Ficha<T>>> _hands;
    public Finisher(List<bool> turns, Dictionary<Player<T>, List<Ficha<T>>> hands)
    {
        _turns = turns;   
        _hands = hands;
    }
    public abstract bool IsEnd();
}

public class ClassicFinisher : Finisher<int>
{
    public ClassicFinisher(List<bool> turns, Dictionary<Player<int>, List<Ficha<int>>> hands) : base(turns, hands)
    {
    }

    public override bool IsEnd()
    {
        return PlayerWin() || AllPasses();
    }

    private bool PlayerWin()
    {
        foreach (var item in _hands.Values)
        {
            if (item.Count == 0) return true;
        }

        return false;
    }

    private bool AllPasses()
    {
        for (int i = 0; i < _hands.Count; i++)
        {
            if (_turns[_turns.Count-1-i]) return false;
        }

        return true;
    }
}

public abstract class Matcher<T> : IMatcher<T>
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

    public abstract bool CanMatch(T toParent, bool right);
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
            }
        }
    }
}

public abstract class Generator<T> : IGenerator<T>
{
    public abstract List<Ficha<T>> Generate(int n);
}

public class ClassicGenerator : Generator<int>
{
    public override List<Ficha<int>> Generate(int n)
    {
        List<Ficha<int>> fichas = new List<Ficha<int>>();

        for (int i = 0; i < n; i++)
        {
            for (int j = i; j < n; j++)
            {
                fichas.Add(new Ficha<int>(i,j));
            }
        }

        return fichas;
    }
}
