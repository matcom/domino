using System.Collections;
using System.Data;

namespace DominoEngine;

public class Game<T> : IEnumerable<GameState<T>> { //hay que hacerlo
	private Judge<T> _judge;
	private Partida<T> _partida;

	public Game(Judge<T> judge, List<Player<T>> players) {
		_judge = judge;
		_partida = new Partida<T>(players);
	}

	public IEnumerator<GameState<T>> GetEnumerator() {
		_judge.Start(_partida);
		return _judge.Play().Select((player, i) => new GameState<T>(i, player, _partida.Board, _partida.Hands))
			.GetEnumerator();
	}

	IEnumerator IEnumerable.GetEnumerator() {
		return GetEnumerator();
	}
}

public record GameState<T>(int Turn, Player<T> PlayerToPlay, List<Move<T>> Board, Dictionary<Player<T>, Hand<T>> Hands);