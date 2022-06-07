namespace DominoEngine;

public interface IGenerator<T>
{
    public List<Ficha<T>> Generate(int n);
}

public interface IDealer<T>
{
    public void Dealing(int piecesForPlayers, IEnumerable<Hand<T>> hands, IList<Ficha<T>> fichas);
}

public interface IMatcher<T>
{
    public bool CanMatch(T toParent, bool rigth);
}

public interface ITurner<T>
{
    public Player<T> NextTurn();
}

public interface IFinisher<T>
{
    public bool IsEnd();
}

public interface IScorer<T>
{
    public double Scorer();
}

public interface ICloneable<T> : ICloneable
{
    public new T Clone();
}