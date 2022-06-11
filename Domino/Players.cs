using Domino.Tokens;
using Domino.Moves;
using Domino.Strategies;

namespace Domino.Players;

public abstract class BasePlayer {
    // Identifier
    public string Name { get; }
    // Strategy
    public IStrategy Strategy { get; }

    public BasePlayer(string name, IStrategy strategy) {
        this.Name = name;
        this.Strategy = strategy;
    }
}

public class GreedyPlayer : BasePlayer {
    public GreedyPlayer(int index, GreedyStrategy strategy) : 
        base($"Greedy Player #{index}", strategy) {}
}
