using Domino.Tokens;
using Domino.Players;
using Domino.Utils;

namespace Domino.Moves;

public class BaseMove {
    public BasePlayer Player { get; }
    public GraphNode<BaseToken> Node { get; }

    public BaseMove(BasePlayer player, GraphNode<BaseToken> node) {
        this.Player = player;
        this.Node = node;
    }
}
