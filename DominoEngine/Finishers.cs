using DominoEngine;

namespace Rules;

public class PlayerEndFinisher<T> : IFinisher<T>
{
    public bool GameOver(Partida<T> partida)
        => PlayerEnd(partida);

    private static bool PlayerEnd(Partida<T> partida) 
        => partida.Players().Any(player => partida.Hand(player).IsEmpty());

    public override string ToString()
        => "El juego termina cuando un jugador se pega";
}

public class AllCheckFinisher<T> : IFinisher<T>
{
    public bool GameOver(Partida<T> partida)
        => AllCheck(partida);

    private bool AllCheck(Partida<T> partida)
        => partida.Players().All(player => !partida.Board.Where(move => move.PlayerId == partida.PlayerId(player)).IsEmpty() 
            && partida.Board.Last(x => x.PlayerId == partida.PlayerId(player)).Check);

    public override string ToString()
        => "El juego termina cuando la ultima jugada de cada player fue un pase";
}

public class TurnCountFinisher<T> : IFinisher<T>
{
    private readonly int _number;
    public TurnCountFinisher(int number) {
        _number = number;
    }

    public bool GameOver(Partida<T> partida)
        => partida.Board.Count(move => !move.Check) > _number;

    public override string ToString()
        => "El juego termina cuando se ha jugado n cantidad de fichas";
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

    public override string ToString()
        => "El juego termina cuando los players se han pasado n veces en total";
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

    public override string ToString()
        => $@"Intersect:
    {_finisher1.ToString()!.Replace("\n","\n\t")}
	{_finisher2.ToString()!.Replace("\n","\n\t")}";
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

    public override string ToString()
        => $@"Union:
    {_finisher1.ToString()!.Replace("\n","\n\t")}
	{_finisher2.ToString()!.Replace("\n","\n\t")}";
}
