using Domino.Tokens;
using Domino.Strategies;

namespace Domino.Players;

public abstract class BasePlayer {
    // Identifier
    public string Name { get; }
    // Tokens to play
    public IEnumerable<BaseToken> AvailableTokens { get; set; }
    // Strategy
    public BaseStrategy Strategy { get; }

    public BasePlayer(string name, IEnumerable<BaseToken> tokens, BaseStrategy strategy) {
        this.Name = name;
        this.AvailableTokens = tokens;
        this.Strategy = strategy;
    }
}

public class GreedyPlayer : BasePlayer {
    public GreedyPlayer(int index, IEnumerable<BaseToken> tokens, GreedyStrategy strategy) : 
        base($"Greedy Player #{index}", tokens, strategy) {}
}
