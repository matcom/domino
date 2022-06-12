using DominoEngine;

public class Partida<T> {
	private readonly Board<T> _board = new Board<T>();
	private readonly Dictionary<Player<T>, Hand<T>> _hands;

	public Partida(Dictionary<Player<T>, Hand<T>> hands) {
		_hands = hands;
	}

	internal void AddMove(Move<T> move) => _board.Add(move);

	internal bool InHand(Player<T> player, Ficha<T> ficha) => _hands.ContainsKey(player) && _hands[player].Contains(ficha);

	internal bool RemoveFromHand(Player<T> player, Ficha<T> ficha) =>
		_hands.ContainsKey(player) && _hands[player].Remove(ficha);
}