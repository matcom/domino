namespace DominoEngine;

public interface IGenerator<T> {
	public IEnumerable<Ficha<T>> Generate();
}

public interface IDealer<T> {
	public void Deal(Partida<T> partida, IEnumerable<Ficha<T>> fichas);
}

public interface IMatcher<T> {
	public bool CanMatch(Partida<T> partida, Move<T> move);
}

public interface ITurner<T> {
	public IEnumerable<Player<T>> Players(Partida<T> partida);
}

public interface IFinisher<T> {
	public bool GameOver(Partida<T> partida);
}

public interface IScorer<T> {
	public double Scorer(Partida<T> partida, Move<T> move);
}