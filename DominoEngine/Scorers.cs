using DominoEngine;

namespace Rules;

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
