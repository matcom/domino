namespace DominoEngine;

public abstract class Rules {}

public class DominoRules : Rules {}

public abstract class Linkers<T> : DominoRules, ILinker<T> where T : ILinkerable
{
    public abstract bool Link(T t1, T t2);
}

public class IntLinker : Linkers<Int>
{
    public IntLinker() {}

    public override bool Link(Int t1, Int t2)
    {
        return t1.value == t2.value;
    }
}

public class IntLinkerGeneral : IntLinker
{
    int _a;
    public IntLinkerGeneral(int a)
    {
        _a = a;
    }

    public override bool Link(Int t1, Int t2)
    {
        return Math.Abs(t1.value - t2.value) <= _a;
    }
}