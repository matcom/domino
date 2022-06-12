namespace DominoEngine;

public interface IGenerator<T>
{
    public List<Ficha<T>> Generate();
}

public interface IDealer<T>
{
    public void Dealing(int piecesForPlayers, IEnumerable<Hand<T>> hands, IList<Ficha<T>> fichas);
}

public interface IMatcher<T>
{
    void SetPartida(Partida<T> partida);
    public bool CanMatch(IMove<T> move);
}

public interface ITurner<T>
{
    void SetPartida(Partida<T> partida);
    public IEnumerable<Player<T>> NextTurn();
}

public interface IFinisher<T>
{
    void SetPartida(Partida<T> partida);
    public bool GameOver();
}

public interface IScorer<T>
{
    void SetPartida(Partida<T> partida);
    public double Scorer(Move<T> move);
}

public interface ICloneable<T> : ICloneable
{
    public new T Clone();
}

public interface IMove<T> { }