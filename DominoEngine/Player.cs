namespace DominoEngine;

public abstract class Player<T> {
	protected Random random = new Random();
	private Hand<T>? _hand;
	public readonly string name = "";

    protected Player(string Name) {
		name = Name;
	}

    public Player<T> SetHand(Hand<T> hand){
		_hand = hand;
		return this;
	}

	public override string ToString()
	{
		return name;
	}

	public abstract Move<T> Play(IEnumerable<Move<T>> possibleMoves, List<Move<T>> board, 
		Func<int, int> inHand, Func<Move<T>, double> scorer);

	public int PlayerId => this.GetHashCode();
}