namespace DominoEngine;

public struct Ficha<T> {
	public readonly T Head;
	public readonly T Tail;

	public Ficha(T head, T tail) {
		Tail = tail;
		Head = head;
	}

	public override bool Equals(object? obj) => obj is Ficha<T> obj1 && Equals(obj1);

	public override int GetHashCode() {
		return HashCode.Combine(Head) * HashCode.Combine(Tail);
	}

	public bool Equals(Ficha<T> ficha) =>
		(Equals(Head, ficha.Head) && Equals(Tail, ficha.Tail)) ||
		(Equals(Head, ficha.Tail) && Equals(Tail, ficha.Head));

	public void Deconstruct(out T head, out T tail) {
		head = Head;
		tail = Tail;
	}

    public override string ToString()
    {
        return $"({Head}|{Tail})";
    }
}