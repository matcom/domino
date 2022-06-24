using Domino.Game;
using Domino.Tokens;

namespace Domino.Players;

public class DominoPlayer {
    string identifier;
    public DominoPlayer(string name) {
        this.identifier = name;
    }
    public virtual DominoMove PlayToken(
        IEnumerable<DominoToken> tokens, IEnumerable<DominoMove> moves, int[] freeValues
    ) {
        IEnumerable<DominoToken> availableTokens = tokens.Where(
            token => DominoMove.IsValid(token, freeValues)
        );

        DominoToken selected = availableTokens.OrderBy(token => token.Value()).First();

        return new DominoMove(this, selected);
    }

    public virtual DominoToken PlayStartToken(IEnumerable<DominoToken> tokens) {
        return tokens.First();
    }

    public override string ToString() {
        return this.identifier;
    }
}
