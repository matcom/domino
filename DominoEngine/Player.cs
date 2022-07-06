namespace DominoEngine;

public abstract class Player<T> {
	protected Random random = new Random();
	protected Hand<T>? _hand;
	public readonly string name = "";

    protected Player(string Name) {
		name = Name;
	}

    public Player<T> SetHand(Hand<T> hand){
		_hand = hand;
		return this;
	}

	public override string ToString() => name;

	public Move<T> Play(IEnumerable<Move<T>> possibleMoves, Func<int, IEnumerable<int>> passesInfo, List<Move<T>> board, 
		Func<int, int> inHand, Func<Move<T>, double> scorer, Func<int, int, bool> partner) {
			var move = PreferenceCriterion(possibleMoves, passesInfo, board, inHand, scorer, partner).First();
			_hand!.Remove(move.Token!);
			return move;
		}

	public abstract IEnumerable<Move<T>> PreferenceCriterion(IEnumerable<Move<T>> moves, Func<int, IEnumerable<int>> passesInfo, 
		List<Move<T>> board, Func<int, int> inHand, Func<Move<T>, double> scorer, Func<int, int, bool> partner);

	public int PlayerId => this.GetHashCode();
}