using DominoEngine;

public class Partida<T> {
	private readonly Board<T> _board = new Board<T>();
	private readonly Dictionary<Player<T>, Hand<T>> _hands;
	private readonly List<Player<T>> _players;

	public Partida(Dictionary<Player<T>, Hand<T>> hands) {
		_hands = hands;
		_players = hands.Keys.ToList();
	}

	internal void AddMove(Move<T> move) => _board.Add(move);

	internal bool InHand(Player<T> player, Ficha<T> ficha) => _hands.ContainsKey(player) && _hands[player].Contains(ficha);

	internal bool RemoveFromHand(Player<T> player, Ficha<T> ficha) =>
		_hands.ContainsKey(player) && _hands[player].Remove(ficha);

	internal IEnumerable<Ficha<T>> Hand(Player<T> player) => _hands[player].Clone();

	internal int PlayerId(Player<T> player) => _players.IndexOf(player);
	
	internal IEnumerable<Move<T>> Board => _board;
}