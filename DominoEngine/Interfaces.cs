namespace DominoEngine;

public interface IGenerator<T>
{
    // Interfaz que se encarga de generar todas los tokens que estaran en juego
    public IEnumerable<Token<T>> Generate();
}

public interface IDealer<T>
{
    // Interfaz que se encarga de repartir los tokens a los jugadores
    public Dictionary<Player<T>, Hand<T>> Deal(Partida<T> partida, IEnumerable<Token<T>> tokens);
}

public interface IMatcher<T>
{
    // Interfaz que se encarga de decidir si dos tokens matchean o no
    public IEnumerable<Move<T>> CanMatch(Partida<T> partida, IEnumerable<Move<T>> enumerable,
            Func<Token<T>, double> token_scorer);
}

public interface ITurner<T>
{
    // Interfaz que se encarga de iterar por los jugadores en algun orden
    public IEnumerable<Player<T>> Players(Partida<T> partida);
}

public interface IFinisher<T>
{
    // Interfaz que contiene las condiciones de finalizacion de una partida
    public bool GameOver(Partida<T> partida);
}

public interface IScorer<T>
{
    // Interfaz que se encarga de puntuar un movimiento en un momento dado de la partida
    public double Scorer(Partida<T> partida, Move<T> move);

    public double TokenScorer(Token<T> token);

    public IEnumerable<Team<T>> Winner(Partida<T> partida);
}

public interface IWinnerSelector<T>
{
    public IEnumerable<Team<T>> Winner();

    public IWinnerSelector<T> NewInstance(Judge<T> judge, IEnumerable<Team<T>> teams);

    public IEnumerable<Game<T>> Games(IWinnerSelector<T> winsel);
}