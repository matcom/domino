namespace DominoEngine;

public record Move<T>(T Head, T Tail, int PlayerId, bool Check = false, int Turn = -2);