namespace Table;

public class NodeDimension : Node
{
    public override Token ValueToken { get; set; }
    public override Node[] Conections { get; set; }
    public int FirstConectionFree
    {
        get { return ConectionFree(); }
    }
    private int ConectionFree()
    {
        for (int i = 0; i < this.Conections.Length; i++)
        {
            if (this.Conections[i] == null) return i;
        }
        return -1;
    }
    public NodeDimension(int n)
    {
        this.ValueToken = null!;
        this.Conections = new Node[n];
    }
}