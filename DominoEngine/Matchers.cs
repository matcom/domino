using DominoEngine;

namespace Rules;

public class ClassicMatcher<T> : IMatcher<T>
{
    private readonly Dictionary<Partida<T>, List<int>> _validsTurns = new();

    public IEnumerable<Move<T>> CanMatch(Partida<T> partida, IEnumerable<Move<T>> enumerable,
            Func<Token<T>, double> tokenScorer) {
                ValidsTurn(partida);
                var enume = enumerable.Where(x => !x.Check && CanMatch(partida, x));
                return (enume.IsEmpty()) ? enumerable.Where(x => x.Check) : enume;
            }

    public IEnumerable<int> ValidsTurns(Partida<T> partida, int player) => _validsTurns[partida];

    private bool CanMatch(Partida<T> partida, Move<T> move) {
        // Permite salir con cualquier token
        return partida.Board.IsEmpty() || _validsTurns[partida]!.Contains(move.Turn);
        // .
        //     Select(validturn => validturn == -1 ? partida.Board[0].Head!.Equals(move.Head) 
        //         : partida.Board[validturn].Tail!.Equals(move.Head)).FirstOrDefault();
    }

    private void ValidsTurn(Partida<T> partida) {
        // Si no existe añadimos una partida nueva al diccionario
        if (!_validsTurns.ContainsKey(partida)) _validsTurns.Add(partida, new List<int>() { 0, -1 });

        // Actuliza los turnos validos de la forma clasica
        foreach (var (i, move) in partida.Board.Enumerate().Where(x => !x.Item2.Check &&
                x.Item1 > _validsTurns[partida].MaxBy(x => x) && x.Item1 >= 1)) {
            _validsTurns[partida].Remove(move.Turn);
            _validsTurns[partida].Add(i);
        }
    }
}

public class EqualMatcher<T> : IMatcher<T>
{
    public IEnumerable<Move<T>> CanMatch(Partida<T> partida, IEnumerable<Move<T>> enumerable,
        Func<Token<T>, double> tokenScorer) {
        var enume = enumerable.Where(x => !x.Check && CanMatch(partida, x));
        return (enume.IsEmpty()) ? enumerable.Where(x => x.Check) : enume;
    }

    private bool CanMatch(Partida<T> partida, Move<T> move) 
        => partida.Board.IsEmpty() || partida.Board[move.Turn].Tail!.Equals(move.Head);

    public IEnumerable<int> ValidsTurns(Partida<T> partida, int player)
        => partida.Board.Where(move => !move.Check).Select(move => move.Turn);
}

public class LonganaMatcher<T> : IMatcher<T>
{
    private readonly Dictionary<Partida<T>, Dictionary<int, List<(int turn, int player)>>> _validsTurns = new();

    public IEnumerable<Move<T>> CanMatch(Partida<T> partida, IEnumerable<Move<T>> enumerable,
            Func<Token<T>, double> tokenScorer) {
        ValidsTurn(partida, enumerable.First().PlayerId);
        var enume = enumerable.Where(x => !x.Check && CanMatch(partida, x, tokenScorer));
        return (enume.IsEmpty()) ? enumerable.Where(x => x.Check) : enume;
    }

    public IEnumerable<int> ValidsTurns(Partida<T> partida, int player)
        => _validsTurns[partida][player].Select(pair => pair.turn);

    private bool CanMatch(Partida<T> partida, Move<T> move, Func<Token<T>, double> tokenScorer) {
        // Permite salir solo con la mayor de los tokens dobles
        var higher = partida.Hands.SelectMany(x => x.Value).
            Where(x => x.Head!.Equals(x.Tail)).MaxBy(tokenScorer);
        if (partida.Board.IsEmpty()) return move.Token.Equals(higher!);

        // Valida movimientos si estan disponibles y cumplen la condicion de longana
        return _validsTurns[partida][move.PlayerId].Select(x => x.turn).Contains(move.Turn);
        // .Where(x => x == move.Turn).
        //     Select(validturn => validturn is -1 ? partida.Board[0].Head!.Equals(move.Head) 
        //         : partida.Board[validturn].Tail!.Equals(move.Head)).FirstOrDefault();
    }

    private void ValidsTurn(Partida<T> partida, int player) {
        // Agregamos una nueva partida al diccionario de partidas si no existe
        if (!_validsTurns.ContainsKey(partida)) {
            _validsTurns.Add(partida, new Dictionary<int, List<(int, int)>>());
            partida.Players().Select(partida.PlayerId).Make(x => _validsTurns[partida].
            Add(x, new List<(int, int)>() { (-1, x) }));
        }

        // Eliminamos los turnos que ya no se pueden usar
        _validsTurns[partida].Where(x => x.Key != player).
        Make(x => x.Value.Remove(_validsTurns[partida][player].First(x => x.player == player)));

        // Por cada jugador que se paso, agregamos un turno valido
        foreach (var playerId in partida.Players().Select(x => partida.PlayerId(x)).Where(x => x != player)) {
            if (partida.Board.All(x => x.PlayerId != playerId))
                continue;
            var lastmove = partida.Board.Last(x => x.PlayerId == playerId);
            if (lastmove.Check)
                _validsTurns[partida][player].Add(_validsTurns[partida][playerId].
                Where(x => x.player == playerId).MaxBy(x => x.Item1));
        }

        // Actualiza los ultimos cambios en el tablero
        foreach (var (i, move) in partida.Board.Enumerate().Where(x => x.Item1 > _validsTurns[partida].
                SelectMany(x => x.Value).MaxBy(x => x.Item1).Item1 && !x.Item2.Check)) {
            // Si es el primer movimiento del juego, solo modificamos a un jugador
            if (i is 0 || move.Turn is -1) {
                _validsTurns[partida][move.PlayerId] = new List<(int turn, int player)>() { (i, move.PlayerId) };
                continue;
            }

            // Sino modificamos a todos los que sean necesarios
            var turnOwner = _validsTurns[partida].First(x => x.Value.Contains((move.Turn, x.Key))).Key;
            foreach (var player_ in partida.Players().Select(partida.PlayerId)) {
                // Actualiza si el turno fue jugado por el dueño de la rama
                if (move.PlayerId == turnOwner && move.PlayerId == player_) {
                    _validsTurns[partida][player_].Remove((move.Turn, move.PlayerId));
                    _validsTurns[partida][player_].Add((i, player_));
                }
                // Actualiza si el turno no fue jugado por el dueño de la rama
                else
                    if (_validsTurns[partida][player_].RemoveAll(x => x.turn == move.Turn) > 0)
                        _validsTurns[partida][player_].Add((i, turnOwner));
            }
        }
    }
}

public class RelativesPrimesMatcher : IMatcher<int>
{
    public IEnumerable<Move<int>> CanMatch(Partida<int> partida, IEnumerable<Move<int>> enumerable, 
            Func<Token<int>, double> tokenScorer) {
        var enume = enumerable.Where(x => !x.Check && CanMatch(partida, x, tokenScorer));
        return (enume.IsEmpty()) ? enumerable.Where(x => x.Check) : enume;
    }

    public IEnumerable<int> ValidsTurns(Partida<int> partida, int player)
        => partida.Board.Where(move => !move.Check).Select(move => move.Turn);

    // Matchea solo si la ficha por donde va a jugar y esta tienen sumas primas relativas
    private static bool CanMatch(Partida<int> partida, Move<int> move, Func<Token<int>, double> token_scorer) {
        if (move.Turn is -1) return true;
        return Mcd((int)token_scorer(partida.Board[move.Turn].Token), (int)token_scorer(move.Token)) is 1;
    }

    private static int Mcd(int a, int b)
        => a is 0 || b is 0 ? 0 : (a % b is 0) ? b : Mcd(b, a % b);
}

public class EvenOddMatcher : IMatcher<int>
{
    public IEnumerable<Move<int>> CanMatch(Partida<int> partida, IEnumerable<Move<int>> enumerable, 
            Func<Token<int>, double> tokenScorer) {
        var enume = enumerable.Where(x => !x.Check && CanMatch(partida, x, tokenScorer));
        return (enume.IsEmpty()) ? enumerable.Where(x => x.Check) : enume;
    }

    public IEnumerable<int> ValidsTurns(Partida<int> partida, int player)
        => partida.Board.Where(move => !move.Check).Select(move => move.Turn);

    // Devuelve true si el token y el turno tienen la misma paridad
    private static bool CanMatch(Partida<int> partida, Move<int> move, Func<Token<int>, double> tokenScorer)
        => (int)tokenScorer(move.Token) % 2  == partida.Board.Count % 2;
}

public class TeamTokenInvalidMatcher<T> : IMatcher<T>
{
    public IEnumerable<Move<T>> CanMatch(Partida<T> partida, IEnumerable<Move<T>> enumerable, 
            Func<Token<T>, double> tokenScorer) {
                var enume = enumerable.Where(x => !x.Check && CanMatch(partida, x));
                return (enume.IsEmpty()) ? enumerable.Where(x => x.Check) : enume;
    }

    public IEnumerable<int> ValidsTurns(Partida<T> partida, int player)
        => partida.Board.Where(move => !move.Check).Select(move => move.Turn);

    // Devuelve false si se quiere jugar por el turno donde jugo uno del mismo equipo
    private static bool CanMatch(Partida<T> partida, Move<T> move) {
        var team = partida.TeamOf(move.PlayerId);
        if (partida.Board.Where(move => partida.TeamOf(move.PlayerId).Equals(team)).IsEmpty() || move.Turn < 0)
            return true;
        else 
            return team.Equals(partida.TeamOf(partida.Board[move.Turn].PlayerId));
    }
}

public static class MatcherExtensors
{
    public static IMatcher<TSource> Intersect<TSource>(this IMatcher<TSource> matcher1, IMatcher<TSource> matcher2)
        => new IntersectMatcher<TSource>(matcher1, matcher2);

    public static IMatcher<TSource> Inverse<TSource>(this IMatcher<TSource> matcher)
        => new InverseMatcher<TSource>(matcher);

    public static IMatcher<TSource> Join<TSource>(this IMatcher<TSource> matcher1, IMatcher<TSource> matcher2)
        => new JoinMatcher<TSource>(matcher1, matcher2);
}

internal class InverseMatcher<T> : IMatcher<T>
{
    private readonly IMatcher<T> _matcher;

    public InverseMatcher(IMatcher<T> matcher) {
        _matcher = matcher;
    }

    public IEnumerable<Move<T>> CanMatch(Partida<T> partida, IEnumerable<Move<T>> enumerable,
            Func<Token<T>, double> tokenScorer) {
                var enume = enumerable.Complement(_matcher.CanMatch(partida, enumerable, tokenScorer));
                return (enume.IsEmpty()) ? enumerable.Where(x => x.Check) : enume;
            }

    public IEnumerable<int> ValidsTurns(Partida<T> partida, int player)
        => partida.Board.Where(move => !move.Check).Select(move => move.Turn).Complement(_matcher.ValidsTurns(partida, player));
}

internal class IntersectMatcher<T> : IMatcher<T>
{
    private readonly IMatcher<T> _matcher1;
    private readonly IMatcher<T> _matcher2; 

    public IntersectMatcher(IMatcher<T> matcher1, IMatcher<T> matcher2) {
        (_matcher1, _matcher2) = (matcher1, matcher2);
    }

    public IEnumerable<Move<T>> CanMatch(Partida<T> partida, IEnumerable<Move<T>> enumerable, 
            Func<Token<T>, double> tokenScorer) {
        var enume = _matcher1.CanMatch(partida, enumerable, tokenScorer).
            Intersect(_matcher2.CanMatch(partida, enumerable, tokenScorer));
        return (enume.IsEmpty()) ? enumerable.Where(move => move.Check) : enume;
    }

    public IEnumerable<int> ValidsTurns(Partida<T> partida, int player)
        => _matcher1.ValidsTurns(partida, player).Intersect(_matcher2.ValidsTurns(partida, player));
}

internal class JoinMatcher<T> : IMatcher<T>
{
    private readonly IMatcher<T> _matcher1;
    private readonly IMatcher<T> _matcher2;

    public JoinMatcher(IMatcher<T> matcher1, IMatcher<T> matcher2)
    {
        (_matcher1, _matcher2) = (matcher1, matcher2);
    }

    public IEnumerable<Move<T>> CanMatch(Partida<T> partida, IEnumerable<Move<T>> enumerable, 
            Func<Token<T>, double> tokenScorer) {
        var enume = _matcher1.CanMatch(partida, enumerable, tokenScorer).
            Union(_matcher2.CanMatch(partida, enumerable, tokenScorer));
        return (enume.IsEmpty()) ? enumerable.Where(move => move.Check) : enume;
    }

    public IEnumerable<int> ValidsTurns(Partida<T> partida, int player)
        => _matcher1.ValidsTurns(partida, player).Union(_matcher2.ValidsTurns(partida, player));
}
