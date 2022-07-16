using Domino.Game;
using Domino.Tokens;

namespace Domino.Players;

/// <summary>
///     Basic domino player abstraction, a player can be defined just by giving it an
///     identifier, and asking how to play given certain circumstances
/// </summary>
public abstract class DominoPlayer {
    string identifier;

    public DominoPlayer(string name) {
        this.identifier = name;
    }

    /// <returns>
    ///     Valid <c>DominoMove</c> object, that represents the move to make, given certain 
    ///     game status
    /// </returns>
    public abstract DominoMove Play(
        IEnumerable<DominoToken> tokens, IEnumerable<DominoMove> moves, int[] freeValues
    );

    /// <summary>
    ///     Chooses a token from the starting hand to play the first move of the game
    /// </summary>
    public abstract DominoToken PlayStartToken(IEnumerable<DominoToken> tokens);

    public override string ToString() {
        return this.identifier;
    }
}

/// <summary>
///     Greedy algorithm implementation for a domino player, it chooses the highest value
///     token from a subset of playable tokens at this moment
/// </summary>
public class GreedyDominoPlayer : DominoPlayer {
    public GreedyDominoPlayer(string name) : base(name) {}
    public override DominoMove Play(
        IEnumerable<DominoToken> tokens, IEnumerable<DominoMove> moves, int[] freeValues
    ) {
        IEnumerable<DominoToken> availableTokens = tokens.Where(
            token => DominoMove.IsValid(token, freeValues)
        );

        DominoToken selected = availableTokens.OrderBy(token => token.Value()).First();

        return new DominoMove(this, selected);
    }

    public override DominoToken PlayStartToken(IEnumerable<DominoToken> tokens) {
        return tokens.OrderBy(token => token.Value()).First();
    }

    public override string ToString() {
        return $"Greedy Player - {base.ToString()}";
    }
}

/// <summary>
///     Random algorithm implementation for a domino player, it chooses a random token
///     from a subset of playable tokens at this moment
/// </summary>
public class RandomDominoPlayer : DominoPlayer {
    Random randObj = new Random();
    public RandomDominoPlayer(string name) : base(name) {}

    public override DominoToken PlayStartToken(IEnumerable<DominoToken> tokens)
    {
        return tokens.ElementAt(this.randObj.Next(tokens.Count()));
    }

    public override DominoMove Play(
        IEnumerable<DominoToken> tokens, IEnumerable<DominoMove> moves, int[] freeValues
    ) {
        IEnumerable<DominoToken> availableTokens = tokens.Where(
            token => DominoMove.IsValid(token, freeValues)
        );

        return new DominoMove(this, availableTokens.ElementAt(randObj.Next(availableTokens.Count())));
    }

    public override string ToString()
    {
        return $"Random Player - {base.ToString()}";
    }
}

/// <summary>
///     Human implementation for a domino player, this allow human players to choose a move
///     from a collection of available tokens and given game state
/// </summary>
public class HumanDominoPlayer : DominoPlayer {
    public HumanDominoPlayer(string name) : base(name) {}

    public override DominoToken PlayStartToken(IEnumerable<DominoToken> tokens)
    {
        System.Console.WriteLine("Enter the initial token to play:");
        for (int i = 0; i < tokens.Count(); i++)
        {
            System.Console.WriteLine($"{i} - {tokens.ElementAt(i)}");
        }

        string? input = Console.ReadLine();
        int tokenIndex = int.Parse(input == null ? "" : input);

        return tokens.ElementAt(tokenIndex);
    }

    public override DominoMove Play(IEnumerable<DominoToken> tokens, IEnumerable<DominoMove> moves, int[] freeValues)
    {
        IEnumerable<DominoToken> availableTokens = tokens.Where(
            token => DominoMove.IsValid(token, freeValues)
        );

        DominoToken? selected = null;

        System.Console.WriteLine($"The available numbers are {freeValues[0]} and {freeValues[1]}");

        System.Console.WriteLine("Enter the token to play:");
        for (int i = 0; i < availableTokens.Count(); i++)
        {
            System.Console.WriteLine($"{i} - {availableTokens.ElementAt(i)}");
        }

        while (selected == null) {
            string? input = Console.ReadLine();
            int tokenIndex = int.Parse(input == null ? "" : input);

            try {
                selected = availableTokens.ElementAt(tokenIndex);
            } catch (System.ArgumentOutOfRangeException) {
                selected = null;
                System.Console.WriteLine("Invalid token index");
            }
        }

        return new DominoMove(this, selected!);
    }

    public override string ToString()
    {
        return $"Human Player - {base.ToString()}";
    }
}

/// <summary>
///     Implementation for a domino player that plays prioritizing a given data, which
///     is defined as the token numeration with more ocurrences within player's hand
/// </summary>
public class DataDominoPlayer : DominoPlayer {
    int data = -1;
    int maxTokenValue;
    public DataDominoPlayer(string name, int maxValue) : base(name) {
        this.maxTokenValue = maxValue;
    }

    int GetData(IEnumerable<DominoToken> tokens) {
        int[] values = new int[this.maxTokenValue + 1];

        foreach (DominoToken token in tokens) {
            if (token.IsDouble) {
                values[token.Left]++;
                continue;
            }

            values[token.Left]++;
            values[token.Right]++;
        }

        return values.Max();
    }
    public override DominoMove Play(
        IEnumerable<DominoToken> tokens, IEnumerable<DominoMove> moves, int[] freeValues
    ) {
        if (this.data == -1) 
            this.data = GetData(tokens);

        IEnumerable<DominoToken> availableTokens = tokens.Where(
            token => DominoMove.IsValid(token, freeValues)
        );

        IEnumerable<DominoToken> dataSelection = 
            availableTokens.Where(token => token.Left == this.data || token.Right == this.data);
        
        if (dataSelection.Count() != 0)
            return new DominoMove(this, dataSelection.First());
        
        return new DominoMove(this, availableTokens.First());
    }

    public override DominoToken PlayStartToken(IEnumerable<DominoToken> tokens) {
        if (this.data == -1) 
            this.data = GetData(tokens);
        
        return tokens.Where(token => token.Left == this.data || token.Right == this.data).First();
    }

    public override string ToString()
    {
        return $"Data Player - {base.ToString()}";
    }
}
