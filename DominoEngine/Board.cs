using System.Collections;

namespace DominoEngine;

public class Node<T>
{
    private T _toParent;
    private T _toChild;
    private int _turn;
    private Node<T>? _parent;
    public Node<T>? Child = null;

    public Node(T toParent, T toChild, int turn, Node<T>? parent)
    {
        _toParent = toParent;
        _toChild = toChild;
        _turn = turn;
        _parent = parent;
    }

    public T ToParent => _toParent;
    public T ToChild => _toChild;
    public int Turn => _turn;

    public Node<T>? Parent { get => _parent; set => _parent = value; }
}

public record Ficha<T>(T Head, T Tail);

public class StartNode<T> : Node<T>
{
    public StartNode(T toParent, T toChild) : base(toParent, toChild, 0, null) { }
}

public class Board<T> : IEnumerable<(int turn, bool right, T toParent, T toChild)>
{

    public StartNode<T> Salida; //rename	
    private Node<T> _left;
    private Node<T> _right;

    public Node<T> Left => _left;
    public Node<T> Right => _right;

    public Board(Ficha<T> salida)
    { //rename
        Salida = new StartNode<T>(salida.Head, salida.Tail);
        _left = Salida;
        _right = Salida;
    }

    public bool AddLeft(T matching, T other, int turn)
    {
        var inNode = new Node<T>(matching, other, turn, Left);
        if (Left.Equals(Salida))
        {
            Left.Parent = inNode;
            _left = Left.Parent;
            return true;
        }

        Left.Child = inNode;
        _left = Left.Child;
        return true;
    }

    public bool AddRight(T matching, T other, int turn)
    {
        Right.Child = new Node<T>(matching, other, turn, Right);
        _right = Right.Child;
        return true;
    }

    public IEnumerator<(int turn, bool right, T toParent, T toChild)> GetEnumerator()
    {
        return Photo().GetEnumerator();
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return GetEnumerator();
    }

    private IEnumerable<(int turn, bool right, T toParent, T toChild)> Photo()
    {
        yield return (0, true, Salida.ToParent, Salida.ToChild);
        var curLeft = Salida.Parent;
        var curRight = Salida.Child;
        while (!(curLeft is null && curRight is null))
        {
            if (curLeft is null)
            {
                var turn = curRight!.Turn;
                var toParent = curRight.ToParent;
                var toChild = curRight.ToChild;
                curRight = curRight.Child;
                yield return (turn, true, toParent, toChild);
            }
            else if (curRight is null)
            {
                var turn = curLeft!.Turn;
                var toParent = curLeft.ToParent;
                var toChild = curLeft.ToChild;
                curLeft = curLeft.Child;
                yield return (turn, false, toParent, toChild);
            }
            else if (curLeft.Turn < curRight.Turn)
            {
                var turn = curLeft!.Turn;
                var toParent = curLeft.ToParent;
                var toChild = curLeft.ToChild;
                curLeft = curLeft.Child;
                yield return (turn, false, toParent, toChild);
            }
            else
            {
                var turn = curRight!.Turn;
                var toParent = curRight.ToParent;
                var toChild = curRight.ToChild;
                curRight = curRight.Child;
                yield return (turn, true, toParent, toChild);
            }
        }
    }
}

public record Move<T>(T Head, Ficha<T> ficha, bool rigth, Board<T> board);

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

public abstract class Player<T>
{

}