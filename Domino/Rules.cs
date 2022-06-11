using Domino.Tokens;
using Domino.Utils;
using Domino.Boards;
using Domino.Moves;

namespace Domino.Rules;

public interface ITokenRule<TToken> where TToken : IToken {
    // Is next move valid
    public bool IsValid(BaseMove move, Graph<TToken> board);
}

public interface IPlayerRule {

}

public class BasicDominoTokenRules : ITokenRule<DominoToken> {
    public bool IsValid(BaseMove move, Graph<DominoToken> board) {
        throw new NotImplementedException();
    }
}
