using DominoEngine;

namespace Rules;

public class ClassicDealer<T> : IDealer<T>
{
    private readonly int _pieceForPlayers;
    public ClassicDealer(int piecesForPlayers) {
        _pieceForPlayers = piecesForPlayers;
    }

    public Dictionary<Player<T>, Hand<T>> Deal(Partida<T> partida, IEnumerable<Token<T>> tokens) {
        Dictionary<Player<T>, Hand<T>> hands = new();
        var r = new Random();
        using var enumerator = tokens.OrderBy(x => r.NextDouble() - 0.5).GetEnumerator();
        var count = 0;

        foreach (var player in partida.Players()) {
            var hand = new Hand<T>();
            while (count < _pieceForPlayers && enumerator.MoveNext()) {
                hand.Add(enumerator.Current);
                count++;
            }
            hands.Add(player, hand);
            count = 0;
        }

        return hands;
    }
}