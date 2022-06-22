using DominoEngine;

namespace Players;
public class RandomPlayer<T> : Player<T>
{
    public RandomPlayer(int ID) : base(ID) {}

    public override Move<T> Play(IEnumerable<Move<T>> possibleMoves, Partida<T> partida, Func<Move<T>, double> scorer)
        => possibleMoves.ElementAt(new Random().Next(possibleMoves.Count()));
}

public class Botagorda<T> : Player<T>
{
    public Botagorda(int ID) : base(ID) {}

    public override Move<T> Play(IEnumerable<Move<T>> possibleMoves, Partida<T> partida, Func<Move<T>, double> scorer)
        => possibleMoves.MaxBy(x => scorer(x))!;
}