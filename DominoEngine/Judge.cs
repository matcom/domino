namespace DominoEngine;

public class Judge<T>
{

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
        Player<T> firstplayer = _rules.NextTurn();
        Ficha<T> salida = firstplayer.Play();
        _playInfo.FirstPlay(salida, firstplayer);

        while (NotFinished() && _playInfo.CountMoves() != 0)
        {
            Play();
        }

        CheckPlay();
    }

    public void CheckPlay() 
    {
        int count = 0;
        foreach (var item in _playInfo.GetTurns())
        {
            System.Console.WriteLine($"player {count++ % 4 + 1} : {item.move}");
        }
    }

    public void Play()
    {
        Player<T> actual_player = _rules.NextTurn();
        List<Move<T>> moves = _playInfo.PlayerMove(actual_player);

        if (moves.Count == 0)
        {
            _playInfo.AddTurn(actual_player, new Check<T>());
            return;
        }

        Move<T> move = actual_player.Play(moves);

        while (!_playInfo.ValidMove(move, actual_player)) move = actual_player.Play(moves);

        _playInfo.AddNode(move, actual_player);
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
    IMatcher<T>? _matcher;
    private List<(Player<T> player, IMove<T> move)> _turns;
    private int _turn = 0;
    private List<Ficha<T>>? _inGame;
    List<Move<T>>? _possibles;

    public PlayInfo(Dictionary<Player<T>, Hand<T>> hands, IMatcher<T> matcher)
    {
        _hands = hands;
        _matcher = matcher;
        _turns = new List<(Player<T> player, IMove<T> move)>();
        InGame();
    }

    public bool ValidMove(Move<T> move, Player<T> player) => _hands[player].Contains(move.Ficha);

    void InGame()
    {
        _inGame = new List<Ficha<T>>();

        foreach (var player in _hands.Keys)
        {
            foreach (var ficha in _hands[player])
            {
                _inGame.Add(ficha);
            }
        }
    }

    public List<(Player<T> player, IMove<T> move)> GetTurns() => _turns;

    public void AddTurn(Player<T> player, IMove<T> move)
    {
        _turns.Add((player, move));
        _turn++;
    }

    void Update()
    {
        _possibles = new List<Move<T>>();
        
        foreach (var ficha in _inGame!)
        {
            AskNode(ficha, true);
            AskNode(ficha, false);
        }
    }

    void AskNode(Ficha<T> ficha, bool rigth)
    {
        if (_matcher!.CanMatch(ficha.Head, rigth)) _possibles!.Add(new Move<T>(ficha.Head, ficha, rigth));
        if (_matcher.CanMatch(ficha.Tail, rigth)) _possibles!.Add(new Move<T>(ficha.Tail, ficha, rigth));
    }

    void Delete(Ficha<T> ficha) => _inGame!.Remove(ficha);

    public List<Move<T>> PlayerMove(Player<T> player)
    {
        List<Move<T>> moves = new List<Move<T>>();

        foreach (var move in _possibles!)
        {
            if (_hands[player].Contains(move.Ficha)) moves.Add(move);
        }

        return moves;
    }

    public int CountMoves() => _possibles!.Count;

    public void AddNode(Move<T> move, Player<T> player)
    {
        AddTurn(player, move);

        if (move.Rigth)
        {
            _board!.AddRight(move.Head, move.Ficha.Other(move.Head), _turn);
        }
        else _board!.AddLeft(move.Head, move.Ficha.Other(move.Head), _turn);

        DeleteMove(move, player);
        Delete(move.Ficha);
        Update();
    }

    public void DeleteMove(Move<T> move, Player<T> player)
    {
        _hands[player].Remove(move.Ficha);
    }

    public void FirstPlay(Ficha<T> ficha, Player<T> player)
    {
        _board = new Board<T>(ficha);
        DeleteMove(new Move<T>(ficha.Head, ficha, true), player);
        Delete(ficha);
        AddTurn(player, new Salida<T>(ficha.Head, ficha.Tail));
        _matcher!.SetBoard(_board);
        Update();
    }
}