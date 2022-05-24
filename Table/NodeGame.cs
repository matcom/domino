namespace Table;

public class NodeGame
{
    public Token ValueToken { get; set; }
    public NodeGame[] Conections { get; set; }

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
    public NodeGame(int n)
    {
        this.ValueToken = null!;
        this.Conections = new NodeGame[n];
    }
}