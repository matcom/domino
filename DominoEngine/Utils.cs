using System.Collections;

namespace DominoEngine;

public static class Utils
{
    public static IEnumerable<(int, TSource)> Enumerate<TSource>(this IEnumerable<TSource> source) 
        => source.Select((t, i) => (index: i, item: t));

    public static IEnumerable<TSource> Complement<TSource>(this IEnumerable<TSource> source, IEnumerable<TSource> another)
        => source.Where(x => !another.Contains(x));

    public static bool IsEmpty<TSource>(this IEnumerable<TSource> source) => (source.Take(1).Count() is 0);

    public static void Make<TSource>(this IEnumerable<TSource> source, Action<TSource> function) {
        foreach (var item in source)
            function(item);
    }

    public static IEnumerable<TSource> Infinty<TSource>(this IEnumerable<TSource> source) {
        var enumerator = new InfiniteEnumerator<TSource>(source);
        while (enumerator.MoveNext())
            yield return enumerator.Current;
    }

    public static IEnumerable<TSource> OneByOne<TSource>(this IEnumerable<IEnumerable<TSource>> source) {
        var enumerator = new OneByOneEnumerator<TSource>(source);
        while(enumerator.MoveNext())
            yield return enumerator.Current;
    }
}

class OneByOneEnumerator<T> : IEnumerator<T>
{
    int _current = -1;
    List<IEnumerator<T>> _list = new();
    public OneByOneEnumerator(IEnumerable<IEnumerable<T>> enumerable) {
        int max = MCM(enumerable.Select(x => x.Count()));
        enumerable.Make(x => _list.Add(x.Take(max).GetEnumerator()));
    }

    public T Current => _list[_current].Current;

    object IEnumerator.Current => Current!;

    public void Dispose() {}

    public bool MoveNext() {
        _current  = (_current+1) % _list.Count();
        return _list[_current].MoveNext();
    }

    public void Reset() => _current = -1;

    int MCM(IEnumerable<int> items) {
        int mcm = 0;
        for (int i = 1; i < items.Count(); i++) {
            mcm = MCM(items.ElementAt(i), items.ElementAt(i-1));
        }
        return mcm;
    }

    int MCM(int a, int b) => (a * b) / MCD(a, b);

    int MCD(int a, int b) {
        int mcd = b;
        while (a % b != 0)
            (a, b) = (b, a % b);
        return b;
    }
}

class InfiniteEnumerator<T> : IEnumerator<T>
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
		_current = (_current+1) % _enumerable.Count();
		return true;
	}

    public void Reset() => _current = -1;
}
