namespace DominoEngine;

public abstract class DominoBoard<T> : IBoard<DominoPiece<T>> where T : ILinkerable
{
    IList<Stack<DominoPiece<T>>> branches = new List<Stack<DominoPiece<T>>>();
    protected DominoBoard(DominoPiece<T> init, Rules rules) {}

    public void NextAction(IPlayer<DominoPiece<T>> player, IBoard<DominoPiece<T>> state)
    {
        player.Play(this);
    }

    public IList<DominoPiece<T>> PossiblesActions()
    {
        IList<DominoPiece<T>> heads = new List<DominoPiece<T>>();

        foreach (var branch in branches)
        {
            heads.Add(branch.Peek());
        }

        return heads;
    }

    public Stack<DominoPiece<T>> FindPiece(DominoPiece<T> piece)
    {
        foreach (var branch in branches)
        {
            foreach (var item in branch)
            {
                if (item.Equals(piece)) return branch;
            }
        }

        return null!;
    }
}
