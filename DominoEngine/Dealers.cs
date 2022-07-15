using DominoEngine;

namespace Rules;

public class ClassicDealer<T> : IDealer<T>
{
    private readonly int _numberOfPieces;
    private readonly int _pieceForPlayers;
    public ClassicDealer(int numberOfPieces, int piecesForPlayers) {
        _numberOfPieces = numberOfPieces;
        _pieceForPlayers = piecesForPlayers;
    }

    public Dictionary<Player<T>, Hand<T>> Deal(Partida<T> partida, IEnumerable<Token<T>> tokens) {
        Dictionary<Player<T>, Hand<T>> hands = new();
        var r = new Random();
        using var enumerator = tokens.Take(_numberOfPieces).OrderBy(x => r.NextDouble() - 0.5).GetEnumerator();
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

    public override string ToString()
        => "Toma una cantidad de fichas y reparte el mismo numero por jugador";
}

public class EvenDealer : IDealer<int>
{
    private readonly int _numberOfPieces;
    private readonly int _pieceForPlayers;
    public EvenDealer(int numberOfPieces, int piecesForPlayers) {
        _numberOfPieces = numberOfPieces;
        _pieceForPlayers = piecesForPlayers;
    }

    public Dictionary<Player<int>, Hand<int>> Deal(Partida<int> partida, IEnumerable<Token<int>> tokens) {
        Dictionary<Player<int>, Hand<int>> hands = new();
        var r = new Random();
        using var enumerator = tokens.Where(token => (token.Head + token.Tail) % 2 is 0).
            Take(_numberOfPieces).OrderBy(x => r.NextDouble() - 0.5).GetEnumerator();
        var count = 0;

        foreach (var player in partida.Players()) {
            var hand = new Hand<int>();
            while (count < _pieceForPlayers && enumerator.MoveNext()) {
                hand.Add(enumerator.Current);
                count++;
            }
            hands.Add(player, hand);
            count = 0;
        }

        return hands;
    }

    public override string ToString()
        => "Toma una cantidad de fichas con suma par y reparte el mismo numero por jugador";
}

public class OddDealer : IDealer<int>
{
    private readonly int _numberOfPieces;
    private readonly int _pieceForPlayers;
    public OddDealer(int numberOfPieces, int piecesForPlayers) {
        _numberOfPieces = numberOfPieces;
        _pieceForPlayers = piecesForPlayers;
    }

    public Dictionary<Player<int>, Hand<int>> Deal(Partida<int> partida, IEnumerable<Token<int>> tokens) {
        Dictionary<Player<int>, Hand<int>> hands = new();
        var r = new Random();
        using var enumerator = tokens.Where(token => (token.Head + token.Tail) % 2 is not 0).
            Take(_numberOfPieces).OrderBy(x => r.NextDouble() - 0.5).GetEnumerator();
        var count = 0;

        foreach (var player in partida.Players()) {
            var hand = new Hand<int>();
            while (count < _pieceForPlayers && enumerator.MoveNext()) {
                hand.Add(enumerator.Current);
                count++;
            }
            hands.Add(player, hand);
            count = 0;
        }

        return hands;
    }

    public override string ToString()
        => "Toma una cantidad de fichas con suma impar y reparte el mismo numero por jugador";
}