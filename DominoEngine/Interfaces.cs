namespace DominoEngine;

public interface IPlayer<T>
{
    public virtual void Play(IBoard<T> board){}
}

public interface IRandomPlayer<T> : IPlayer<T>
{
    public virtual void PlayRandom(){}
}

public interface ILinkerable
{
    public bool ConectTo(ILinkerable d, ILinker<ILinkerable> linker);
}

public interface IBoard<T>
{
    public void NextAction(IPlayer<T> player, IBoard<T> state);

    public abstract IList<T> PossiblesActions();
}

public interface ILinker<T>
{
    public bool Link(T t1, T t2);
}
