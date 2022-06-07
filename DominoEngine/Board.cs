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
        _left = new Node<T>(Salida.ToParent, Salida.ToChild, 0, Salida);
        _right = new Node<T>(Salida.ToChild, Salida.ToParent, 0, Salida);
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

public abstract class Player<T>
{
    private Hand<T> _hand;

    public Player(Hand<T> hand)
    {
        _hand = hand;
    }

    public Hand<T> Hand { get => _hand; }

    public abstract Move<T> Play(IList<Move<T>> moves);
    public abstract Ficha<T> Play();
}

public class RandomPlayer<T> : Player<T>
{
    public RandomPlayer(Hand<T> hand) : base(hand) {}
    public override Move<T> Play(IList<Move<T>> moves)
    {
        Random r = new Random();
        return moves[r.Next(moves.Count)];
    }

    public override Ficha<T> Play()
    {
        Random r = new Random();
        return Hand[r.Next(Hand.Count)];
    }
}
