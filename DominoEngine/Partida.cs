namespace DominoEngine;

public class Partida<T>
{
    private Board<T> _board;
    private Dictionary<Player<T>, Hand<T>> _hands;

    public Partida(Dictionary<Player<T>, Hand<T>> hands)
    {
        _board = new Board<T>();
        _hands = hands;
    }

    public Board<T> Board => _board;
    public Dictionary<Player<T>, Hand<T>> Hands => _hands;

    public IEnumerable<Player<T>> Players()
    {
        return Hands.Keys;
    }

    public List<Move<T>> PlayersMoves(Player<T> player)
    {
        throw new NotImplementedException();
    }
}