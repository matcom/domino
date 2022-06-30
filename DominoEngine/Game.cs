using System.Collections;
using System.Data;

namespace DominoEngine;

public class Game<T> : IEnumerable<GameState<T>> { //hay que hacerlo
    private Judge<T> _judge;
    private Partida<T> _partida;

    public Game(Judge<T> judge, List<Team<T>> teams)
    {
        _judge = judge;
        _partida = new Partida<T>(teams);
    }

    public IEnumerator<GameState<T>> GetEnumerator()
    {
        _judge.Start(_partida);
        var first_state = new List<GameState<T>>() { new GameState<T>(_partida.Board, _partida.Hands) };
        return first_state.Concat(_judge.Play(_partida).
        Select((player, i) => new GameState<T>(_partida.Board, _partida.Hands, i, player))).GetEnumerator();
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return GetEnumerator();
    }

    public Team<T> Winner() => _judge.Winner(_partida);
}

public record GameState<T>(List<Move<T>> Board, Dictionary<Player<T>, Hand<T>> Hands,
                int Turn = -1, Player<T> PlayerToPlay = default!)
{
    public override string ToString()
    {
        string result = "";
        if (Turn is not -1) {
            result += $"Turn: {Turn}\n";
            result += $"Player to Play: {PlayerToPlay.ToString()}\n";
        }
        result += $"Hands:\n";
        foreach (var (player, hand) in Hands)
            result += $"Player {player.ToString()}: {hand.ToString()}\n";
        int count = 0;
        foreach (var move in Board)
            result += $"Turn {count++}: Player {move.PlayerId}: {move.ToString()}\n";
        return result;
    }
}
