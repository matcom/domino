namespace DominoEngine;

public class ClassicScorer : IScorer<int>
{
    public ClassicScorer() { }

    public double Scorer(Partida<int> partida, Move<int> move)
    {
        return move.Head + move.Tail;
    }

    public double TokenScorer(Ficha<int> token) => token.Head + token.Tail;

    public Team<int> Winner(Partida<int> partida)
    {
        foreach (var player in partida.Players().Where(x => partida.Hands[x].IsEmpty()))
            return partida.TeamOf(player);
        return partida.TeamOf(partida.Hands.MinBy(x => x.Value.Sum(x => TokenScorer(x))).Key);
    }
}

public class ClassicFinisher<T> : IFinisher<T>
{
    public ClassicFinisher() { }

    public bool GameOver(Partida<T> partida)
    {
        return AllCheck(partida) || PlayerEnd(partida);
    }

    public bool AllCheck(Partida<T> partida)
    {
        foreach (var player in partida.Players()){
            if (partida.Board.Count <= partida.Players().Count() 
                || !partida.Board.Where(x => x.PlayerId == partida.PlayerId(player)).Last().Check) 
                return false;
        }

        return true;
    }

    public bool PlayerEnd(Partida<T> partida)
    {
        foreach (var player in partida.Players())
            if (partida.Hand(player).Count() == 0) return true;

        return false;
    }
}

public class ClassicMatcher<T> : IMatcher<T>
{
    Dictionary<Partida<T>, List<int>> validsTurns = new();

    public ClassicMatcher() { }

    public IEnumerable<Move<T>> CanMatch(Partida<T> partida, IEnumerable<Move<T>> enumerable) {
        ValidsTurn(partida);
        var enume = enumerable.Where(x => !x.Check && CanMatch(partida, x));
        return (enume.IsEmpty())? enumerable.Where(x => x.Check) : enume;
    }

    private bool CanMatch(Partida<T> partida, Move<T> move)
    {
        if (partida.Board.Count == 0) return true;
        if (move.Check) return true;
        foreach (var validturn in validsTurns[partida]!.Where(x => x == move.Turn)) {
            if (validturn == -1) return partida.Board[0].Head!.Equals(move.Head);
            return partida.Board[validturn].Tail!.Equals(move.Head);
        }
        return false;
    }

    private void ValidsTurn(Partida<T> partida)
    {
        if (!validsTurns.ContainsKey(partida)) validsTurns.Add(partida, new List<int>(){0, -1});

        foreach (var (i,move) in partida.Board.Enumerate().Where(x => !x.Item2.Check &&
                x.Item1 > validsTurns[partida].MaxBy(x => x) && x.Item1 >= 1)) {
            validsTurns[partida].Remove(move.Turn);
            validsTurns[partida].Add(i);
        }
    }
}

public class LonganaMatcher<T> : IMatcher<T>
{
    Dictionary<Partida<T>, Dictionary<int, List<(int turn, int player)>>> validsTurns = new();

    public IEnumerable<Move<T>> CanMatch(Partida<T> partida, IEnumerable<Move<T>> enumerable) {
        ValidsTurn(partida, enumerable.First().PlayerId);
        var enume = enumerable.Where(x => !x.Check && CanMatch(partida, x));
        return (enume.IsEmpty())? enumerable.Where(x => x.Check) : enume;
    }

    private bool CanMatch(Partida<T> partida, Move<T> move) {
        // Permite salir solo con fichas dobles
        if (partida.Board.Count() == 0) return move.Head!.Equals(move.Tail);

        // Valida movimientos si estan disponibles y cumplen la condicion de longana
        foreach (var validturn in validsTurns[partida][move.PlayerId].Select(x => x.turn).Where(x => x == move.Turn)) {
            if (validturn == -1) return partida.Board[0].Head!.Equals(move.Head);
            return partida.Board[validturn].Tail!.Equals(move.Head);
        }
        
        return false;
    }

    private void ValidsTurn(Partida<T> partida, int player)
    {
        // Agregamos una nueva partida al diccionario de partidas si no existe
        if (!validsTurns.ContainsKey(partida)) {
            validsTurns.Add(partida, new Dictionary<int, List<(int, int)>>());
            partida.Players().Select(x => partida.PlayerId(x)).Make(x => validsTurns[partida].
            Add(x, new List<(int, int)>(){(-1, x)}));
        }

        // Eliminamos los turnos que ya no se pueden usar
        validsTurns[partida].Where(x => x.Key != player).
        Make(x => x.Value.Remove(validsTurns[partida][player].Where(x => x.player == player).First()));

        // Por cada jugador que se paso, agregamos un turno valido
        foreach (var playerId in partida.Players().Select(x => partida.PlayerId(x)).Where(x => x != player)) {
            if (partida.Board.Where(x => x.PlayerId == playerId).Count() == 0) 
                continue;
            var lastmove = partida.Board.Where(x => x.PlayerId == playerId).Last();
            if (lastmove.Check)
                validsTurns[partida][player].Add(validsTurns[partida][playerId].
                Where(x => x.player == playerId).MaxBy(x => x.Item1));
        }

        // Actualiza los ultimos cambios en el tablero
        foreach (var (i,move) in partida.Board.Enumerate().Where(x => x.Item1 > validsTurns[partida].
                SelectMany(x => x.Value).MaxBy(x => x.Item1).Item1 && !x.Item2.Check)) {
            // Si es el primer movimiento del juego, solo modificamos a un jugador
            if (i is 0 || move.Turn is -1) {
                validsTurns[partida][move.PlayerId] = new List<(int turn, int player)>() {(i, move.PlayerId)};
                continue;
            }

            // Sino modificamos a todos los que sean necesarios
            var turn_owner = validsTurns[partida].Where(x => x.Value.Contains((move.Turn, x.Key))).First().Key;
            foreach (var player_ in partida.Players().Select(x => partida.PlayerId(x))) {
                if (move.PlayerId == turn_owner && move.PlayerId == player_){
                    validsTurns[partida][player_].Remove((move.Turn, move.PlayerId));
                    validsTurns[partida][player_].Add((i, player_));
                }
                else
                    if (validsTurns[partida][player_].RemoveAll(x => x.turn == move.Turn) > 0)
                        validsTurns[partida][player_].Add((i, turn_owner));
            }
        }
    }
}

public class ClassicTurner<T> : ITurner<T>
{
    public ClassicTurner() { }

    public IEnumerable<Player<T>> Players(Partida<T> partida)
    {
        while (true)
            foreach (var player in partida!.Players())
                yield return player;
    }
}

public class ClassicDealer<T> : IDealer<T>
{
    int _pieceForPlayers;
    public ClassicDealer(int piecesForPlayers)
    {
        _pieceForPlayers = piecesForPlayers;
    }

    public Dictionary<Player<T>, Hand<T>> Deal(Partida<T> partida, IEnumerable<Ficha<T>> fichas)
    {
        Dictionary<Player<T>, Hand<T>> hands = new();
        Random r = new Random();
        var enumerator = fichas.OrderBy(x => r.NextDouble()-0.5).GetEnumerator();

        foreach (var player in partida.Players()){
            var hand = new Hand<T>();
            var count = 0;
            while (count++ < _pieceForPlayers && enumerator.MoveNext())
                hand.Add(enumerator.Current);
            hands.Add(player, hand);
        }

        return hands;
    }
}

public class ClassicGenerator : IGenerator<int>
{
    int _number;

    public ClassicGenerator(int number)
    {
        _number = number;
    }

    IEnumerable<Ficha<int>> IGenerator<int>.Generate()
    {
        List<Ficha<int>> fichas = new List<Ficha<int>>();

        for (int i = 0; i < _number; i++)
            for (int j = i; j < _number; j++)
                fichas.Add(new Ficha<int>(i,j));

        return fichas;
    }
}
