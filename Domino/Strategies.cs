using Domino.Tokens;
using Domino.Boards;
using Domino.Moves;

namespace Domino.Strategies;

public interface IStrategy<TMove> where TMove : BaseMove {
    public TMove MakeMove(IEnumerable<IToken> tokens, BaseBoard board);
}

public class GreedyStrategy<TMove> : IStrategy<TMove> where TMove : BaseMove {
    public TMove MakeMove(IEnumerable<IToken> tokens, BaseBoard board) {
        throw new NotImplementedException();
    }
}