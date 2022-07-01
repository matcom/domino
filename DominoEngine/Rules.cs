using System.Collections;

namespace DominoEngine;

#region IScorers
public class ClassicScorer : IScorer<int>
{
    public double Scorer(Partida<int> partida, Move<int> move) => TokenScorer(move.Ficha);

    public double TokenScorer(Ficha<int> token) => token.Head + token.Tail;

    public Team<int> Winner(Partida<int> partida) {
        foreach (var player in partida.Players().Where(x => partida.Hands[x].IsEmpty()))
            return partida.TeamOf(player);
        return partida.TeamOf(partida.Hands.MinBy(x => x.Value.Sum(x => TokenScorer(x))).Key);
    }
}

public class ModFiveScorer : IScorer<int>
{
    // Solo devuelve puntuacion si la suma es divisible por 5
    public double Scorer(Partida<int> partida, Move<int> move) {
        if ((TokenScorer(partida.Board[move.Turn].Ficha) + TokenScorer(move.Ficha) % 5 is 0))
            return TokenScorer(partida.Board[move.Turn].Ficha) + TokenScorer(move.Ficha);
        else return 0;
    }

    public double TokenScorer(Ficha<int> token) => token.Head + token.Tail;

    // DEvuelve al equipo q tiene al jugador con la mayor puntuacion
    public Team<int> Winner(Partida<int> partida)
        => partida.TeamOf(partida.Players().MaxBy(player => partida.Board.
            Where(move => move.PlayerId == partida.PlayerId(player) && !move.Check).Sum(move => Scorer(partida, move)))!);
}

public class TurnDividesBoardScorer : IScorer<int>
{
    Dictionary<Partida<int>, List<(int turn, int score)>> _scores = new();

    public double Scorer(Partida<int> partida, Move<int> move) {
        if (!_scores.ContainsKey(partida))
            _scores.Add(partida, new List<(int turn, int score)>(){(0, 0)});
        if (_scores[partida].Count is 1)
            return TokenScorer(move.Ficha);
        else {
            Update(partida);
            if (_scores[partida].Last().score + TokenScorer(move.Ficha) % (_scores[partida].Last().score + 1) is 0)
                return _scores[partida].Last().score + TokenScorer(move.Ficha);
            else return 0;
        }
    }

    private void Update(Partida<int> partida)
        => partida.Board.Enumerate().Where(pair => !pair.Item2.Check && pair.Item1 > _scores[partida].Count).
            Make(pair => _scores[partida].Add((pair.Item1, _scores[partida].Last().score + (int)TokenScorer(pair.Item2.Ficha))));


    public double TokenScorer(Ficha<int> token) => token.Head + token.Tail;

    public Team<int> Winner(Partida<int> partida)
        => partida.TeamOf(partida.Players().MaxBy(player => partida.Board.Enumerate().Where(pair => !pair.Item2.Check 
            && pair.Item2.PlayerId == partida.PlayerId(player)).
            Sum(pair => _scores[partida].Where(x => x.turn == pair.Item1).First().score))!);
}

public class InverseScorer<T> : IScorer<T>
{
    private IScorer<T> _scorer;

    public InverseScorer(IScorer<T> scorer) {
        _scorer = scorer;
    }

    public double Scorer(Partida<T> partida, Move<T> move) {
        if (_scorer.Scorer(partida, move) is 0) return int.MaxValue;
        else return 1 / (_scorer.Scorer(partida, move));
    }

    public double TokenScorer(Ficha<T> token) {
        if (_scorer.TokenScorer(token) is 0) return int.MaxValue;
        else return 1 / (_scorer.TokenScorer(token));
    }

    public Team<T> Winner(Partida<T> partida) {
        foreach (var player in partida.Players().Where(x => partida.Hands[x].IsEmpty()))
            return partida.TeamOf(player);
        return partida.TeamOf(partida.Hands.MinBy(x => x.Value.Sum(x => TokenScorer(x))).Key);
    }
}

#endregion

#region  IFinishers
public class ClassicFinisher<T> : IFinisher<T>
{
    public bool GameOver(Partida<T> partida) {
        return AllCheck(partida) || PlayerEnd(partida);
    }

    public bool AllCheck(Partida<T> partida) {
        foreach (var player in partida.Players()) {
            if (partida.Board.Where(x => x.PlayerId == partida.PlayerId(player)).Count() is 0
                || !partida.Board.Where(x => x.PlayerId == partida.PlayerId(player)).Last().Check)
                return false;
        }

        return true;
    }

    public bool PlayerEnd(Partida<T> partida) {
        foreach (var player in partida.Players())
            if (partida.Hand(player).Count() == 0) return true;

        return false;
    }
}

public class TurnCountFinisher<T> : IFinisher<T>
{
    public bool GameOver(Partida<T> partida)
    {
        throw new NotImplementedException();
    }
}

#endregion

#region IMatchers
public class ClassicMatcher<T> : IMatcher<T>
{
    Dictionary<Partida<T>, List<int>> validsTurns = new();

    public IEnumerable<Move<T>> CanMatch(Partida<T> partida, IEnumerable<Move<T>> enumerable,
            Func<Ficha<T>, double> token_scorer) {
        ValidsTurn(partida);
        var enume = enumerable.Where(x => !x.Check && CanMatch(partida, x));
        return (enume.IsEmpty()) ? enumerable.Where(x => x.Check) : enume;
    }

    private bool CanMatch(Partida<T> partida, Move<T> move) {
        // Permite salir con cualquier ficha
        if (partida.Board.Count == 0) return true;
        foreach (var validturn in validsTurns[partida]!.Where(x => x == move.Turn))
        {
            if (validturn == -1) return partida.Board[0].Head!.Equals(move.Head);
            return partida.Board[validturn].Tail!.Equals(move.Head);
        }
        return false;
    }

    private void ValidsTurn(Partida<T> partida) {
        // Si no existe añadimos una partida nueva al diccionario
        if (!validsTurns.ContainsKey(partida)) validsTurns.Add(partida, new List<int>() { 0, -1 });

        // Actuliza los turnos validos de la forma clasica
        foreach (var (i, move) in partida.Board.Enumerate().Where(x => !x.Item2.Check &&
                x.Item1 > validsTurns[partida].MaxBy(x => x) && x.Item1 >= 1))
        {
            validsTurns[partida].Remove(move.Turn);
            validsTurns[partida].Add(i);
        }
    }
}

public class LonganaMatcher<T> : IMatcher<T> 
{
    Dictionary<Partida<T>, Dictionary<int, List<(int turn, int player)>>> validsTurns = new();

    public IEnumerable<Move<T>> CanMatch(Partida<T> partida, IEnumerable<Move<T>> enumerable,
            Func<Ficha<T>, double> token_scorer) {
        ValidsTurn(partida, enumerable.First().PlayerId);
        var enume = enumerable.Where(x => !x.Check && CanMatch(partida, x, token_scorer));
        return (enume.IsEmpty()) ? enumerable.Where(x => x.Check) : enume;
    }

    private bool CanMatch(Partida<T> partida, Move<T> move, Func<Ficha<T>, double> token_scorer) {
        // Permite salir solo con la mayor de las fichas dobles
        var higher = partida.Hands.SelectMany(x => x.Value).Where(x => x.Head!.Equals(x.Tail)).MaxBy(x => token_scorer(x));
        if (partida.Board.Count() == 0) return move.Ficha.Equals(higher!);

        // Valida movimientos si estan disponibles y cumplen la condicion de longana
        foreach (var validturn in validsTurns[partida][move.PlayerId].Select(x => x.turn).Where(x => x == move.Turn)) {
            if (validturn == -1) return partida.Board[0].Head!.Equals(move.Head);
            return partida.Board[validturn].Tail!.Equals(move.Head);
        }

        return false;
    }

    private void ValidsTurn(Partida<T> partida, int player) {
        // Agregamos una nueva partida al diccionario de partidas si no existe
        if (!validsTurns.ContainsKey(partida)) {
            validsTurns.Add(partida, new Dictionary<int, List<(int, int)>>());
            partida.Players().Select(x => partida.PlayerId(x)).Make(x => validsTurns[partida].
            Add(x, new List<(int, int)>() { (-1, x) }));
        }

        // Eliminamos los turnos que ya no se pueden usar
        validsTurns[partida].Where(x => x.Key != player).
        Make(x => x.Value.Remove(validsTurns[partida][player].Where(x => x.player == player).First()));

        // Por cada jugador que se paso, agregamos un turno valido
        foreach (var playerId in partida.Players().Select(x => partida.PlayerId(x)).Where(x => x != player)) {
            if (partida.Board.Where(x => x.PlayerId == playerId).Count() == 0)
                continue;
            var lastmove = partida.Board.Where(x => x.PlayerId == playerId).Last();
            if (lastmove.Check)
                validsTurns[partida][player].Add(validsTurns[partida][playerId].
                Where(x => x.player == playerId).MaxBy(x => x.Item1));
        }

        // Actualiza los ultimos cambios en el tablero
        foreach (var (i, move) in partida.Board.Enumerate().Where(x => x.Item1 > validsTurns[partida].
                SelectMany(x => x.Value).MaxBy(x => x.Item1).Item1 && !x.Item2.Check)) {
            // Si es el primer movimiento del juego, solo modificamos a un jugador
            if (i is 0 || move.Turn is -1) {
                validsTurns[partida][move.PlayerId] = new List<(int turn, int player)>() { (i, move.PlayerId) };
                continue;
            }

            // Sino modificamos a todos los que sean necesarios
            var turn_owner = validsTurns[partida].Where(x => x.Value.Contains((move.Turn, x.Key))).First().Key;
            foreach (var player_ in partida.Players().Select(x => partida.PlayerId(x))) {
                // Actualiza si el turno fue jugado por el dueño de la rama
                if (move.PlayerId == turn_owner && move.PlayerId == player_) {
                    validsTurns[partida][player_].Remove((move.Turn, move.PlayerId));
                    validsTurns[partida][player_].Add((i, player_));
                }
                // Actualiza si el turno no fue jugado por el dueño de la rama
                else
                    if (validsTurns[partida][player_].RemoveAll(x => x.turn == move.Turn) > 0)
                    validsTurns[partida][player_].Add((i, turn_owner));
            }
        }
    }
}

public class EvenOddMatcher : IMatcher<int>
{
    public IEnumerable<Move<int>> CanMatch(Partida<int> partida, IEnumerable<Move<int>> enumerable, 
            Func<Ficha<int>, double> token_scorer) {
        var enume = enumerable.Where(x => !x.Check && CanMatch(partida, x, token_scorer));
        return (enume.IsEmpty()) ? enumerable.Where(x => x.Check) : enume;
    }

    // Devuelve true si la ficha y el turno tienen la misma paridad
    private bool CanMatch(Partida<int> partida, Move<int> move, Func<Ficha<int>, double> token_scorer)
        => (int)token_scorer(move.Ficha) % 2  == partida.Board.Count % 2;
}

public class TeamTokenInvalidMatcher<T> : IMatcher<T>
{
    // IMatcher<T> _matcher;

    // public TeamTokenInvalidMAtcher(IMatcher<T> matcher)
    // {
    //     _matcher = matcher;
    // }

    public IEnumerable<Move<T>> CanMatch(Partida<T> partida, IEnumerable<Move<T>> enumerable, 
            Func<Ficha<T>, double> token_scorer) {
                var enume = enumerable.Where(x => !x.Check && CanMatch(partida, x));
                return (enume.IsEmpty()) ? enumerable.Where(x => x.Check) : enume;
    }
    //             // var enume = _matcher.CanMatch(partida, enumerable, token_scorer);
    //             // var team = partida.TeamOf(enumerable.First().PlayerId);
    //             // // Si de este equipo aun no ha jugado nadie, juega normalmente
    //             // if (partida.Board.Where(move => partida.TeamOf(move.PlayerId).Equals(team)).IsEmpty()) 
    //             //     return enume;
    //             // else {
    //             //     // En caso contrario, juega solo por donde por fichas que no puso tu equipo
    //             //     var new_enum = enume.Where(move => move.Turn > -1 &&
    //             //     !team.Equals(partida.TeamOf(partida.Board[move.Turn].PlayerId)));
    //             //     return (new_enum.IsEmpty()) ? enumerable.Where(move => move.Check) : new_enum;
    //             // }
    

    bool CanMatch(Partida<T> partida, Move<T> move) {
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

public class InverseMatcher<T> : IMatcher<T>
{
    private IMatcher<T> _matcher;

    public InverseMatcher(IMatcher<T> matcher)
    {
        _matcher = matcher;
    }

    public IEnumerable<Move<T>> CanMatch(Partida<T> partida, IEnumerable<Move<T>> enumerable,
            Func<Ficha<T>, double> token_scorer)
        => enumerable.Complement(_matcher.CanMatch(partida, enumerable, token_scorer));
}

public class IntersectMatcher<T> : IMatcher<T>
{
    IMatcher<T> _matcher1; 
    IMatcher<T> _matcher2; 

    public IntersectMatcher(IMatcher<T> matcher1, IMatcher<T> matcher2)
    {
        (_matcher1, _matcher2) = (matcher1, matcher2);
    }

    public IEnumerable<Move<T>> CanMatch(Partida<T> partida, IEnumerable<Move<T>> enumerable, 
            Func<Ficha<T>, double> token_scorer) {
        var enume = _matcher1.CanMatch(partida, enumerable, token_scorer).
            Intersect(_matcher2.CanMatch(partida, enumerable, token_scorer));
        return (enume.IsEmpty()) ? enumerable.Where(x => x.Check) : enume;
    }
}

public class JoinMatcher<T> : IMatcher<T>
{
    IMatcher<T> _matcher1; 
    IMatcher<T> _matcher2;

    public JoinMatcher(IMatcher<T> matcher1, IMatcher<T> matcher2)
    {
        (_matcher1, _matcher2) = (matcher1, matcher2);
    }

    public IEnumerable<Move<T>> CanMatch(Partida<T> partida, IEnumerable<Move<T>> enumerable, 
            Func<Ficha<T>, double> token_scorer) {
        var enume = _matcher1.CanMatch(partida, enumerable, token_scorer).
            Concat(_matcher2.CanMatch(partida, enumerable, token_scorer));
        return (enume.IsEmpty()) ? enumerable.Where(x => x.Check) : enume;
    }
}

public class MultiMatcher<T> : IMatcher<T>
{
    InfiniteEnumerator<IMatcher<T>> _matcherEnumerator;

    public MultiMatcher(params IMatcher<T>[] matchers)
    {
        _matcherEnumerator = new InfiniteEnumerator<IMatcher<T>>(matchers);
    }

    public IEnumerable<Move<T>> CanMatch(Partida<T> partida, IEnumerable<Move<T>> enumerable, 
            Func<Ficha<T>, double> token_scorer) {
        _matcherEnumerator.MoveNext();
        return _matcherEnumerator.Current.CanMatch(partida, enumerable, token_scorer);
    }
}

#endregion

#region  ITurners
public class ClassicTurner<T> : ITurner<T>
{
    public ClassicTurner() { }

    public IEnumerable<Player<T>> Players(Partida<T> partida) {
        while (true)
            foreach (var player in partida!.Players())
                yield return player;
    }
}

public class NPassesReverse<T> : ITurner<T>
{
    private int _n;

    public NPassesReverse(int n) {
        _n = n;
    }

    public IEnumerable<Player<T>> Players(Partida<T> partida) {
        int passes = 0;
        ReversibleInfiniteEnumerator<Player<T>> enumerator = new ReversibleInfiniteEnumerator<Player<T>>(partida.Players());

        while (enumerator.MoveNext()) {
            yield return enumerator.Current;
            if (partida.Board.Where(x => x.Check).Count() != passes &&
            partida.Board.Where(x => x.Check).Count() % _n is 0) {
                enumerator.Direction();
                passes += partida.Board.Where(x => x.Check).Count();
            }
        }
    }

    
}

#endregion

#region  IDealers
public class ClassicDealer<T> : IDealer<T>
{
    int _pieceForPlayers;
    public ClassicDealer(int piecesForPlayers) {
        _pieceForPlayers = piecesForPlayers;
    }

    public Dictionary<Player<T>, Hand<T>> Deal(Partida<T> partida, IEnumerable<Ficha<T>> fichas) {
        Dictionary<Player<T>, Hand<T>> hands = new();
        Random r = new Random();
        var enumerator = fichas.OrderBy(x => r.NextDouble() - 0.5).GetEnumerator();

        foreach (var player in partida.Players())
        {
            var hand = new Hand<T>();
            var count = 0;
            while (count++ < _pieceForPlayers && enumerator.MoveNext())
                hand.Add(enumerator.Current);
            hands.Add(player, hand);
        }

        return hands;
    }
}

#endregion

#region  IGenerators
public class ClassicGenerator : IGenerator<int>
{
    int _number;

    public ClassicGenerator(int number) {
        _number = number;
    }

    IEnumerable<Ficha<int>> IGenerator<int>.Generate() {
        List<Ficha<int>> fichas = new List<Ficha<int>>();

        for (int i = 0; i < _number; i++)
            for (int j = i; j < _number; j++)
                fichas.Add(new Ficha<int>(i, j));

        return fichas;
    }
}

#endregion