namespace Table;

public class TableDimension : TableGame
{
    /// <summary>Nodos que contienen una ficha</summary>
    public override HashSet<Node> PlayNode { get; protected set; }
    /// <summary>Nodos disponibles para jugar</summary>
    public override HashSet<Node> FreeNode { get; protected set; }
    /// <summary>Nodos contenidos en el grafo</summary>
    public List<Node> TableNode { get; protected set; }
    public TableDimension(Token token)
    {
        this.PlayNode = new HashSet<Node>();
        this.FreeNode = new HashSet<Node>();
        this.TableNode = new List<Node>();
        Node node = CreateNode(token.CantValues);
        node.ValueToken = token;
        Expand(node);
        this.PlayNode.Add(node);
    }
    protected override void Expand(Node node)
    {
        for (int i = 0; i < node.Conections.Length; i++)
        {
            if (node.Conections[i] == null)
            {
                this.UnionNode(node, CreateNode(node.Conections.Length), i);
                this.FreeTable(node.Conections[i]);
            }
        }
    }
    /// <summary>Crear un nodo</summary>
    /// <param name="n">Numero de aristas</param>
    /// <returns>Nuevo nodo</returns>
    protected Node CreateNode(int n)
    {
        Node node = new NodeDimension(n);
        this.TableNode.Add(node);
        return node;
    }
}
