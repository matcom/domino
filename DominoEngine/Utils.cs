using System.Collections;

namespace DominoEngine;

public static class Utils
{
    public static IEnumerable<(int, TSource)> Enumerate<TSource>(this IEnumerable<TSource> source) => source.Select((t, i) => (index: i, item: t));

    public static IEnumerable<TSource> Complement<TSource>(this IEnumerable<TSource> source, IEnumerable<TSource> another)
        => source.Where(x => !another.Contains(x));

    public static bool IsEmpty<TSource>(this IEnumerable<TSource> source) => (source.Take(1).Count() is 0);

    public static void Make<TSource>(this IEnumerable<TSource> source, Action<TSource> function)
    {
        foreach (var item in source)
            function(item);
    }
}

public class InfiniteEnumerator<T> : IEnumerator<T>
{
	private IEnumerable<T> _enumerable;
	private int _current = -1;

    public InfiniteEnumerator(IEnumerable<T> enumerable)
    {
		_enumerable = enumerable;
    }

    public T Current => _enumerable.ElementAt(_current);

    object IEnumerator.Current => Current!;

    public void Dispose() => throw new NotImplementedException();

    public bool MoveNext() {
		_current = (_current++) % _enumerable.Count();
		return true;
	}

    public void Reset() => _current = -1;
}

public class ReversibleInfiniteEnumerator<T> : IEnumerator<T>
{
    private bool _direction = true;
    IEnumerable<T> _items;
    private int _current = -1;

    public ReversibleInfiniteEnumerator(IEnumerable<T> items)
    {
        _items = items;
    }

    public T Current {
        get
        {
            if (_current == _items.Count())
                _current = 0;
            else if (_current == -1)
                _current = _items.Count() - 1;
            return _items.ElementAt(_current);
        }
    }

    public void Direction() => _direction = !_direction;

    object IEnumerator.Current => Current!;

    public void Dispose()
    {
        throw new NotImplementedException();
    }

    public bool MoveNext() {
        _current += _direction ? 1 : -1;
        return true;
    }

    public void Reset() => _current = -1;
}