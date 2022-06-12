namespace DominoEngine;

public abstract class Player<T>
{
    protected Hand<T>? _hand;

    public void SetHand(Hand<T> hand)
    {
        _hand = hand;
    }

    public Hand<T> Hand => _hand!;

    public abstract Move<T> Play(IList<Move<T>> moves);
}