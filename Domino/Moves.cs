using Domino.Tokens;
using Domino.Players;
using Domino.Utils;

namespace Domino.Moves;

public class MoveValueComparer : IComparer<BaseMove> {
    public int Compare(BaseMove x, BaseMove y) {
        if (x.Token.Value() > y.Token.Value())
            return 1;
        if (x.Token.Value() == y.Token.Value())
            return 0;
        return -1;
    }
}

public class BaseMove {
    public BasePlayer Player { get; }
    public GraphNode<IToken> Node { get; }
    public IToken Token { get; }

    public BaseMove(BasePlayer player, IToken token, GraphNode<IToken> node) {
        this.Player = player;
        this.Node = node;
        this.Token = token;
    }
}
