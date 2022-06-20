namespace DominoEngine;

public abstract class Player<T> {
	public abstract Move<T> Play(IEnumerable<Move<T>> possibleMoves, Partida<T> partida,
		Func<Partida<T>, Move<T>, double> scorer);
}