using DominoEngine;

namespace Players;
public class RandomPlayer<T> : Player<T>
{
    public RandomPlayer(string name) : base(name) {}

    public override IEnumerable<Move<T>> PreferenceCriterion(IEnumerable<Move<T>> moves, 
        Func<int, IEnumerable<int>> passesInfo, List<Move<T>> board, Func<int, int> inHand, 
            Func<Move<T>, double> scorer, Func<int, int, bool> partner)
                => moves.OrderBy(move => random.Next());
}

public class Botagorda<T> : Player<T>
{
    public Botagorda(string name) : base(name) {}

    public Botagorda(int playerId) : base(playerId) {}

    public override IEnumerable<Move<T>> PreferenceCriterion(IEnumerable<Move<T>> moves, 
        Func<int, IEnumerable<int>> passesInfo, List<Move<T>> board, Func<int, int> inHand, 
            Func<Move<T>, double> scorer, Func<int, int, bool> partner)
                => moves.OrderByDescending(move => scorer(move));
}

public class DestroyerPlayer<T> : Player<T>
{
    Botagorda<T> _aux;
    public DestroyerPlayer(string Name) : base(Name) {
        _aux = new(PlayerId);
    }

    public DestroyerPlayer(int playerId) : base(playerId) {
        _aux = new(playerId);
    }

    public override IEnumerable<Move<T>> PreferenceCriterion(IEnumerable<Move<T>> moves, 
        Func<int, IEnumerable<int>> passesInfo, List<Move<T>> board, Func<int, int> inHand, 
            Func<Move<T>, double> scorer, Func<int, int, bool> partner) 
                => moves.OrderBy(move => DestroyRival(move.Head!, board, MakeRivalsTeams(board, partner), passesInfo)).
                Average(moves.OrderByDescending(move => RivalPasses(move.Tail!, board, MakeRivalsTeams(board, partner), passesInfo)),1).
                Average(moves.OrderBy(move => PreferRivalsTokens(move, board, partner)),1).
                Average(_aux.PreferenceCriterion(moves, passesInfo, board, inHand, scorer ,partner),0.5);


    private IEnumerable<int> MakeRivalsTeams(List<Move<T>> board, Func<int, int, bool> partner)
        => board.Select(move => move.PlayerId).Where(pId => !partner(pId, PlayerId)).ToHashSet();

    private int PreferRivalsTokens(Move<T> move, List<Move<T>> board, Func<int, int, bool> partner) {
        if (board.IsEmpty()) return 0;
        else return (move.Check)? 1 : (partner(PlayerId, board[(move.Turn is -1)? 0 : move.Turn].PlayerId))? 0 : 1;
    }

    private int DestroyRival(T head, List<Move<T>> board, IEnumerable<int> rivalId, Func<int, IEnumerable<int>> passesInfo) 
        => rivalId.Where(player => board.Where(move => move.Check && move.PlayerId == player).Enumerate().
            Select(pair => passesInfo(pair.Item1)).Any(turns => turns.Any(turn => board[turn].Tail!.Equals(head)))).Count();

    private int RivalPasses(T tail, List<Move<T>> board, IEnumerable<int> rivalId, Func<int, IEnumerable<int>> passesInfo) 
        => rivalId.Where(player => board.Where(move => move.Check && move.PlayerId == player).Enumerate().
            Select(pair => passesInfo(pair.Item1)).Any(turns => turns.Any(turn => board[turn].Tail!.Equals(tail)))).Count();
}

public class SupportPlayer<T> : Player<T>
{
    Botagorda<T> _aux;
    public SupportPlayer(string Name) : base(Name) {
        _aux = new(PlayerId);
    }

    public SupportPlayer(int playerId) : base(playerId) {
        _aux = new(playerId);
    }

    public override IEnumerable<Move<T>> PreferenceCriterion(IEnumerable<Move<T>> moves, 
        Func<int, IEnumerable<int>> passesInfo, List<Move<T>> board, Func<int, int> inHand, 
            Func<Move<T>, double> scorer, Func<int, int, bool> partner)
                => moves.OrderBy(move => HelpTeam(move.Head!, board, MakeTeam(board, partner), passesInfo)).
                Average(moves.OrderBy(move => AvoidFriendPasses(move.Tail!, board, MakeTeam(board, partner), passesInfo)),1).
                Average(moves.OrderBy(move => AvoidFriendsTokens(move, board, partner)),1).
                Average(_aux.PreferenceCriterion(moves, passesInfo, board, inHand, scorer ,partner), 0.5);

    // Encontrar Id de compañeros de equipo
    private IEnumerable<int> MakeTeam(List<Move<T>> board, Func<int, int, bool> partner)
        => board.Select(move => move.PlayerId).Where(pId => pId != PlayerId
            && partner(pId, PlayerId)).ToHashSet();

    // Calcula cantidad de compañeros pasados por un valor especifico
    private int HelpTeam(T head, List<Move<T>> board, IEnumerable<int> teamId, Func<int, IEnumerable<int>> passesInfo) 
        => teamId.Where(player => board.Where(move => move.Check && move.PlayerId == player).Enumerate().
            Select(pair => passesInfo(pair.Item1)).Any(turns => turns.Any(turn => board[turn].Tail!.Equals(head)))).Count();

    // Evitar jugar por fichas puestas por compañeros de equipo
    private int AvoidFriendsTokens(Move<T> move, List<Move<T>> board, Func<int, int, bool> partner) {
        if (board.IsEmpty()) return 0;
        else return (move.Check)? 1 : (partner(PlayerId, board[(move.Turn is -1)? 0 : move.Turn].PlayerId))? 1 : 0;
    }  

    // Contar la cantidad de compañeros que podrian pasarse con un valor especifico
    private int AvoidFriendPasses(T tail, List<Move<T>> board, IEnumerable<int> teamId, Func<int, IEnumerable<int>> passesInfo) 
        => teamId.Where(player => board.Where(move => move.Check && move.PlayerId == player).Enumerate().
            Select(pair => passesInfo(pair.Item1)).Any(turns => turns.Any(turn => board[turn].Tail!.Equals(tail)))).Count();
}

public class SelfishPlayer<T> : Player<T>
{
    Botagorda<T> _aux;
    public SelfishPlayer(string Name) : base(Name) {
        _aux = new(PlayerId);
    }

    public SelfishPlayer(int playerId) : base(playerId) {
        _aux = new(playerId);
    }

    public override IEnumerable<Move<T>> PreferenceCriterion(IEnumerable<Move<T>> moves, 
        Func<int, IEnumerable<int>> passesInfo, List<Move<T>> board, Func<int, int> inHand, 
            Func<Move<T>, double> scorer, Func<int, int, bool> partner) {
                var data = Data();
                var new_moves = moves.OrderByDescending(move => Math.Min(data[move.Tail!], data[move.Head!]));
                return new_moves.Average(_aux.PreferenceCriterion(moves, passesInfo, board, inHand, scorer, partner), 0.5);
            }

    private Dictionary<T, int> Data() {
        Dictionary<T, int> data = new();
        foreach (var token in _hand!) {
            if (!data.ContainsKey(token.Head)) data.Add(token.Head, 1);
            else data[token.Head]++;
            if (!data.ContainsKey(token.Tail)) data.Add(token.Tail, 1);
            else data[token.Head]++;
        }
        return data;
    }
}

public class SmartPlayer<T> : Player<T>
{
    SelfishPlayer<T> selfish;
    SupportPlayer<T> helper;
    DestroyerPlayer<T> destroyer;
    public SmartPlayer(string Name) : base(Name) { 
        selfish = new(PlayerId);
        helper = new(PlayerId);
        destroyer = new(PlayerId);
    }

    public override IEnumerable<Move<T>> PreferenceCriterion(IEnumerable<Move<T>> moves, 
        Func<int, IEnumerable<int>> passesInfo, List<Move<T>> board, Func<int, int> inHand, 
            Func<Move<T>, double> scorer, Func<int, int, bool> partner) {
                var first = selfish.SetHand(_hand!).PreferenceCriterion(moves, passesInfo, board, inHand, scorer, partner);
                var second = helper.SetHand(_hand!).PreferenceCriterion(moves, passesInfo, board, inHand, scorer, partner);
                var third = destroyer.SetHand(_hand!).PreferenceCriterion(moves, passesInfo, board, inHand, scorer, partner);

                if (MakeRivalsTeams(board, partner).IsEmpty() || MakeTeam(board, partner).IsEmpty())
                    return first;
                
                if (MakeRivalsTeams(board, partner).Select(pId => inHand(pId)).Min() > 4) 
                    if (MakeTeam(board, partner).Select(pId => inHand(pId)).Min() < inHand(PlayerId))
                        (first, second) = (second, first);
                else (first, third) = (third, first);

                return first.Average(second,0.5).Average(third,0.5);
            }

    private IEnumerable<int> MakeTeam(List<Move<T>> board, Func<int, int, bool> partner)
        => board.Select(move => move.PlayerId).Where(pId => pId != PlayerId
            && partner(pId, PlayerId)).ToHashSet();

    private IEnumerable<int> MakeRivalsTeams(List<Move<T>> board, Func<int, int, bool> partner)
        => board.Select(move => move.PlayerId).Where(pId => !partner(pId, PlayerId)).ToHashSet();
}
