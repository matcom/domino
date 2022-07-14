using DominoEngine;

namespace Rules;

public class ClassicFinisher<T> : IFinisher<T>
{
    public bool GameOver(Partida<T> partida)
        => AllCheck(partida) || PlayerEnd(partida);

    private bool AllCheck(Partida<T> partida)
        => partida.Players().All(player => !partida.Board.Where(move => move.PlayerId == partida.PlayerId(player)).IsEmpty() 
            && partida.Board.Last(x => x.PlayerId == partida.PlayerId(player)).Check);

        private static bool PlayerEnd(Partida<T> partida) 
        => partida.Players().Any(player => partida.Hand(player).IsEmpty());
}

public class TurnCountFinisher<T> : IFinisher<T>
{
    private readonly int _number;
    public TurnCountFinisher(int number) {
        _number = number;
    }

    public bool GameOver(Partida<T> partida)
        => partida.Board.Count(move => !move.Check) > _number;
}

public class PassesCountFinisher<T> : IFinisher<T>
{
    private readonly int _number;
    public PassesCountFinisher(int number)
    {
        _number = number;
    }

    public bool GameOver(Partida<T> partida)
        => partida.Board.Count(move => move.Check) > _number;
}

public static class FinisherExtensors
{
    public static IFinisher<TSource> Join<TSource>(this IFinisher<TSource> finisher1, IFinisher<TSource> finisher2)
        => new JoinFinisher<TSource>(finisher1, finisher2);

    public static IFinisher<TSource> Intersect<TSource>(this IFinisher<TSource> finisher1, IFinisher<TSource> finisher2)
        => new JoinFinisher<TSource>(finisher1, finisher2);
}

internal class IntersectFinisher<T> : IFinisher<T>
{
    private readonly IFinisher<T> _finisher1;
    private readonly IFinisher<T> _finisher2;
    public IntersectFinisher(IFinisher<T> finisher1, IFinisher<T> finisher2)
    {
        _finisher1 = finisher1;
        _finisher2 = finisher2;
    }

    public bool GameOver(Partida<T> partida)
        => _finisher1.GameOver(partida) && _finisher2.GameOver(partida);
}

internal class JoinFinisher<T> : IFinisher<T>
{
    private readonly IFinisher<T> _finisher1;
    private readonly IFinisher<T> _finisher2;
    public JoinFinisher(IFinisher<T> finisher1, IFinisher<T> finisher2)
    {
        _finisher1 = finisher1;
        _finisher2 = finisher2;
    }

    public bool GameOver(Partida<T> partida)
        => _finisher1.GameOver(partida) || _finisher2.GameOver(partida);
}
