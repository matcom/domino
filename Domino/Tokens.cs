namespace Domino.Tokens;

public class DominoToken {
    public int Left { get; }
    public int Right { get; }

    public DominoToken(int left, int right) {
        this.Left = left;
        this.Right = right;
    }
    public int Value() {
        return Left + Right;
    }

    public override string ToString()
    {
        return $"({Left} | {Right})";
    }
}
