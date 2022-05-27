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
        NodeGeometry aux = (obj as NodeGeometry)!;
        if (aux == null) return false;
        return this.Ubication.Equals(aux.Ubication);
    }
    public override int GetHashCode()
    {
        return this.Ubication.GetHashCode();
    }
}