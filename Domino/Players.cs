using Domino.Tokens;
using Domino.Strategies;

namespace Domino.Players;

public abstract class BasePlayer {
    // Identifier
    public string Name { get; }
    // Strategy
    public BaseStrategy Strategy { get; }

    public BasePlayer(string name, BaseStrategy strategy) {
        this.Name = name;
        this.Strategy = strategy;
    }
}

public class GreedyPlayer : BasePlayer {
    public GreedyPlayer(int index, GreedyStrategy strategy) : 
        base($"Greedy Player #{index}", strategy) {}
}
