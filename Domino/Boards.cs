using Domino.Moves;
using Domino.Tokens;
using Domino.Utils;

namespace Domino.Boards;

public abstract class BaseBoard {
    // Table Like Structure
    public Graph<BaseToken> Table { get; protected set; }
    public BaseBoard(Graph<BaseToken> table) {
        this.Table = table;
    }
    // Move history
    public abstract IEnumerable<BaseMove> Moves();
}

public class DominoBoard : BaseBoard {
    List<BaseMove> moves = new List<BaseMove>();
    int[] currentBoard;

    public DominoBoard(Graph<BaseToken> table, DominoToken openToken) : base(table) {
        this.currentBoard = openToken.Values().ToArray<int>();
    }
    public override IEnumerable<BaseMove> Moves() {
        return this.moves;
    }
}
