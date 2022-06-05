using Domino.Tokens;
using Domino.Utils;
using Domino.Boards;

namespace Domino.Rules;

public interface ITokenRule<TToken> where TToken : IToken {
    // Is next move valid
    public bool IsValid(TToken token, Graph<TToken> board);
}

public interface IPlayerRule {

}

public class BasicDominoTokenRules : ITokenRule<DominoToken> {
    public bool IsValid(DominoToken token, Graph<DominoToken> board) {
        throw new NotImplementedException();
    }
}
