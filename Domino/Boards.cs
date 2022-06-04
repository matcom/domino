using Domino.Moves;
using Domino.Tokens;
using Domino.Utils;
using Domino.Players;

namespace Domino.Boards;

public abstract class BaseBoard {
    // Table Like Structure
    public Graph<IToken> Table { get; protected set; }
    public BaseBoard(Graph<IToken> table) {
        this.Table = table;
    }
    // Move history
    public abstract IEnumerable<BaseMove> Moves();
    public abstract void AddMove(BasePlayer player, IToken token, GraphNode<IToken> node);
}

public class DominoBoard : BaseBoard {
    List<BaseMove> moves = new List<BaseMove>();

    public DominoBoard(Graph<IToken> table) : base(table) {}
    public override IEnumerable<BaseMove> Moves() {
        return this.moves;
    }

    public override void AddMove(BasePlayer player, IToken token, GraphNode<IToken> node) {
        node.SetValue(token);
        this.moves.Add(new BaseMove(player, node));
        this.Table.AddNodes(node, 1);
    }
}
