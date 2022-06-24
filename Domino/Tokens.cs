namespace Domino.Tokens;

public class DominoToken {
    public int Left { get; }
    public int Right { get; }

    public DominoToken(int left, int right) {
        this.Left = left;
        this.Right = right;
    }
    public virtual int Value() {
        return this.Left + this.Right;
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

        if (this.Right != 6)
            value += this.Right;
        if (this.Left != 6)
            value += this.Left;

        return value;
    }
}

public class DoubledValueDominoToken : DominoToken {
    public DoubledValueDominoToken(int left, int right) : base(left, right) {}
    public override int Value()
    {
        return  2 * (this.Left + this.Right);
    }
}

public interface ITokenGenerator<TToken> {
    public IList<TToken> GenerateTokens(int tokenValues);
}

public class DominoTokenGenerator : ITokenGenerator<DominoToken> {
    public IList<DominoToken> GenerateTokens(int tokenValues) {
        List<int[]> tokenValuesList = new List<int[]>();
        List<DominoToken> tokens = new List<DominoToken>();

        Utils.Utils.GenerateTokenValues(tokenValues, tokenValuesList);
        
        foreach(int[] values in tokenValuesList) {
            tokens.Add(new DominoToken(values[0], values[1]));
        }

        return tokens;
    }
}
