namespace DominoEngine;

public record Move<T>(int PlayerId, bool Check = true, int Turn = -2, T? Head = default, T? Tail = default) {
	public Token<T?> Token => new Token<T?>(Head, Tail);

    public override string ToString()
    {
        if (Check) return "Pass";
		else return Token.ToString()!;
    }
}