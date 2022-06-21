namespace DominoEngine;

public abstract class Player<T> {
	public abstract Move<T> Play(IEnumerable<Move<T>> possibleMoves,
		Func<Move<T>, double> scorer);
}