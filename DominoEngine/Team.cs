using System.Collections;

namespace DominoEngine;

public class Team<T> : IReadOnlyList<Player<T>>
{
    private readonly List<Player<T>> _team;

    public Team(List<Player<T>> team) {
        _team = team;
    }

    public Player<T> this[int index] => _team[index];

    public int Count => _team.Count;

    public IEnumerator<Player<T>> GetEnumerator() => _team.GetEnumerator();

    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
}