namespace DominoEngine;

public class Token<T> {
    private T _head;
    private T _tail;

    public T Tail { get => _tail; }

    public T Head { get => _head; }

    public Token(T head, T tail) {
		_tail = tail;
		_head = head;
	}

	public override bool Equals(object? obj) => obj is Token<T> obj1 && Equals(obj1);

	public override int GetHashCode() {
		return HashCode.Combine(Head) * HashCode.Combine(Tail);
	}

	public bool Equals(Token<T> token) =>
		(Equals(Head, token.Head) && Equals(Tail, token.Tail)) ||
		(Equals(Head, token.Tail) && Equals(Tail, token.Head)); 

	public void Deconstruct(out T head, out T tail) {
		head = Head;
		tail = Tail;
	}

    public override string ToString() => $"({Head}|{Tail})";
}