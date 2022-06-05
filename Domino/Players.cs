using Domino.Tokens;
using Domino.Moves;
using Domino.Strategies;

namespace Domino.Players;

public abstract class BasePlayer<TMove> where TMove : BaseMove {
    // Identifier
    public string Name { get; }
    // Strategy
    public IStrategy<TMove> Strategy { get; }

    public BasePlayer(string name, IStrategy<TMove> strategy) {
        this.Name = name;
        this.Strategy = strategy;
    }
}

public class GreedyPlayer<TMove> : BasePlayer<TMove> where TMove : BaseMove {
    public GreedyPlayer(int index, GreedyStrategy<TMove> strategy) : 
        base($"Greedy Player #{index}", strategy) {}
}
