using DominoEngine;

public class Partida<T> {
	private readonly Board<T> _board = new();
	private readonly Dictionary<Player<T>, Hand<T>> _hands = new();
	private readonly List<Team<T>> _teams;

	public Partida(List<Team<T>> teams) {
		_teams = teams;
	}

	internal void AddMove(Move<T> move) => _board.Add(move);

	internal bool InHand(Player<T> player, Ficha<T> ficha) =>
		Hands.ContainsKey(player) && Hands[player].Contains(ficha);

	internal bool RemoveFromHand(Player<T> player, Ficha<T> ficha) =>
		Hands.ContainsKey(player) && Hands[player].Remove(ficha);

	internal IEnumerable<Ficha<T>> Hand(Player<T> player) => Hands[player].Clone();

	internal int PlayerId(Player<T> player) => player.PlayerId;

	internal int InHand(int hash) {
		if (Players().Where(x => x.PlayerId == hash).IsEmpty()) return -1;
		return _hands[Players().Where(x => x.PlayerId == hash).FirstOrDefault()!].Count;
	} 

	internal Team<T> TeamOf(Player<T> player) => _teams.FirstOrDefault(x => x!.Contains(player), default)!;

	public List<Move<T>> Board => _board.ToList();

	internal void SetHand(Player<T> player, Hand<T> hand) => Hands.Add(player, hand.Clone());

	internal IEnumerable<Player<T>> Players() {
		foreach (var team in _teams.SelectMany(x => x))
			yield return team;
	}

    internal Dictionary<Player<T>, Hand<T>> Hands => _hands;
}