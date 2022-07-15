using DominoEngine;

namespace Rules;

public class ClassicGenerator : IGenerator<int>
{
    public ClassicGenerator() {}

    IEnumerable<Token<int>> IGenerator<int>.Generate() {
        for (var i = 0; ; i++)
            for (var j = 0; j <= i; j++)
                yield return new Token<int>(i, j);
    }

    public override string ToString()
        => "Genera todas las fichas de forma clasica";
}

public class NoDoubleGenerator : IGenerator<int>
{
    IEnumerable<Token<int>> IGenerator<int>.Generate() {
        for (var i = 0; ; i++)
            for (var j = 0; j < i; j++)
                yield return new Token<int>(i, j);
    }

    public override string ToString()
        => "Genera todas las fichas de forma clasica sin contar dobles";
}

public class SumPrimeGenerator : IGenerator<int>
{
    IEnumerable<Token<int>> IGenerator<int>.Generate() {
        var tokens = new List<Token<int>>();

        for (var i = 0; ; i++)
            for (var j = 0; j <= i; j++)
                if (IsPrime(i + j)) yield return new Token<int>(i, j);
    }

    private static bool IsPrime(int a) {
        for (var i = 2; i <= Math.Sqrt(a); i++) {
            if (a % i is 0) return false;
        }
        return true;
    }

    public override string ToString()
        => "Genera todas las fichas cuya suma sea un numero primo";
}

public static class GeneratorsExtensors
{
    public static IGenerator<TSource> Join<TSource>(this IGenerator<TSource> g1, IGenerator<TSource> g2)
        => new JoinGenerator<TSource>(g1, g2);

    public static IGenerator<TSource> Intersect<TSource>(this IGenerator<TSource> g1, IGenerator<TSource> g2)
        => new IntersectGenerator<TSource>(g1, g2);
}

internal class IntersectGenerator<T> : IGenerator<T>
{
    private IGenerator<T> _generator1;
    private IGenerator<T> _generator2;
    private HashSet<Token<T>> _hashSet1 = new();
    private HashSet<Token<T>> _hashSet2 = new();

    public IntersectGenerator(IGenerator<T> generator1, IGenerator<T> generator2) {
        _generator1 = generator1;
        _generator2 = generator2;
    }

    public IEnumerable<Token<T>> Generate() {
        var enum1 = _generator1.Generate().GetEnumerator();
        var enum2 = _generator2.Generate().GetEnumerator();
        while (enum1.MoveNext() && enum2.MoveNext()) {
            if (Equals(enum1.Current, enum2.Current))
                yield return enum1.Current;
            else {
                for (int i = 0; i < 2; i++) {
                    if (_hashSet2.Contains(enum1.Current))
                        yield return enum1.Current;
                    else _hashSet1.Add(enum1.Current);
                    (enum1, enum2) = (enum2, enum1);
                    (_hashSet1, _hashSet2) = (_hashSet2, _hashSet1);
                }
            }
        }
    }

    public override string ToString()
        => $@"Interseccion:
    {_generator1.ToString()!.Replace("\n","\n\t")}
	{_generator2.ToString()!.Replace("\n","\n\t")}";
}

internal class JoinGenerator<T> : IGenerator<T>
{
    private IGenerator<T> _generator1;
    private IGenerator<T> _generator2;
    private HashSet<Token<T>> _hashSet1 = new();
    private HashSet<Token<T>> _hashSet2 = new();

    public JoinGenerator(IGenerator<T> generator1, IGenerator<T> generator2) {
        _generator1 = generator1;
        _generator2 = generator2;
    }

    public IEnumerable<Token<T>> Generate() {
        var enum1 = _generator1.Generate().GetEnumerator();
        var enum2 = _generator2.Generate().GetEnumerator();
        while (enum1.MoveNext() && enum2.MoveNext()) {
            for (int i = 0; i < 2; i++) {
                if (!_hashSet2.Contains(enum1.Current)) {
                    _hashSet1.Add(enum1.Current);
                    yield return enum1.Current;
                }
                (enum1, enum2) = (enum2, enum1);
                (_hashSet1, _hashSet2) = (_hashSet2, _hashSet1);
            }
        }
    }

    public override string ToString()
        => $@"Union:
    {_generator1.ToString()!.Replace("\n","\n\t")}
	{_generator2.ToString()!.Replace("\n","\n\t")}";
}
