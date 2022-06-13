namespace DominoEngine;

public record Move<T>(int PlayerId, bool Check = true, int Turn = -2, T? Head = default, T? Tail = default) {
	public Ficha<T?> Ficha => new Ficha<T?>(Head, Tail);
}