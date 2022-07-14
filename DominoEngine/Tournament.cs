using System.Collections;

namespace DominoEngine;

public abstract class Tournament<T> : IEnumerable<Game<T>>, IWinnerSelector<T>
{
    protected Judge<T>? Judge;
    protected IEnumerable<Team<T>>? Teams;

    protected Tournament(Judge<T> judge, IEnumerable<Team<T>> teams) {
        Judge = judge;
        Teams = teams;
    }

    protected Tournament() { }

    public Tournament<T> SetJudge(Judge<T> judge) {
        Judge =  judge;
        return this;
    }

    public Tournament<T> SetTeams(IEnumerable<Team<T>> teams) {
        Teams = teams;
        return this;
    }

    public abstract IEnumerable<Game<T>> Games(IWinnerSelector<T> winsel);

    public abstract IEnumerable<Team<T>> Winner();

    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

    public virtual IEnumerator<Game<T>> GetEnumerator() => Games(new Game<T>()).GetEnumerator();

    public abstract IWinnerSelector<T> NewInstance(Judge<T> judge, IEnumerable<Team<T>> teams);
}

public class AllVsAllTournament<T> : Tournament<T>
{
    private readonly Dictionary<Team<T>, int> _games = new();

    public AllVsAllTournament() { }

    private AllVsAllTournament(Judge<T> judge, IEnumerable<Team<T>> teams) : base(judge, teams) { }

    public override IEnumerable<Game<T>> Games(IWinnerSelector<T> winsel) {
        Teams!.Make(team => _games.Add(team, 0)); // Inicializar el contador de partidas de cada equipo
        foreach (var (i, team1) in Teams!.Enumerate())
            foreach (var (j,team2) in Teams!.Enumerate().Where(pair => pair.Item1 != i)) {
                var newWinsel = winsel.NewInstance(Judge!, new List<Team<T>>{team1, team2});
                foreach (var game in newWinsel.Games(new Game<T>())) 
                    yield return game; // Devuelve un Game por cada combinacion de equipos
                _games[newWinsel.Winner().First()] += 3; // Incrementar el contador de puntos del equipo ganador
            }
    }

    public override IWinnerSelector<T> NewInstance(Judge<T> judge, IEnumerable<Team<T>> teams)
        => new AllVsAllTournament<T>(judge, teams);

    public override IEnumerable<Team<T>> Winner() => _games.Keys.OrderByDescending(team => _games[team])!;
}

public class DirichletTournament<T> : Tournament<T>
{
    private readonly Dictionary<Team<T>, List<IWinnerSelector<T>>> _games = new(); 
    private readonly int _numberOfWins;

    public DirichletTournament(int number) {
        _numberOfWins = number; 
    }

    public DirichletTournament(Judge<T> judge, IEnumerable<Team<T>> teams, int number) : base(judge, teams) {
        _numberOfWins = number;
    }

    public override IEnumerable<Game<T>> Games(IWinnerSelector<T> winsel) {
        while (EndCondition()) {
            var newWinsel = winsel.NewInstance(Judge!, Teams!); 
            foreach (var game in newWinsel.Games(new Game<T>()))
                yield return game; 
            var team = newWinsel.Winner().First();
            if (!_games.ContainsKey(team)) _games.Add(team, new List<IWinnerSelector<T>>(){newWinsel});
            else _games[team].Add(newWinsel);
        }
    }

    private bool EndCondition() => _games.All(pair => pair.Value.Count() < _numberOfWins);

    public override IEnumerable<Team<T>> Winner() => _games.Keys.OrderByDescending(team => _games[team].Count)!;

    public override IWinnerSelector<T> NewInstance(Judge<T> judge, IEnumerable<Team<T>> teams) 
        => new DirichletTournament<T>(judge, teams, _numberOfWins); 
}

public class EliminatoryTournament<T> : Tournament<T>
{
    private readonly List<IEnumerable<Team<T>>> _rankings = new();
    private IEnumerable<Team<T>>? _winners;

    public EliminatoryTournament() { }

    public EliminatoryTournament(Judge<T> judge, IEnumerable<Team<T>> teams) : base(judge, teams) {}

    public override IEnumerable<Game<T>> Games(IWinnerSelector<T> winsel) {
        if (Teams!.Count() is 2) {
            var newWinsel = winsel.NewInstance(Judge!, Teams!);
            foreach (var game in newWinsel.Games(winsel))
                yield return game;
            yield break; // Si solo hay dos equipos, no hay mas partidas por jugar
        }
        var teamsNumber = (Teams!.Count() + 1) / 2; // El número de equipos en el torneo es el doble del número de equipos en la fase de eliminatoria
        var count = 0; 
        while (count <= teamsNumber) {
            if (Teams!.Skip(count).Count() is 1) {
                _rankings.Add(Teams!.Skip(count).Take(1));
                break;
            }
            var newWinsel = winsel.NewInstance(Judge!, Teams!.Skip(count).Take(teamsNumber)); // Seleccionar los equipos que participaran en la fase de eliminatoria
            foreach (var game in newWinsel.Games(new Game<T>()))
                yield return game;
            _rankings.Add(newWinsel.Winner()); // Guardar el ranking de los equipos en la fase de eliminatoria
            count += teamsNumber; // Incrementar el contador de equipos en la fase de eliminatoria
        }
        // Seleccionar los equipos que participaran en la siguiente fase
        var nextWinsel = NewInstance(Judge!, _rankings.SelectMany(teams => teams.Take((teams.Count() + 1) / 2)))!;
        foreach (var game in nextWinsel.Games(winsel))
            yield return game;
        _winners = nextWinsel.Winner(); // Guardar los equipos ganadores de la fase
    }

    public override IWinnerSelector<T> NewInstance(Judge<T> judge, IEnumerable<Team<T>> teams)
        => new EliminatoryTournament<T>(judge, teams);

    public override IEnumerable<Team<T>> Winner() => _winners!;
}

public static class TournamentExtensors
{
    public static Tournament<TSource> Compose<TSource>(this Tournament<TSource> source, Tournament<TSource> other)
        => new TournamentComposition<TSource>(source, other);
}

internal class TournamentComposition<T> : Tournament<T>
{
    private readonly Tournament<T> _t1;
    private readonly Tournament<T> _t2;

    public TournamentComposition(Tournament<T> t1, Tournament<T> t2) {
        _t1 = t1;
        _t2 = t2;
    }

    private TournamentComposition(Judge<T> judge, IEnumerable<Team<T>> teams, 
        Tournament<T> t1, Tournament<T> t2) : base(judge, teams) {
            _t1 = t1;
            _t2 = t2;
    }

    public override IEnumerator<Game<T>> GetEnumerator() => Games(_t2).GetEnumerator();

    public override IEnumerable<Game<T>> Games(IWinnerSelector<T> winsel) 
        => _t1.SetJudge(Judge!).SetTeams(Teams!).Games(winsel);

    public override IWinnerSelector<T> NewInstance(Judge<T> judge, IEnumerable<Team<T>> teams)
        => new TournamentComposition<T>(judge!, teams!, _t1, _t2);

    public override IEnumerable<Team<T>> Winner() => _t1.Winner();
}
