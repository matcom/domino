namespace DominoEngine;

public interface IGenerator<T> {
	// Interfaz que se encarga de generar todas las fichas que estaran en juego
	public IEnumerable<Ficha<T>> Generate();
}

public interface IDealer<T> {
	// Interfaz que se encarga de repartir las fichas a los jugadores
	public Dictionary<Player<T>, Hand<T>> Deal(Partida<T> partida, IEnumerable<Ficha<T>> fichas);
}

public interface IMatcher<T> {
	// Interfaz que se encarga de decidir si dos fichas matchean o no
	public IEnumerable<Move<T>> CanMatch(Partida<T> partida, IEnumerable<Move<T>> enumerable);
	public bool CanMatch(Partida<T> partida, Move<T> move);
}

public interface ITurner<T> {
	// Interfaz que se encarga de iterar por los jugadores en algun orden
	public IEnumerable<Player<T>> Players(Partida<T> partida);
}

public interface IFinisher<T> {
	// Interfaz que contiene las condiciones de finalizacion de una partida
	public bool GameOver(Partida<T> partida);
}

public interface IScorer<T> {
	// Interfaz que se encarga de puntuar un movimiento en un momento dado de la partida
	public double Scorer(Partida<T> partida, Move<T> move);
}