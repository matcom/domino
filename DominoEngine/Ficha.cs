using System.Collections;

namespace DominoEngine;


public record Ficha<T>(T Head, T Tail)
{
    public T Other(T other)
    {
        return (other!.Equals(Head))? Tail : Head;
    }
}

public record Move<T>(T Head, Ficha<T> Ficha, bool Rigth) : IMove<T>
{
    public override string ToString()
    {
        if (Rigth) return $"{Ficha.Head!.ToString()} | {Ficha.Tail!.ToString()} for rigth";
        else return $"{Ficha.Head!.ToString()} | {Ficha.Tail!.ToString()} for left";
    }
}

public record Salida<T>(T Head, T Tail) : IMove<T>
{
    public override string ToString()
    {
        return $"{Head!.ToString()} | {Tail!.ToString()} salida";
    }
}

public record Check<T> : IMove<T>
{
    public override string ToString()
    {
        return "pass";
    }
}

public class Hand<T> : IList<Ficha<T>>, ICloneable<Hand<T>>
{
    List<Ficha<T>> fichas;
    public Hand(List<Ficha<T>> Fichas = null!)
    {
        fichas = (Fichas is null) ? new List<Ficha<T>>() : Fichas;
    }

    public Ficha<T> this[int index] { get => fichas[index]; set => fichas[index] = value; }

    public int Count => fichas.Count;

    public bool IsReadOnly => throw new NotImplementedException();

    public void Add(Ficha<T> item) => fichas.Add(item);

    public void Clear() => fichas.Clear();

    public Hand<T> Clone()
    {
        List<Ficha<T>> new_list = new List<Ficha<T>>();

        foreach (var item in fichas) new_list.Add(item);

        return new Hand<T>(new_list);
    }

    public bool Contains(Ficha<T> item) => fichas.Contains(item);

    public void CopyTo(Ficha<T>[] array, int arrayIndex)
    {
        throw new NotImplementedException();
    }

    public IEnumerator<Ficha<T>> GetEnumerator()
    {
        return fichas.GetEnumerator();
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return GetEnumerator();
    }

    public int IndexOf(Ficha<T> item) => fichas.IndexOf(item);

    public void Insert(int index, Ficha<T> item)
    {
        throw new NotImplementedException();
    }

    public bool Remove(Ficha<T> item) => fichas.Remove(item);

    public void RemoveAt(int index) => fichas.RemoveAt(index);

    object ICloneable.Clone()
    {
        throw new NotImplementedException();
    }
}