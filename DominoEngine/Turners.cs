using DominoEngine;

namespace Rules;

public class ClassicTurner<T> : ITurner<T>
{
    // Turner clasico que reparte los turnos en un orden estricto
    public IEnumerable<Player<T>> Players(Partida<T> partida) => partida.Teams().OneByOne().Infinity();

    public override string ToString()
        => "Sentido de los turnos clasico";
}

public class NPassesReverseTurner<T> : ITurner<T>
{
    private readonly int _n; 
    public NPassesReverseTurner(int NumberOfPasses) {
        _n = NumberOfPasses;
    }

    // Cambia el sentido de reparticion de turnos cuando ocurran n pases
    public IEnumerable<Player<T>> Players(Partida<T> partida) {
        var passes = 0;
        var index = 0;
        var players = partida.Teams().OneByOne();

        while (true) {
            foreach (var (i, player) in players.Infinity().Enumerate().Skip(index)) {
                yield return player; 
                if (partida.Board.Count(x => x.Check) == passes ||
                    partida.Board.Count(x => x.Check) % _n is not 0) continue; 
                passes += _n; 
                index = players.Count() - i;
                players = players.Reverse(); 
                break;
            }
        }
    }   

    public override string ToString()
        => "Despues de n pases, el sentido de los turnos se invierte"; 
}

public class RandomTurner<T> : ITurner<T>
{
    public IEnumerable<Player<T>> Players(Partida<T> partida) {
        while (true)
            yield return partida.Players().ElementAt(new Random().Next(partida.Players().Count()));
    }

    public override string ToString()
        => "Devuelve los turnos en un orden aleatorio";
}
