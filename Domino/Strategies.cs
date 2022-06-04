using Domino.Tokens;
using Domino.Rules;
using Domino.Boards;

namespace Domino.Strategies;

public abstract class BaseStrategy {
    public abstract void MakeMove(IEnumerable<IToken> tokens, IRuleSet<IRule> rules, BaseBoard board);
}

public class GreedyStrategy : BaseStrategy {
    public override void MakeMove(IEnumerable<IToken> tokens, IRuleSet<IRule> rules, BaseBoard board) {
        throw new NotImplementedException();
    }
}