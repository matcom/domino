namespace DominoEngine; 

public abstract class Player<T> {
	private Hand<T>? _hand;
	public int playerId;

	public void SetHand(Hand<T> hand) => _hand = hand;

	public abstract Move<T> Play(IEnumerable<Move<T>> possibleMoves, Func<Move<T>, double> scorer);
}