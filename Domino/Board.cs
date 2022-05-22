namespace Domino.Board;

public abstract class BaseBoard {
    IEnumerable<BaseToken> TokensInPlay { public get; protected set; }
}
