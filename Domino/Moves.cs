using Domino.Tokens;
using Domino.Players;
using Domino.Utils;

namespace Domino.Moves;

public class BaseMove {
    public BasePlayer Player { get; }
    public GraphNode<IToken> Node { get; }

    public BaseMove(BasePlayer player, GraphNode<IToken> node) {
        this.Player = player;
        this.Node = node;
    }
}
