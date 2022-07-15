namespace DominoEngine;

public abstract class Player<T> {
	protected readonly Random Random = new Random();
	protected Hand<T>? Hand;
	private readonly string _name = "";

	protected Player(string name) {
		_name = name;
		PlayerId = this.GetHashCode();
	}

    protected Player(int playerId) => PlayerId = playerId;

    public Player<T> SetHand(Hand<T> hand){
		Hand = hand;
		return this;
	}

	public override string ToString() => _name;

	public abstract Move<T> Play(IEnumerable<Move<T>> possibleMoves, Func<int, IEnumerable<int>> passesInfo, List<Move<T>> board, 
		Func<int, int> inHand, Func<Move<T>, double> scorer, Func<int, int, bool> partner);

	public int PlayerId { get; }
}

public abstract class CriterionPlayer<T> : Player<T>
{
    protected CriterionPlayer(string name) : base(name) {}

    protected CriterionPlayer(int playerId) : base(playerId) {}

	public override Move<T> Play(IEnumerable<Move<T>> possibleMoves, Func<int, IEnumerable<int>> passesInfo, List<Move<T>> board, 
		Func<int, int> inHand, Func<Move<T>, double> scorer, Func<int, int, bool> partner) {
			if (possibleMoves.First().Check) return possibleMoves.First(); 
			var move = PreferenceCriterion(possibleMoves, passesInfo, board, inHand, scorer, partner).First();
			Hand!.Remove(move.Token!);
			return move; 
		}

	public abstract IEnumerable<Move<T>> PreferenceCriterion(IEnumerable<Move<T>> moves, Func<int, IEnumerable<int>> passesInfo, 
		List<Move<T>> board, Func<int, int> inHand, Func<Move<T>, double> scorer, Func<int, int, bool> partner);
}
