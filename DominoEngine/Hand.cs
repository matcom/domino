using System.Collections;
using System.Runtime.ExceptionServices;

namespace DominoEngine; 

public class Hand<T>: ICollection<Ficha<T>> {
	private readonly List<Ficha<T>> _hand;

	public Hand() {
		_hand = new List<Ficha<T>>();
	}

	private Hand(IEnumerable<Ficha<T>> hand) {
		_hand = hand.ToList();
	}
	public IEnumerator<Ficha<T>> GetEnumerator() => _hand.GetEnumerator();

	IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

	public void Add(Ficha<T> item) => _hand.Add(item);

	public void Clear() => _hand.Clear();

	public bool Contains(Ficha<T> item) => _hand.Contains(item);

	public void CopyTo(Ficha<T>[] array, int arrayIndex) => _hand.CopyTo(array,arrayIndex);

	public bool Remove(Ficha<T> item){
		foreach (var ficha in _hand)
			if (item.Equals(ficha))
				return _hand.Remove(ficha);
		return false;
	}

	public int Count => _hand.Count;
	public bool IsReadOnly => false;
	public Hand<T> Clone() => new(this);

    public override string ToString()
    {
		string result = "";
        foreach (var ficha in _hand)
			result += " " + ficha.ToString() + ",";
		return (this.Count == 0)? "" : result.Substring(1, result.Length-2);
    }
}