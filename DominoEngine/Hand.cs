using System.Collections;
using System.Runtime.ExceptionServices;

namespace DominoEngine; 

public class Hand<T>: ICollection<Token<T>> {
	private readonly List<Token<T>> _hand;

	public Hand() {
		_hand = new List<Token<T>>();
	}

	private Hand(IEnumerable<Token<T>> hand) {
		_hand = hand.ToList();
	}
	public IEnumerator<Token<T>> GetEnumerator() => _hand.GetEnumerator();

	IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

	public void Add(Token<T> item) => _hand.Add(item);

	public void Clear() => _hand.Clear();

	public bool Contains(Token<T> item) => _hand.Contains(item);

	public void CopyTo(Token<T>[] array, int arrayIndex) => _hand.CopyTo(array,arrayIndex);

	public bool Remove(Token<T> item){
		foreach (var token in _hand)
			if (item.Equals(token))
				return _hand.Remove(token);
		return false;
	}

	public int Count => _hand.Count;
	public bool IsReadOnly => false;
	public Hand<T> Clone() => new(this);

    public override string ToString()
    {
		string result = "";
        foreach (var token in _hand)
			result += " " + token.ToString() + ",";
		return (this.Count == 0)? "" : result.Substring(1, result.Length-2);
    }
}