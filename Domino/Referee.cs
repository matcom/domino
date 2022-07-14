using Domino.Players;
using Domino.Tokens;
using Domino.Game;

namespace Domino.Referee;

public interface IWinCondition {
    public bool Achieved(
        IEnumerable<DominoPlayer> players, 
        IEnumerable<IEnumerable<DominoToken>> playerTokens, 
        int passCounter, int[] freeValues, IEnumerable<DominoMove> moves
    );
}

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

public interface IPlayerOrder {
    public int NextPlayer(IEnumerable<DominoPlayer> players, int currentIndex, int passCounter);
}

public class StandardPlayerOrder : IPlayerOrder {
    public int NextPlayer(IEnumerable<DominoPlayer> players, int currentIndex, int passCounter) {
        return currentIndex % players.Count();
    }

    public override string ToString()
    {
        return "Standard Domino Player Order";
    }
}

public class OnPassInvertedPlayerOrder : IPlayerOrder {
    bool inverted = false;

    public int NextPlayer(IEnumerable<DominoPlayer> players, int currentIndex, int passCounter) {
        if (passCounter > 0)
            inverted = true;

        if (!inverted)
            return Math.Abs(currentIndex++ % players.Count());
        else
            return Math.Abs(currentIndex-- % players.Count());
    }

    public override string ToString()
    {
        return "On Pass Inverted player Order";
    }
}

public interface IWinner {
    public DominoPlayer GetWinner(IEnumerable<DominoPlayer> players, IEnumerable<IEnumerable<DominoToken>> playerTokens);
}

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
