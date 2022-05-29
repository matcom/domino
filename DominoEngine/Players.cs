namespace DominoEngine;

public class DominoPlayer<T> : IPlayer<DominoPiece<T>> where T : ILinkerable
{
    IList<DominoPiece<T>> _pieces;

    public DominoPlayer(IList<DominoPiece<T>> pieces)
    {
        _pieces = pieces;
    }

    public virtual void Play(DominoBoard<T> state, IList<ILinker<ILinkerable>> linkers) {}
}

public class RandomDominoPlayer<T> : DominoPlayer<T>, IRandomPlayer<DominoPiece<T>> where T : ILinkerable
{
    IList<DominoPiece<T>> _pieces;

    public RandomDominoPlayer(IList<DominoPiece<T>> pieces) : base(pieces)
    {
        _pieces = pieces;
    }

    public override void Play(DominoBoard<T> state, IList<ILinker<ILinkerable>> linkers)
    {
        PlayRandom(state, linkers);
    }

    public void PlayRandom(DominoBoard<T> state, IList<ILinker<ILinkerable>> linkers)
    {
        Random r = new Random();
        IList<Tuple<DominoPiece<T>, DominoPiece<T>, T, T>> possibilities
            = GetAllPossibilities(state, linkers);
        Tuple<DominoPiece<T>, DominoPiece<T>, T, T> tuple
            = possibilities[r.Next(possibilities.Count)];

        DominoPiece<T>.Match(state, tuple);
    }

    public IList<Tuple<DominoPiece<T>, DominoPiece<T>, T, T>> 
        GetAllPossibilities(DominoBoard<T> state, IList<ILinker<ILinkerable>> linkers)
    {
        IList<Tuple<DominoPiece<T>, DominoPiece<T>, T, T>> possibilities = new List<Tuple<DominoPiece<T>, DominoPiece<T>, T, T>>();

        foreach (var head in state.PossiblesActions())
        {
            foreach (var option1 in head.AskAvailables())
            {
                foreach (var piece in _pieces)
                {
                    foreach (var option2 in piece.AskAvailables())
                    {
                        foreach (var linker in linkers)
                        {
                            if (option1.ConectTo(option2, linker))
                            {
                                DominoPiece<T> t1 = head;
                                DominoPiece<T> t2 = piece;
                                T t3 = option1;
                                T t4 = option2;

                                Tuple<DominoPiece<T>, DominoPiece<T>, T, T> tuple 
                                    = new Tuple<DominoPiece<T>, DominoPiece<T>, T, T>(t1, t2, t3, t4);

                                possibilities.Add(tuple);
                            } 
                        }
                    }
                }
            }
        }

        return possibilities;
    }
}