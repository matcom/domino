using Rules;

namespace DominoEngine;

public class Judge<T> {
	private readonly IGenerator<T> _generator; 
	private readonly IDealer<T> _dealer; 
	private readonly ITurner<T> _turner; 
	private readonly IMatcher<T> _matcher; 
	private readonly IScorer<T> _scorer; 
	private readonly IFinisher<T> _finisher; 

	protected Judge(IGenerator<T> generator, IDealer<T> dealer, ITurner<T> turner, IMatcher<T> matcher, IScorer<T> scorer,
		IFinisher<T> finisher) {
		_generator = generator; 
		_dealer = dealer; 
		_turner = turner; 
		_matcher = matcher; 
		_scorer = scorer; 
		_finisher = finisher; 
	}

    public void Start(Partida<T> partida) {
		// Inicializa la partida, le reparte las manos a los players
		foreach (var (player, hand) in _dealer.Deal(partida, _generator.Generate()))
			partida.SetHand(player.SetHand(hand), hand);
	}

	public IEnumerable<Player<T>> Play(Partida<T> partida) {
		// Mientras no se pueda salir no entra al foreach
		foreach (var (i, player) in _turner.Players(partida).Enumerate().SkipWhile(x => Salir(partida, x.Item2))) {
			if (i is 0) {
				// Si es el primer turno, devuelve al player y pasa a la siguiente iteracion
				yield return player; 
				continue; 
			}
			if (_finisher.GameOver(partida!)) // Si se activa la condicion de finalizacion, sal del foreach
				yield break; 

			var validMoves = GenValidMoves(partida, player).ToHashSet(); // Se generan las jugadas validas
			var move = player.Play(validMoves, partida.PassesInfo,partida.Board.ToList(), partida.InHand,
				x => _scorer.Scorer(partida!, x), partida.Partnership); // El player juega
			if (!validMoves.Contains(move)) move = validMoves.FirstOrDefault(); // Si no es valido, se selecciona jugada valida
			partida.AddMove(move!); // Se agrega la jugada a la partida
			partida.AddValidsTurns(_matcher.ValidsTurns(partida, partida.PlayerId(player))); // Se agrega la jugada a la lista de jugadas validas
			if (!move!.Check) partida.RemoveFromHand(player, move.Token!); // Si no es un pase, se quita de la mano
			yield return player; // Se devuelve al player
		}
	}

	private bool Salir(Partida<T> partida, Player<T> player) {
		var validMoves = GenSalidas(partida, player).ToHashSet(); // Se generan las salidas validas
		if (validMoves.IsEmpty()) return true; // Si no hay salidas validas, devuelve true
		var move = player.Play(validMoves, partida.PassesInfo,partida!.Board.ToList(), partida.InHand, 
			x => _scorer.Scorer(partida, x), partida.Partnership); // El player juega
		if (!validMoves.Contains(move)) move = validMoves.FirstOrDefault(); // Si no es valido, se selecciona jugada valida
		if (!move!.Check) partida!.RemoveFromHand(player, move.Token!); // Si no es un pase, se quita de la mano
		partida!.AddMove(move!); // Se agrega la jugada a la partida
		return false; 
	}

	private static IEnumerable<Move<T>> GenMoves(Partida<T> partida, Player<T> player) {
		var playerId = partida.PlayerId(player); // Se obtiene el id del player
		yield return new Move<T>(playerId); // Se devuelve un pase
		foreach (var (head, tail) in partida.Hand(player)) {
			// Se devuelven las jugadas que apuntan a la salida
			yield return new Move<T>(playerId, false, -1, head, tail); 
			yield return new Move<T>(playerId, false, -1, tail, head); 
			foreach (var (i, move) in partida.Board.Enumerate().Where(t => !t.Item2.Check)) {
				// Se devuelven las jugadas que apuntan al resto del tablero
				yield return new Move<T>(playerId, false, i, head, tail);
				yield return new Move<T>(playerId, false, i, tail, head); 
			}
		}
	}

	private IEnumerable<Move<T>> GenValidMoves(Partida<T> partida, Player<T> player) =>
		_matcher.CanMatch(partida!, GenMoves(partida, player), _scorer.TokenScorer); // Se devuelven las jugadas validas

	private IEnumerable<Move<T>> GenSalidas(Partida<T> partida, Player<T> player) {
		var id = partida.PlayerId(player); // Se obtiene el id del player
		foreach (var move in _matcher.CanMatch(partida, partida.Hand(player).
				Select(x => new Move<T>(id, false, -1, x.Head, x.Tail)), _scorer.TokenScorer))
			yield return move; // Se devuelven las salidas validas
	}

	internal IEnumerable<Team<T>> Winner(Partida<T> partida) => _scorer.Winner(partida); // Se devuelve el ganador
}

public class ClassicJudge : Judge<int>
{
    public ClassicJudge() : base(new ClassicGenerator(), new ClassicDealer<int>(55, 10), 
		new ClassicTurner<int>(), new SideMatcher<int>().Intersect(new EqualMatcher<int>()), 
		new ClassicScorer(), new EmptyHandFinisher<int>()) { }
}
