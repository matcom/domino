namespace DominoEngine;

public interface IRules<T> : IGenerator<T>, IDealer<T>, IMatcher<T>, ITurner<T>, IFinisher<T>, IScorer<T> { }

public interface IGenerator<T>
{
    public List<Ficha<T>> Generate();
}

public interface IDealer<T>
{
    public void Dealing(IList<Ficha<T>> fichas);
}

public interface IMatcher<T>
{
    public bool CanMatch(Move<T> move);
}

public interface ITurner<T>
{
    public IEnumerable<Player<T>> NextTurn();
}

public interface IFinisher<T>
{
    public bool GameOver();
}

public interface IScorer<T>
{
    public double Scorer(Move<T> move);
}

public interface ICloneable<T> : ICloneable
{
    public new T Clone();
}