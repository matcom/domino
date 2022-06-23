using DominoEngine;

namespace Players;
public class RandomPlayer<T> : Player<T>
{
    public RandomPlayer(string name) : base(name) {}

    public override Move<T> Play(IEnumerable<Move<T>> possibleMoves, List<Move<T>> board, 
        Func<int, int> inHand, Func<Move<T>, double> scorer)
            => possibleMoves.ElementAt(random.Next(possibleMoves.Count()));
}

public class Botagorda<T> : Player<T>
{
    public Botagorda(string name) : base(name) {}

    public override Move<T> Play(IEnumerable<Move<T>> possibleMoves, List<Move<T>> board,
        Func<int, int> inHand, Func<Move<T>, double> scorer)
            => possibleMoves.MaxBy(x => scorer(x))!;
}