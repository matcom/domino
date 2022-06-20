using DominoEngine;

public class Partida<T> {
	private readonly Board<T> _board = new();
	private readonly Dictionary<Player<T>, Hand<T>> _hands = new();
	private readonly List<Player<T>> _players;

	public Partida(List<Player<T>> players) {
		_players = players;
	}

	internal void AddMove(Move<T> move) => _board.Add(move);

	internal bool InHand(Player<T> player, Ficha<T> ficha) =>
		_hands.ContainsKey(player) && _hands[player].Contains(ficha);

	internal bool RemoveFromHand(Player<T> player, Ficha<T> ficha) =>
		_hands.ContainsKey(player) && _hands[player].Remove(ficha);

	internal IEnumerable<Ficha<T>> Hand(Player<T> player) => _hands[player].Clone();

	internal Dictionary<Player<T>, Hand<T>> Hands() => _hands.ToDictionary(t => t.Key, t => t.Value);

	internal int PlayerId(Player<T> player) => _players.IndexOf(player);

	internal List<Move<T>> Board => _board.ToList();

	internal void SetHand(Player<T> player, Hand<T> hand) => _hands[player] = hand.Clone();
}