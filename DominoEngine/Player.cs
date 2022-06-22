namespace DominoEngine;

public abstract class Player<T> {
	private Hand<T>? _hand;
	public int id;

    protected Player(int ID)
    {
		id = ID;
    }

    public Player<T> SetHand(Hand<T> hand){
		_hand = hand;
		return this;
	}

	public override string ToString()
	{
		return id.ToString();
	}

	public abstract Move<T> Play(IEnumerable<Move<T>> possibleMoves, Partida<T> partida,
		Func<Move<T>, double> scorer);
}