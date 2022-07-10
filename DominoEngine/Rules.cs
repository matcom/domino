namespace DominoEngine;

#region IScorers
public class ClassicScorer : IScorer<int>
{
    //Scorer clasico
    public double Scorer(Partida<int> partida, Move<int> move) => TokenScorer(move.Token);

    public double TokenScorer(Token<int> token) => token.Head + token.Tail;

    public IEnumerable<Team<int>> Winner(Partida<int> partida) {
        foreach (var player in partida.Players().Where(x => partida.Hands[x].IsEmpty())) {
            var winners = new List<Team<int>>(){partida.TeamOf(player)};
            return winners.Concat(partida.Teams().Complement(winners));
        }
        return partida.Teams().OrderByDescending(team => team.Sum(player => partida.Hand(player).
                Sum(TokenScorer)));
    }
}

public class ModFiveScorer : IScorer<int>
{
    // Solo devuelve puntuacion si la suma es divisible por 5
    public double Scorer(Partida<int> partida, Move<int> move) {
        if ((TokenScorer(partida.Board[move.Turn].Token) + TokenScorer(move.Token) % 5 is 0))
            return TokenScorer(partida.Board[move.Turn].Token) + TokenScorer(move.Token);
        else return 0;
    }

    public double TokenScorer(Token<int> token) => token.Head + token.Tail;

    // Devuelve los equipos rankeados por la suma de la puntuacion de sus jugadores
    public IEnumerable<Team<int>> Winner(Partida<int> partida)
        => partida.Teams().OrderByDescending(team => team.Sum(player => partida.Board.
            Where(move => move.PlayerId == partida.PlayerId(player) && !move.Check).Sum(move => Scorer(partida, move))));
        // => partida.TeamOf(partida.Players().MaxBy(player => partida.Board.
        //     Where(move => move.PlayerId == partida.PlayerId(player) && !move.Check).Sum(move => Scorer(partida, move)))!);
}

public class TurnDividesBoardScorer : IScorer<int>
{
    private readonly Dictionary<Partida<int>, List<(int turn, int score)>> _scores = new();

    public double Scorer(Partida<int> partida, Move<int> move) {
        if (!_scores.ContainsKey(partida))
            _scores.Add(partida, new List<(int turn, int score)>(){(0, 0)});
        if (_scores[partida].Count is 1)
            return TokenScorer(move.Token);
        else {
            Update(partida);
            if (_scores[partida].Last().score + TokenScorer(move.Token) % (_scores[partida].Last().score + 1) is 0)
                return TokenScorer(move.Token);
            else return 0;
        }
    }

    private void Update(Partida<int> partida)
        => partida.Board.Enumerate().Where(pair => !pair.Item2.Check && pair.Item1 > _scores[partida].Count).
            Make(pair => _scores[partida].Add((pair.Item1, _scores[partida].Last().score + (int)TokenScorer(pair.Item2.Token))));


    public double TokenScorer(Token<int> token) => token.Head + token.Tail;

    public IEnumerable<Team<int>> Winner(Partida<int> partida)
        => partida.Teams().OrderByDescending(team => team.Sum(player => partida.Board.Enumerate().
            Where(pair => !pair.Item2.Check && pair.Item2.PlayerId == partida.PlayerId(player)).
            Sum(pair => _scores[partida].First(x => x.turn == pair.Item1).score)));
}

public static class ScorerExtensors
{
    public static IScorer<TSource> Inverse<TSource>(this IScorer<TSource> scorer)
        => new InverseScorer<TSource>(scorer);
}

internal class InverseScorer<T> : IScorer<T>
{
    private readonly IScorer<T> _scorer;

    public InverseScorer(IScorer<T> scorer) {
        _scorer = scorer;
    }

    public double Scorer(Partida<T> partida, Move<T> move)
        => _scorer.Scorer(partida, move) is 0 ? int.MaxValue : 1 / (_scorer.Scorer(partida, move));


    public double TokenScorer(Token<T> token)
        => _scorer.TokenScorer(token) is 0 ? int.MaxValue : 1 / (_scorer.TokenScorer(token));

        public IEnumerable<Team<T>> Winner(Partida<T> partida) => _scorer.Winner(partida).Reverse();
}

#endregion

#region  IFinishers
public class ClassicFinisher<T> : IFinisher<T>
{
    public bool GameOver(Partida<T> partida)
        => AllCheck(partida) || PlayerEnd(partida);

    private bool AllCheck(Partida<T> partida)
        => partida.Players().All(player => !partida.Board.Where(move => move.PlayerId == partida.PlayerId(player)).IsEmpty() 
                                               && partida.Board.Last(x => x.PlayerId == partida.PlayerId(player)).Check);

        private static bool PlayerEnd(Partida<T> partida) 
        => partida.Players().Any(player => partida.Hand(player).IsEmpty());
}

public class TurnCountFinisher<T> : IFinisher<T>
{
    private readonly int _number;
    public TurnCountFinisher(int number) {
        _number = number;
    }

    public bool GameOver(Partida<T> partida)
        => partida.Board.Count(move => !move.Check) > _number;
}

public class PassesCountFinisher<T> : IFinisher<T>
{
    private readonly int _number;
    public PassesCountFinisher(int number)
    {
        _number = number;
    }

    public bool GameOver(Partida<T> partida)
        => partida.Board.Count(move => move.Check) > _number;
}

public static class FinisherExtensors
{
    public static IFinisher<TSource> Join<TSource>(this IFinisher<TSource> finisher1, IFinisher<TSource> finisher2)
        => new JoinFinisher<TSource>(finisher1, finisher2);

    public static IFinisher<TSource> Intersect<TSource>(this IFinisher<TSource> finisher1, IFinisher<TSource> finisher2)
        => new JoinFinisher<TSource>(finisher1, finisher2);
}

internal class IntersectFinisher<T> : IFinisher<T>
{
    private readonly IFinisher<T> _finisher1;
    private readonly IFinisher<T> _finisher2;
    public IntersectFinisher(IFinisher<T> finisher1, IFinisher<T> finisher2)
    {
        _finisher1 = finisher1;
        _finisher2 = finisher2;
    }

    public bool GameOver(Partida<T> partida)
        => _finisher1.GameOver(partida) && _finisher2.GameOver(partida);
}

internal class JoinFinisher<T> : IFinisher<T>
{
    private readonly IFinisher<T> _finisher1;
    private readonly IFinisher<T> _finisher2;
    public JoinFinisher(IFinisher<T> finisher1, IFinisher<T> finisher2)
    {
        _finisher1 = finisher1;
        _finisher2 = finisher2;
    }

    public bool GameOver(Partida<T> partida)
        => _finisher1.GameOver(partida) || _finisher2.GameOver(partida);
}

#endregion

#region IMatchers
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
        return partida.Board.IsEmpty() || _validsTurns[partida]!.Where(x => x == move.Turn).
            Select(validturn => validturn == -1 ? partida.Board[0].Head!.Equals(move.Head) 
                : partida.Board[validturn].Tail!.Equals(move.Head)).FirstOrDefault();
    }

    private void ValidsTurn(Partida<T> partida) {
        // Si no existe añadimos una partida nueva al diccionario
        if (!_validsTurns.ContainsKey(partida)) _validsTurns.Add(partida, new List<int>() { 0, -1 });

        // Actuliza los turnos validos de la forma clasica
        foreach (var (i, move) in partida.Board.Enumerate().Where(x => !x.Item2.Check &&
                x.Item1 > _validsTurns[partida].MaxBy(x => x) && x.Item1 >= 1))
        {
            _validsTurns[partida].Remove(move.Turn);
            _validsTurns[partida].Add(i);
        }
    }
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
        // Permite salir solo con la mayor de las Tokens dobles
        var higher = partida.Hands.SelectMany(x => x.Value).Where(x => x.Head!.Equals(x.Tail)).MaxBy(x => tokenScorer(x));
        if (partida.Board.IsEmpty()) return move.Token.Equals(higher!);

        // Valida movimientos si estan disponibles y cumplen la condicion de longana
        return _validsTurns[partida][move.PlayerId].Select(x => x.turn).Where(x => x == move.Turn).
            Select(validturn => validturn is -1 ? partida.Board[0].Head!.Equals(move.Head) 
                : partida.Board[validturn].Tail!.Equals(move.Head)).FirstOrDefault();
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
        => partida.Board.Select(move => move.Turn);

    // Matchea solo si la ficha por donde va ajugar y esta tienen sumas primas relativas
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
        => partida.Board.Select(move => move.Turn);

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
        => partida.Board.Select(move => move.Turn);

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
        => partida.Board.Select(move => move.Turn).Complement(_matcher.ValidsTurns(partida, player));
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

#endregion

#region  ITurners
public class ClassicTurner<T> : ITurner<T>
{
    // Turner clasico que reparte los turnos en un orden estricto
    public IEnumerable<Player<T>> Players(Partida<T> partida) => partida.Teams().OneByOne().Infinity();
}

public class NPassesReverseTurner<T> : ITurner<T>
{
    private readonly int _n;
    public NPassesReverseTurner(int n) {
        _n = n;
    }

    // Cambia el sentido de reparticion de turnos cuando ocurran n pases
    public IEnumerable<Player<T>> Players(Partida<T> partida) {
        var passes = 0;
        var index = 0;
        var players = partida.Teams().OneByOne();

        while (true) {
            foreach (var (i, player) in players.Infinity().Enumerate().Skip(index)) {
                yield return player;
                if (partida.Board.Count(x => x.Check) == passes ||
                    partida.Board.Count(x => x.Check) % _n is not 0) continue;
                passes += _n;
                index = players.Count() - i;
                players = players.Reverse();
                break;
            }
        }
    }    
}

public class RandomTurner<T> : ITurner<T>
{
    public IEnumerable<Player<T>> Players(Partida<T> partida) {
        while (true)
            yield return partida.Players().ElementAt(new Random().Next(partida.Players().Count()));
    }
}

#endregion

#region  IDealers
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

#endregion

#region  IGenerators
public class ClassicGenerator : IGenerator<int>
{
    private readonly int _number;

    public ClassicGenerator(int number) {
        _number = number;
    }

    IEnumerable<Token<int>> IGenerator<int>.Generate() {
        var tokens = new List<Token<int>>();

        for (var i = 0; i < _number; i++)
            for (var j = i; j < _number; j++)
                tokens.Add(new Token<int>(i, j));

        return tokens;
    }
}

public class SumPrimeGenerator : IGenerator<int>
{
    private readonly int _number;

    public SumPrimeGenerator(int number) {
        _number = number;
    }

    IEnumerable<Token<int>> IGenerator<int>.Generate() {
        var tokens = new List<Token<int>>();

        for (var i = 0; i < _number; i++)
            for (var j = i; j < _number; j++)
                tokens.Add(new Token<int>(i, j));

        return tokens.Where(token => IsPrime(token.Head + token.Tail)).ToList();
    }

    private static bool IsPrime(int a) {
        for (var i = 2; i <= Math.Sqrt(a); i++) {
            if (a % i is 0) return false;
        }
        return true;
    }
}

#endregion