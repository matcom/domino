using System.Collections;

namespace DominoEngine;

public static class Utils
{
    public static IEnumerable<(int, TSource)> Enumerate<TSource>(this IEnumerable<TSource> source) 
        => source.Select((t, i) => (index: i, item: t)); // Devuelve una tupla con el indice y el elemento
        

    public static IEnumerable<TSource> Complement<TSource>(this IEnumerable<TSource> source, IEnumerable<TSource> another)
        => source.Where(x => !another.Contains(x)); // Complemento de uno con respecto a otro

    // Devuelve true si un IEnumerable no tiene elementos
    public static bool IsEmpty<TSource>(this IEnumerable<TSource> source) => (source.Take(1).Count() is 0);

    public static void Make<TSource>(this IEnumerable<TSource> source, Action<TSource> function) {
        foreach (var item in source)
            function(item); // Ejecuta la funcion sobre cada elemento
    }

    // Dados dos IEnumerables con los mismos elementos, construye un tercero reorganizandolos, 
    // de forma tal que se prioricen los elementos que entre ambos IEnumerables tengan menor indice
    public static IEnumerable<TSource> Average<TSource>(this IEnumerable<TSource> source, 
        IEnumerable<TSource> other, double value) where TSource : notnull  {
            Dictionary<TSource, double> record = new(); 
            source.Enumerate().Make(item => record.Add(item.Item2, item.Item1)); 
            other.Select((x,i) => (i * value, x)).Make(item => record[item.x] += item.Item1); 
            return record.Keys.OrderBy(key => record[key]);
        }

    // Dado un IEnumerable, devuele un ciclo infinito de los mismos elementos
    public static IEnumerable<TSource> Infinity<TSource>(this IEnumerable<TSource> source) {
        var enumerator = new InfiniteEnumerator<TSource>(source);
        while (enumerator.MoveNext())
            yield return enumerator.Current;
    }

    // Dado un IEnumerable de IEnumerables, devuelve otro tomando uno a uno cada
    // elemento de cada IEnumerable
    public static IEnumerable<TSource> OneByOne<TSource>(this IEnumerable<IEnumerable<TSource>> source) {
        var enumerator = new OneByOneEnumerator<TSource>(source);
        while(enumerator.MoveNext())
            yield return enumerator.Current;
    }
}

internal class OneByOneEnumerator<T> : IEnumerator<T>
{
    private int _current = -1;
    private readonly List<IEnumerator<T>> _list = new();
    public OneByOneEnumerator(IEnumerable<IEnumerable<T>> enumerable) {
        // Tomar el mcm entre cantidad de elementos para que quede bien formateado
        var max = Mcm(enumerable.Select(x => x.Count()));                       
        enumerable.Make(x => _list.Add(x.Infinity().Take(max).GetEnumerator()));
    }

    public T Current => _list[_current].Current;

    object IEnumerator.Current => Current!;

    public void Dispose() {}

    // No dara false hasta que no se complete el ciclo y vuelva al inicio
    public bool MoveNext() {
        _current  = (_current+1) % _list.Count();
        return _list[_current].MoveNext();
    }

    public void Reset() => _current = -1;

    private int Mcm(IEnumerable<int> items) {
        var mcm = 0;
        for (var i = 1; i < items.Count(); i++) {
            mcm = Mcm(items.ElementAt(i), items.ElementAt(i-1));
        }
        return mcm;
    }

    private int Mcm(int a, int b) => (a * b) / Mcd(a, b);

    private static int Mcd(int a, int b) {
        if (a is 0 || b is 0) return 0;
        return (a % b is 0)? b : Mcd(b, a % b);
    }
}

internal class InfiniteEnumerator<T> : IEnumerator<T>
{
	private readonly IEnumerable<T> _enumerable;
	private int _current = -1;

    public InfiniteEnumerator(IEnumerable<T> enumerable) {
		_enumerable = enumerable;
    }

    public T Current => _enumerable.ElementAt(_current);

    object IEnumerator.Current => Current!;

    public void Dispose() {}

    public bool MoveNext() {
		_current = (_current+1) % _enumerable.Count();
		return true;
	}

    public void Reset() => _current = -1;
}
