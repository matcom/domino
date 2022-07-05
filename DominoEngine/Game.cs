using System.Collections;
using System.Data;

namespace DominoEngine;

public class Game<T> : IEnumerable<GameState<T>>, IWinnerSelector<T> {
    private Judge<T>? _judge;
    private Partida<T>? _partida;

    public Game(Judge<T> judge, IEnumerable<Team<T>> teams) {
        _judge = judge;
        _partida = new Partida<T>(teams);
    }

    public Game() { }

    public IEnumerator<GameState<T>> GetEnumerator() {
        _judge!.Start(_partida!);
        var first_state = new List<GameState<T>>() { new GameState<T>(_partida!.Board, _partida.Hands) };
        return first_state.Concat(_judge.Play(_partida).
        Select((player, i) => new GameState<T>(_partida.Board, _partida.Hands, i, player))).GetEnumerator();
    }
    
    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

    public IEnumerable<Team<T>> Winner() => _judge!.Winner(_partida!);

    public IWinnerSelector<T> NewInstance(Judge<T> judge, IEnumerable<Team<T>> teams) => new Game<T>(judge, teams);

    public IEnumerable<Game<T>> Games(IWinnerSelector<T> winsel) => new List<Game<T>>(){this};
}

public record GameState<T>(List<Move<T>> Board, Dictionary<Player<T>, Hand<T>> Hands,
    int Turn = -1, Player<T> PlayerToPlay = default!) {
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
