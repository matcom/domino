namespace Table;

public class NodeGeometry : Node
{
    public override Token ValueToken { get; set; }
    public override Node[] Conections { get; set; }
    public Coordenates Ubication { get; private set; }
    public NodeGeometry((int, int)[] conections)
    {
        (int, int)[] aux = new (int, int)[conections.Length];
        this.Conections = new Node[conections.Length];
        this.Ubication = new Coordenates(conections);
        this.ValueToken = null!;
    }
    public override bool Equals(object? obj)
    {
        return this.Ubication.Equals(obj);
    }
    public override int GetHashCode()
    {
        return this.Ubication.GetHashCode();
    }
}