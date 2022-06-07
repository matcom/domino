namespace DominoEngine;

public class Ruler<T> {
	private Board<T>? _board;
	private Dictionary<Player<T>, Hand<T>> _hands = new Dictionary<Player<T>, Hand<T>>();
	private List<bool>? _turns;

    public Ruler()
    {
    }
}