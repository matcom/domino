using DominoEngine;

public class Partida<T> {
	private readonly Board<T> _board = new();
	private readonly Dictionary<Player<T>, Hand<T>> _hands = new();
	private readonly IEnumerable<Team<T>> _teams;
	private readonly Dictionary<int, IEnumerable<int>> _validsTurns = new();

	public Partida(IEnumerable<Team<T>> teams) {
		_teams = teams;
	}

	internal void AddMove(Move<T> move) => _board.Add(move);

	internal bool InHand(Player<T> player, Token<T> token) =>
		Hands.ContainsKey(player) && Hands[player].Contains(token);

	internal bool RemoveFromHand(Player<T> player, Token<T> token) =>
		Hands.ContainsKey(player) && Hands[player].Remove(token);

	internal void AddValidsTurns(IEnumerable<int> validsTurns) => _validsTurns.Add(_validsTurns.Count, validsTurns);

	internal IEnumerable<int> PassesInfo(int turn) => _validsTurns[turn];

	internal IEnumerable<Token<T>> Hand(Player<T> player) => Hands[player].Clone();

	internal int PlayerId(Player<T> player) => player.PlayerId;

	internal int InHand(int hash) {
		if (Players().Where(x => x.PlayerId == hash).IsEmpty()) return -1;
		return _hands[Players().Where(x => x.PlayerId == hash).FirstOrDefault()!].Count;
	} 

	internal bool Partnership(int pId1, int pId2) => TeamOf(pId1).Equals(TeamOf(pId2));

	internal Team<T> TeamOf(Player<T> player) => _teams.FirstOrDefault(x => x!.Contains(player), default)!;

	internal Team<T> TeamOf(int playerId) => TeamOf(Players().FirstOrDefault(x => x.PlayerId == playerId)!);

	internal List<Move<T>> Board => _board.ToList();

	internal void SetHand(Player<T> player, Hand<T> hand) => Hands.Add(player, hand.Clone());

	internal IEnumerable<Player<T>> Players() => _teams.SelectMany(x => x);

	internal IEnumerable<Team<T>> Teams() => _teams;

    internal Dictionary<Player<T>, Hand<T>> Hands => _hands;
}