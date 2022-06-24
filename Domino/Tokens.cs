namespace Domino.Tokens;

public class DominoToken {
    public int Left { get; }
    public int Right { get; }

    public DominoToken(int left, int right) {
        this.Left = left;
        this.Right = right;
    }
    public virtual int Value() {
        return Left + Right;
    }

    public override string ToString()
    {
        return $"({Left} | {Right}) : {this.Value()}";
    }
}

public class SixUnvaluableDominoToken : DominoToken {
    public SixUnvaluableDominoToken(int left, int right) : base(left, right) {}

    public override int Value()
    {
        int value = 0;

        if (Right != 6)
            value += Right;
        if (Left != 6)
            value += Left;

        return value;
    }
}

public class DoubledValueDominoToken : DominoToken {
    public DoubledValueDominoToken(int left, int right) : base(left, right) {}
    public override int Value()
    {
        return  2 * (Left + Right);
    }
}
