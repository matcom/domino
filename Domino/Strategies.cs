using Domino.Tokens;
using Domino.Boards;
using Domino.Moves;
using Domino.Utils;

namespace Domino.Strategies;

public interface IStrategy {
    public BaseMove MakeMove(IEnumerable<BaseMove> moves, BaseBoard board);
}

public class GreedyStrategy : IStrategy {
    public BaseMove MakeMove(IEnumerable<BaseMove> moves, BaseBoard board) {
        return moves.OrderBy(move => move.Token.Value()).First();
    }
}