namespace Table;

public abstract class TableGeometry : TableGame
{
    public override HashSet<Node> PlayNode { get; protected set; }
    public override HashSet<Node> FreeNode { get; protected set; }
    public override List<Node> TableNode { get; protected set; }
    /// <summary>Coordenadas con sus respectivos nodos</summary>
    public Dictionary<Coordenates, Node> TableCoord { get; protected set; }
    /// <summary>Valor que contiene cada coordenada</summary>
    public Dictionary<(int, int), int> CoordValor { get; protected set; }
    protected TableGeometry(Token token, (int, int)[] coordenates)
    {
        this.PlayNode = new HashSet<Node>();
        this.FreeNode = new HashSet<Node>();
        this.TableNode = new List<Node>();
        this.TableCoord = new Dictionary<Coordenates, Node>();
        this.CoordValor = new Dictionary<(int, int), int>();
        Node node = this.CreateNode(coordenates);
        node.ValueToken = token;
        this.Expand(node);
        this.PlayNode.Add(node);
    }
    /// <summary>Buscar las coordenadas de un nodo expandido</summary>
    /// <param name="coordenates">Coordenadas del nodo a expandir</param>
    /// <returns>Coordenadas del nodo expandido</returns>
    protected abstract (int, int)[] ExpandGeometry((int, int)[] coordenates);
    /// <summary>Asignar las coordenadas obtenidas en ExpandGeometry</summary>
    /// <param name="coordenates">Coordenadas del nodo a asignar</param>
    protected virtual void AsignCoordenates(Node node, (int, int)[] coordenates)
    {
        Coordenates aux = new Coordenates(coordenates);
        //Comprobar si ya existe un nodo con esas coordenadas
        if (!this.TableCoord.ContainsKey(aux))
        {
            this.FreeTable(this.CreateNode(coordenates));
        }
        int j = 0;
        //Buscar las conexiones libres
        while (node.Conections[j] != null)
        {
            j++;
            if (j == node.Conections.Length) break;
        }
        if (j == node.Conections.Length) return;
        this.UnionNode(node, this.TableCoord[aux], j);
    }
    /// <summary>Crear un nodo</summary>
    /// <param name="coordenates">Cooredenadas de la ficha</param>
    /// <returns>Nuevo nodo</returns>
    protected NodeGeometry CreateNode((int, int)[] coordenates)
    {
        NodeGeometry node = new NodeGeometry(coordenates);
        this.TableNode.Add(node);
        this.TableCoord.Add(node.Ubication, node);
        return node;
    }
}