using System.Collections;

namespace DominoEngine;

public class Board<T>: IReadOnlyList<Move<T>> {
	List<Move<T>>? _moves; 

	public IEnumerator<Move<T>> GetEnumerator() => _moves!.GetEnumerator();

	IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

	public int Count => _moves!.Count;

	public Move<T> this[int index] => _moves![index];

	public void Add(Move<T> item) => _moves!.Add(item);
}