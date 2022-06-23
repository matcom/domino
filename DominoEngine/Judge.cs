using System.Diagnostics;

namespace DominoEngine;

public class Judge<T> {
	protected readonly IGenerator<T> _generator;
	protected readonly IDealer<T> _dealer;
	protected readonly ITurner<T> _turner;
	protected readonly IMatcher<T> _matcher;
	protected readonly IScorer<T> _scorer;
	protected readonly IFinisher<T> _finisher;

	public Judge(IGenerator<T> generator, IDealer<T> dealer, ITurner<T> turner, IMatcher<T> matcher, IScorer<T> scorer,
		IFinisher<T> finisher) {
		_generator = generator;
		_dealer = dealer;
		_turner = turner;
		_matcher = matcher;
		_scorer = scorer;
		_finisher = finisher;
	}

    public void Start(Partida<T> partida) {
		foreach (var (player, hand) in _dealer.Deal(partida, _generator.Generate()))
			partida.SetHand(player.SetHand(hand), hand);
	}

	public IEnumerable<Player<T>> Play(Partida<T> partida) {
		foreach (var (i, player) in _turner.Players(partida!).Enumerate()) {
			yield return player;
			if (_finisher.GameOver(partida!))
				yield break;
			if (i is 0) {
				Salir(partida, player);
				continue;
			}

			var validMoves = GenValidMoves(partida, player).ToHashSet();
			var move = player.Play(validMoves, partida!.Board, x => partida.InHand(x), x => _scorer.Scorer(partida!, x));
			if (!validMoves.Contains(move)) move = validMoves.FirstOrDefault();
			partida!.AddMove(move!);
			if (!move!.Check) partida.RemoveFromHand(player, move.Ficha!);
		}
	}

	private void Salir(Partida<T> partida, Player<T> player) {
		var validMoves = GenSalidas(partida, player).ToHashSet();
		var move = player.Play(validMoves, partida!.Board, x => partida.InHand(x), x => _scorer.Scorer(partida!, x));
		if (!validMoves.Contains(move)) move = validMoves.FirstOrDefault();
		if (!move!.Check) partida!.RemoveFromHand(player, move.Ficha!);
		partida!.AddMove(move!);
	}

	private IEnumerable<Move<T>> GenMoves(Partida<T> partida, Player<T> player) {
		var playerId = partida!.PlayerId(player);
		yield return new Move<T>(playerId);
		foreach (var (head, tail) in partida.Hand(player)) {
			yield return new Move<T>(playerId, false, -1, head, tail);
			yield return new Move<T>(playerId, false, -1, tail, head);
			foreach (var (i, move) in partida.Board.Enumerate().Where(t => !t.Item2.Check)) {
				yield return new Move<T>(playerId, false, i, head, tail);
				yield return new Move<T>(playerId, false, i, tail, head);
			}
		}
	}

	private IEnumerable<Move<T>> GenValidMoves(Partida<T> partida, Player<T> player) =>
		_matcher.CanMatch(partida!, GenMoves(partida, player));

	private IEnumerable<Move<T>> GenSalidas(Partida<T> partida, Player<T> player) {
		var id = partida!.PlayerId(player);
		foreach (var ficha in partida.Hand(player)) {
			yield return new Move<T>(id, false, -2, ficha.Head, ficha.Tail);
		}
	}

	internal Team<T> Winner(Partida<T> partida) => _scorer.Winner(partida);
}

public class ClassicJudge : Judge<int>
{
    public ClassicJudge() : base(new ClassicGenerator(10), new ClassicDealer<int>(10), 
		new ClassicTurner<int>(), new ClassicMatcher<int>(), new ClassicScorer(), new ClassicFinisher<int>()) { }
}
