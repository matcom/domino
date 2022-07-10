namespace DominoEngine;

public readonly struct Token<T> {
	public readonly T Head;
	public readonly T Tail;

	public Token(T head, T tail) {
		Tail = tail;
		Head = head;
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