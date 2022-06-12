using System.Collections;

namespace DominoEngine;


public class Ficha<T>
{
    public T Head;
    public T Tail;

    public Ficha(T head, T tail)
    {
        Head = head;
        Tail = tail;
    }

    public bool Equals(Ficha<T> ficha)
    {
        return (Head!.Equals(ficha.Head) && Tail!.Equals(ficha.Tail)) || (Head!.Equals(ficha.Tail) && Tail!.Equals(ficha.Head));
    }

    public T Other(T other)
    {
        return (other!.Equals(Head)) ? Tail : Head;
    }
}

public record Move<T>(T Head, T Tail, int PlayerId, bool Check = false);

public record BaseMove<T>(T Head, T Tail, int PlayerId, int Turn, bool Check = false) : Move<T>(Head, Tail, PlayerId, Check);

public record Check<T>(T Head, T Tail, int PlayerId, bool Check = true) : Move<T>(Head, Tail, PlayerId, Check);

public class Hand<T> : ICollection<Ficha<T>>, ICloneable<Hand<T>>
{
    List<Ficha<T>> fichas;
    public Hand(List<Ficha<T>> Fichas = null!)
    {
        fichas = (Fichas is null) ? new List<Ficha<T>>() : Fichas;
    }

    public int Count => fichas.Count;

    public bool IsReadOnly => false;

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

    public bool Remove(Ficha<T> item) => fichas.Remove(item);

    object ICloneable.Clone()
    {
        return Clone();
    }
}