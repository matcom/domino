namespace DominoEngine;

public class Int : ILinkerable
{
    public int value;
    public Int(int value)
    {
        this.value = value;
    }

    public bool ConectTo(ILinkerable d, ILinker<ILinkerable> linker)
    {
        return linker.Link(this, d);
    }
}
