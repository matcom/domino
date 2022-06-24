using Domino.Game;
using Domino.Tokens;

namespace Domino.Players;

public abstract class DominoPlayer {
    string identifier;

    public DominoPlayer(string name) {
        this.identifier = name;
    }
    public abstract DominoMove PlayToken(
        IEnumerable<DominoToken> tokens, IEnumerable<DominoMove> moves, int[] freeValues
    );

    public abstract DominoToken PlayStartToken(IEnumerable<DominoToken> tokens);

    public override string ToString() {
        return this.identifier;
    }
}

public class GreedyDominoPlayer : DominoPlayer {
    public GreedyDominoPlayer(string name) : base(name) {}
    public override DominoMove PlayToken(
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
        return $"Greedy - {base.ToString()}";
    }
}

public class RandomDominoPlayer : DominoPlayer {
    Random randObj = new Random();
    public RandomDominoPlayer(string name) : base(name) {}

    public override DominoToken PlayStartToken(IEnumerable<DominoToken> tokens)
    {
        return tokens.ElementAt(this.randObj.Next(tokens.Count()));
    }

    public override DominoMove PlayToken(
        IEnumerable<DominoToken> tokens, IEnumerable<DominoMove> moves, int[] freeValues
    ) {
        IEnumerable<DominoToken> availableTokens = tokens.Where(
            token => DominoMove.IsValid(token, freeValues)
        );

        return new DominoMove(this, availableTokens.ElementAt(randObj.Next(availableTokens.Count())));
    }

    public override string ToString()
    {
        return $"Random - {base.ToString()}";
    }
}

