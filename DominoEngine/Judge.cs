﻿using System.Diagnostics;

namespace DominoEngine;

internal class Judge<T> {
	private readonly IGenerator<T> _generator;
	private readonly IDealer<T> _dealer;
	private readonly ITurner<T> _turner;
	private readonly IMatcher<T> _matcher;
	private readonly IScorer<T> _scorer;
	private readonly IFinisher<T> _finisher;
	private Partida<T>? _partida;

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
		_partida = partida;
		foreach (var (player, hand) in _dealer.Deal(partida, _generator.Generate()))
			_partida.SetHand(player, hand);
	}

	public IEnumerable<int> Play() {
		foreach (var (i, player) in _turner.Players(_partida!).Enumerate().TakeWhile(_ => !_finisher.GameOver(_partida!))) {
			if (i is 0) {
				Salir(player);
				yield return i;
				continue;
			}

			var validMoves = GenValidMoves(player).ToHashSet();
			var move = player.Play(validMoves, x => _scorer.Scorer(_partida!, x));
			if (!validMoves.Contains(move)) move = validMoves.FirstOrDefault();
			_partida!.AddMove(move!);
			if (!move!.Check) _partida.RemoveFromHand(player, move.Ficha!);
			yield return i;
		}
	}

	private void Salir(Player<T> player) {
		var validMoves = GenSalidas(player).ToHashSet();
		var move = player.Play(validMoves, x => _scorer.Scorer(_partida!, x));
		if (!validMoves.Contains(move)) move = validMoves.FirstOrDefault();
		_partida!.AddMove(move!);
	}

	private IEnumerable<Move<T>> GenMoves(Player<T> player) {
		var playerId = _partida!.PlayerId(player);
		yield return new Move<T>(playerId);
		foreach (var (head, tail) in _partida.Hand(player)) {
			yield return new Move<T>(playerId, false, -1, head, tail);
			yield return new Move<T>(playerId, false, -1, tail, head);
			foreach (var (i, move) in _partida.Board.Enumerate().Where(t => !t.Item2.Check)) {
				yield return new Move<T>(playerId, false, i, head, tail);
				yield return new Move<T>(playerId, false, i, tail, head);
			}
		}
	}

	private IEnumerable<Move<T>> GenValidMoves(Player<T> player) =>
		_matcher.CanMatch(_partida!, GenMoves(player));
		// GenMoves(player).Where(t => _matcher.CanMatch(_partida!, t));

	private IEnumerable<Move<T>> GenSalidas(Player<T> player) {
		var id = _partida!.PlayerId(player);
		foreach (var ficha in _partida.Hand(player)) {
			yield return new Move<T>(id, false, -2, ficha.Head, ficha.Tail);
		}
	}
}