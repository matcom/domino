namespace DominoEngine;

public class Judge<T> {
	
    private PlayInfo<T> _playInfo;
    private Rules<T> _rules;

    public Judge(PlayInfo<T> playInfo, Rules<T> rules)
    {
        _playInfo = playInfo;
        _rules = rules;
        Init();
    }

    void Init()
    {
        Player<T> firstplayer = _rules.NexTurn();
        Ficha<T> salida = firstplayer.Play();
        _playInfo.FirstPlay(salida, firstplayer);

        while (NotFinished() && _playInfo.CountMoves() != 0)
        {
            Play();
        }

        CheckPlay();
    }

    public void CheckPlay(){}

    public void Play()
    {
        Player<T> actual_player = _rules.NexTurn();
        List<Move<T>> moves = _playInfo.PlayerNextMove(actual_player);

        if (moves.Count == 0)
        {
            _playInfo.AddTurn(actual_player);
            return;
        }

        Move<T> move = actual_player.Play(moves);
        
        while (!_playInfo.ValidMove(move, actual_player)) move = actual_player.Play(moves);

        _playInfo.AddNode(move, actual_player);
        _playInfo.AddTurn(actual_player);
    }

    public bool NotFinished()
    {
        return !_rules.IsEnd();
    }
}

public class PlayInfo<T>
{
    public Board<T>? _board;
	private Dictionary<Player<T>, Hand<T>> _hands = new Dictionary<Player<T>, Hand<T>>();
    private Dictionary<Node<T>, Dictionary<Player<T>, List<Move<T>>>> _playersNextMove;
    IMatcher<T>? _matcher;
	private List<Player<T>> _turns;
    private int _turn = 0;

    public PlayInfo(Dictionary<Player<T>, Hand<T>> hands, List<Player<T>> turns)
    {
        _hands = hands;
        _turns = turns;
        _playersNextMove = new Dictionary<Node<T>, Dictionary<Player<T>, List<Move<T>>>>();
    }

    public bool ValidMove(Move<T> move, Player<T> player) => _hands[player].Contains(move.Ficha);

    public void SetMatcher()
    {
        _matcher = new Matcher<T>(_board!);
    }

    public int CountMoves()
    {
        int count = 0;

        foreach (var moves in _playersNextMove[_board!.Right].Values) count += moves.Count;
        foreach (var moves in _playersNextMove[_board!.Left].Values) count += moves.Count;

        return count;
    }

    public void AddTurn(Player<T> player)
    {
        _turns.Add(player);
        _turn++;
    }

    public void Update(bool rigth)
    {
        Node<T> node = (rigth)? _board!.Right : _board!.Left;
        if (node.Turn != 0)
        {
            _playersNextMove.Remove(node.Parent!);
            _playersNextMove.Add(node, new Dictionary<Player<T>, List<Move<T>>>());
        }

        foreach (var player in _hands.Keys)
        {
            List<Move<T>> moves = new List<Move<T>>();

            foreach (var ficha in _hands[player])
            {
                if (_matcher!.CanMatch(ficha.Head, rigth)) moves.Add(new Move<T>(ficha.Head, ficha, rigth));
                if (_matcher.CanMatch(ficha.Tail, rigth)) moves.Add(new Move<T>(ficha.Tail, ficha, rigth));
            }
            
            _playersNextMove[node].Add(player, moves);

        }
    }

    public List<Move<T>> PlayerNextMove(Player<T> player)
    {
        List<Move<T>> moves = new List<Move<T>>();

        foreach (var move in _playersNextMove[_board!.Right][player])
        {
            moves.Add(move);
        }
        foreach (var move in _playersNextMove[_board!.Left][player])
        {
            moves.Add(move);
        }

        return moves;
    }

    public void AddNode(Move<T> move, Player<T> player)
    {
        AddTurn(player);

        if (move.Rigth)
        {
            _board!.AddRight(move.Head, move.Ficha.Other(move.Head), _turn);
        }
        else _board!.AddLeft(move.Head, move.Ficha.Other(move.Head), _turn);

        DeleteMove(move, player);
        Update(move.Rigth);
    }

    public void DeleteMove(Move<T> move, Player<T> player)
    {
        _hands[player].Remove(move.Ficha);
    }

    public void FirstPlay(Ficha<T> ficha, Player<T> player)
    {
        _board = new Board<T>(ficha);
        DeleteMove(new Move<T>(ficha.Head, ficha, true), player);
        _playersNextMove.Add(_board!.Right, new Dictionary<Player<T>, List<Move<T>>>());
        _playersNextMove.Add(_board!.Left, new Dictionary<Player<T>, List<Move<T>>>());
        SetMatcher();
        Update(true);
        Update(false);
    }
}