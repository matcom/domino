using DominoEngine;

public class Partida<T> {
	private readonly Board<T> _board = new();
	private readonly Dictionary<Player<T>, Hand<T>> _hands = new();
	private readonly IEnumerable<Team<T>> _teams;
	private readonly Dictionary<int, IEnumerable<int>> _validsTurns = new();

	public Partida(IEnumerable<Team<T>> teams) {
		_teams = teams;
	}

	// Añade movimientos al tablero
	internal void AddMove(Move<T> move) => _board.Add(move);

	// Remueve fichas de las manos de los jugadores
	internal bool RemoveFromHand(Player<T> player, Token<T> token) =>
		Hands.ContainsKey(player) && Hands[player].Remove(token);

	// Actualiza los turnos validos de la partida, guarda el registro de jugadas validas por turno
	internal void AddValidsTurns(IEnumerable<int> validsTurns) => _validsTurns.Add(_validsTurns.Count, validsTurns);

	// Para un turno devuelve las jugadas validas en ese momento
	internal IEnumerable<int> PassesInfo(int turn) => _validsTurns[turn];

	// Devuelve una copia de la mano del player
	internal IEnumerable<Token<T>> Hand(Player<T> player) => Hands[player].Clone();

	internal int PlayerId(Player<T> player) => player.PlayerId;

	// Para un player, devuelve cuantas fichas tiene en las manoaq
	internal int InHand(int hash) {
		if (Players().Where(x => x.PlayerId == hash).IsEmpty()) return -1;
		return _hands[Players().Where(x => x.PlayerId == hash).FirstOrDefault()!].Count;
	} 

	// Devuelve true si dos players estan en el mismo equipo
	internal bool Partnership(int pId1, int pId2) => TeamOf(pId1).Equals(TeamOf(pId2));

	// Devuelve el equipo de un player teniendo su instancia
	internal Team<T> TeamOf(Player<T> player) => _teams.FirstOrDefault(x => x!.Contains(player), default)!;

	// Devuelve el equipo de un player teniendo su iD
	internal Team<T> TeamOf(int playerId) => TeamOf(Players().FirstOrDefault(x => x.PlayerId == playerId)!);

	internal List<Move<T>> Board => _board.ToList();

	// Guarda las manos en el diccionario de manos 
	internal void SetHand(Player<T> player, Hand<T> hand) => Hands.Add(player, hand.Clone());

	// Devuelve todos los players involucrados en la partida
	internal IEnumerable<Player<T>> Players() => _teams.SelectMany(player => player);

	// Devuelve los teams involucrados en la partida
	internal IEnumerable<Team<T>> Teams() => _teams;

    internal Dictionary<Player<T>, Hand<T>> Hands => _hands;
}