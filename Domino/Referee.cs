using Domino.Players;
using Domino.Tokens;
using Domino.Game;

namespace Domino.Referee;

/// <summary>
///     Interface that represents if a given game has met the stop conditions
/// </summary>
public interface IWinCondition {
    /// <summary>
    ///     Boolean method to decide if given a game status, the win condition has been met
    /// </summary>
    public bool Achieved(
        IEnumerable<DominoPlayer> players, 
        IEnumerable<IEnumerable<DominoToken>> playerTokens, 
        int passCounter, int[] freeValues, IEnumerable<DominoMove> moves
    );
}

/// <summary>
///     Standard game win conditions implementations: 4 passes in a row or no player can play
/// </summary>
public class StandardWinCondition : IWinCondition {
    public bool Achieved(
        IEnumerable<DominoPlayer> players, 
        IEnumerable<IEnumerable<DominoToken>> playerTokens, 
        int passCounter, int[] freeValues, IEnumerable<DominoMove> moves
    ) {
        if (playerTokens.Where(tokens => tokens.Count() == 0).Count() > 0)
            return true;

        if (passCounter == players.Count())
                return true;
            return false;
    }

    public override string ToString()
    {
        return "Standard Domino Win Condition";
    }
}

/// <sumary>
///     Four game rounds condition: the round concept is taken as when all players have played,
///     in a row, even if they did not make a move
///</summary>
public class FourRoundsWinCondition : IWinCondition {
    int counter = 4;
    int turns = 0;
    public bool Achieved(
        IEnumerable<DominoPlayer> players, 
        IEnumerable<IEnumerable<DominoToken>> playerTokens, 
        int passCounter, int[] freeValues, IEnumerable<DominoMove> moves
    ) {
        this.turns++;

        if (playerTokens.Where(tokens => tokens.Count() == 0).Count() > 0)
            return true;

        if (this.turns / players.Count() == counter)
            return true;
        
        return false;
    }

    public override string ToString() {
        return "Four Rounds Win Condition";
    }
}

/// <summary>
///     Interface for winner concept abstraction. No tie concept implemented yet
/// </summary>
public interface IWinner {
    /// <summary>
    ///     Given a certain game state, returns the player that met the winner conditions
    /// </summary>
    public DominoPlayer GetWinner(IEnumerable<DominoPlayer> players, IEnumerable<IEnumerable<DominoToken>> playerTokens);
}

/// <summary>
///     The player with the less score (token value) wins
/// </summary>
public class MinValueWinner : IWinner {
    public DominoPlayer GetWinner(IEnumerable<DominoPlayer> players, IEnumerable<IEnumerable<DominoToken>> playerTokens) {
        int[] playerScores = new int[players.Count()];

        System.Console.WriteLine("Scores:");

        for(int i = 0; i < players.Count(); i++)
        {
            playerScores[i] += playerTokens.ElementAt(i).Sum(token => token.Value());
            System.Console.WriteLine($"{players.ElementAt(i)}: {playerScores[i]}");
        }

        return players.ElementAt(Array.IndexOf(playerScores, playerScores.Min()));
    }

    public override string ToString()
    {
        return "Minimum Value Winner";
    }
}

/// <summary>
///     The player with the highest amount of tokens in hand wins
/// </summary>
public class MoreTokensWinner : IWinner {
    public DominoPlayer GetWinner(IEnumerable<DominoPlayer> players, IEnumerable<IEnumerable<DominoToken>> playerTokens) {
        int[] playerTokenAmount = new int[players.Count()];

        System.Console.WriteLine("Scores:");

        for (int i = 0; i < players.Count(); i++) {
            playerTokenAmount[i] = playerTokens.ElementAt(i).Count();
            System.Console.WriteLine($"{players.ElementAt(i)}: {playerTokenAmount[i]}");
        }

        return players.ElementAt(Array.IndexOf(playerTokenAmount, playerTokenAmount.Max()));
    }

    public override string ToString()
    {
        return "Maximum Amount of Tokens Winner";
    }
}
