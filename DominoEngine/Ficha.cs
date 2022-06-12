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

public class Move<T> : IMove<T>
{
    T _head;
    T _tail;
    int _playerId;

    public Move(T Head, T Tail, int PlayerId)
    {
        _head = Head;
        _tail = Tail;
        _playerId = PlayerId;
    }

    public T Head => _head;
    public T Tail => _tail;
    public int PlayerId => _playerId;
}

public class BaseMove<T> : Move<T>
{
    int _turn;

    public BaseMove(T Head, T Tail, int PlayerId, int Turn) : base(Head, Tail, PlayerId) 
    {
        _turn = Turn;
    }

    public int Turn => _turn;
}

public record Check<T>(int PlayerId) : IMove<T>;

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