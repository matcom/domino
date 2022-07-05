using System.Collections;

namespace DominoEngine;

public abstract class Tournament<T> : IEnumerable<Game<T>>, IWinnerSelector<T>
{
    protected Judge<T>? _judge;
    protected IEnumerable<Team<T>>? _teams;

    protected Tournament(Judge<T> judge, IEnumerable<Team<T>> teams) {
        _judge = judge;
        _teams = teams;
    }

    protected Tournament() { }

    public Tournament<T> SetJudge(Judge<T> judge) {
        _judge =  judge;
        return this;
    }

    public Tournament<T> SetTeams(IEnumerable<Team<T>> teams) {
        _teams = teams;
        return this;
    }

    public abstract IEnumerable<Game<T>> Games(IWinnerSelector<T> winsel);

    public abstract Team<T> Winner();

    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

    public virtual IEnumerator<Game<T>> GetEnumerator() => Games(new Game<T>()).GetEnumerator();

    public abstract IWinnerSelector<T> NewInstance(Judge<T> judge, IEnumerable<Team<T>> teams);
}

public class AllVsAllTournament<T> : Tournament<T>
{
    Dictionary<Team<T>, int> _games = new();

    public AllVsAllTournament() { }

    public AllVsAllTournament(Judge<T> judge, IEnumerable<Team<T>> teams) : base(judge, teams) { }

    public override IEnumerable<Game<T>> Games(IWinnerSelector<T> winsel) {
        _teams!.Make(team => _games.Add(team, 0));
        foreach (var (i,team_1) in _teams!.Enumerate())
            foreach (var team_2 in _teams!.Skip(i+1)) {
                var new_winsel = winsel.NewInstance(_judge!, new List<Team<T>>{team_1, team_2});
                // var game = new Game<T>(_judge!, new List<Team<T>>{team_1, team_2});
                foreach (var game in new_winsel.Games(new Game<T>())) 
                    yield return game;
                _games[new_winsel.Winner()] += 3;
            }
    }

    public override IWinnerSelector<T> NewInstance(Judge<T> judge, IEnumerable<Team<T>> teams)
        => new AllVsAllTournament<T>(judge, teams);

    public override Team<T> Winner() => _games.Keys.MaxBy(team => _games[team])!;
}

public class DirichletTournament<T> : Tournament<T>
{
    Dictionary<Team<T>, List<IWinnerSelector<T>>> _games = new(); 
    int _numberOfWins;

    public DirichletTournament(int number) {
        _numberOfWins = number;
    }

    public DirichletTournament(Judge<T> judge, IEnumerable<Team<T>> teams, int number) : base(judge, teams) {
        _numberOfWins = number;
    }

    public override IEnumerable<Game<T>> Games(IWinnerSelector<T> winsel) {
        while (EndCondition()) {
            var new_winsel = winsel.NewInstance(_judge!, _teams!);
            foreach (var game in new_winsel.Games(new Game<T>()))
                yield return game;
            // var game = new Game<T>(_judge!, _teams!);
            // yield return game;
            // var team = game.Winner();
            var team = new_winsel.Winner();
            if (!_games.ContainsKey(team)) _games.Add(team, new List<IWinnerSelector<T>>(){new_winsel});
            else _games[team].Add(new_winsel);
        }
    }

    private bool EndCondition() => _games.All(pair => pair.Value.Count() < _numberOfWins);

    public override Team<T> Winner() => _games.Keys.MaxBy(team => _games[team].Count())!;

    public override IWinnerSelector<T> NewInstance(Judge<T> judge, IEnumerable<Team<T>> teams) 
        => new DirichletTournament<T>(judge, teams, _numberOfWins);
}

public static class TournamentExtensors
{
    public static Tournament<TSource> Compose<TSource>(this Tournament<TSource> source, Tournament<TSource> other)
        => new TournamentComposition<TSource>(source, other);
}

class TournamentComposition<T> : Tournament<T>
{
    Tournament<T> _t1;
    Tournament<T> _t2;

    public TournamentComposition(Tournament<T> t1, Tournament<T> t2) {
        _t1 = t1;
        _t2 = t2;
    }

    public TournamentComposition(Judge<T> judge, IEnumerable<Team<T>> teams, 
        Tournament<T> t1, Tournament<T> t2) : base(judge, teams) {
            _t1 = t1;
            _t2 = t2;
    }

    public override IEnumerator<Game<T>> GetEnumerator() => Games(_t2).GetEnumerator();

    public override IEnumerable<Game<T>> Games(IWinnerSelector<T> winsel) 
        => _t1.SetJudge(_judge!).SetTeams(_teams!).Games(winsel);

    public override IWinnerSelector<T> NewInstance(Judge<T> judge, IEnumerable<Team<T>> teams)
        => new TournamentComposition<T>(_judge!, _teams!, _t1, _t2);

    public override Team<T> Winner() => _t1.Winner();
}
