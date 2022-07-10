namespace DominoEngine;

// Interfaz que se encarga de generar todas los tokens que estaran en juego
public interface IGenerator<T> {
    public IEnumerable<Token<T>> Generate();
}

// Interfaz que se encarga de repartir los tokens a los jugadores
public interface IDealer<T> {
    public Dictionary<Player<T>, Hand<T>> Deal(Partida<T> partida, IEnumerable<Token<T>> tokens);
}

// Interfaz que se encarga de decidir si dos tokens matchean o no
public interface IMatcher<T> {
    // Filtro de jugadas validas
    public IEnumerable<Move<T>> CanMatch(Partida<T> partida, IEnumerable<Move<T>> enumerable,
            Func<Token<T>, double> tokenScorer);

    // Devuelve los turnos validos dados cierta partida
    public IEnumerable<int> ValidsTurns(Partida<T> partida, int player);
}

// Interfaz que se encarga de iterar por los jugadores en algun orden
public interface ITurner<T> {
    public IEnumerable<Player<T>> Players(Partida<T> partida);
}

// Interfaz que contiene las condiciones de finalizacion de una partida
public interface IFinisher<T> {
    public bool GameOver(Partida<T> partida);
}

// Interfaz que se encarga de puntuar un movimientos y tokens
public interface IScorer<T> {
    // Dada una partida, puntua un movimiento
    public double Scorer(Partida<T> partida, Move<T> move);

    // Puntua un token
    public double TokenScorer(Token<T> token);

    // Rankea a los equipos despues de finalizar la partida
    public IEnumerable<Team<T>> Winner(Partida<T> partida);
}

// Esta interfaz define el concepto de un objeto que luego
// de una secuencia de pasos, puede rankear una lista de Teams
public interface IWinnerSelector<T>
{
    // Devuelve un IEnumerable de Teams rankeados
    public IEnumerable<Team<T>> Winner();

    // Dada una instancia de un objeto, este metodo devuelve una nueva instancia
    public IWinnerSelector<T> NewInstance(Judge<T> judge, IEnumerable<Team<T>> teams);

    public IEnumerable<Game<T>> Games(IWinnerSelector<T> winsel);
}