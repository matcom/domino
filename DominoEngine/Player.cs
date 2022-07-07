namespace DominoEngine;

public abstract class Player<T> {
	protected Random random = new Random();
	protected Hand<T>? _hand;
	private readonly string name = "";
	protected int _iD;

    protected Player(string Name) {
		name = Name;
		_iD = this.GetHashCode();
	}

    protected Player(int playerId) {
		_iD = playerId;
    }

    public Player<T> SetHand(Hand<T> hand){
		_hand = hand;
		return this;
	}

	public override string ToString() => name;

	public Move<T> Play(IEnumerable<Move<T>> possibleMoves, Func<int, IEnumerable<int>> passesInfo, List<Move<T>> board, 
		Func<int, int> inHand, Func<Move<T>, double> scorer, Func<int, int, bool> partner) {
			if (possibleMoves.First().Check) return possibleMoves.First();
			var move = PreferenceCriterion(possibleMoves, passesInfo, board, inHand, scorer, partner).First();
			_hand!.Remove(move.Token!);
			return move;
		}

	public abstract IEnumerable<Move<T>> PreferenceCriterion(IEnumerable<Move<T>> moves, Func<int, IEnumerable<int>> passesInfo, 
		List<Move<T>> board, Func<int, int> inHand, Func<Move<T>, double> scorer, Func<int, int, bool> partner);

	public int PlayerId => _iD;
}