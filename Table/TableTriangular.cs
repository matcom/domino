namespace Table;

public class TableTriangular : TableGame
{
    /// <summary>Nodos que contienen una ficha</summary>
    public override HashSet<Node> PlayNode { get; protected set; }
    /// <summary>Nodos disponibles para jugar</summary>
    public override HashSet<Node> FreeNode { get; protected set; }
    /// <summary>Nodos contenidos en el grafo</summary>
    public Dictionary<Coordenates, Node> TableNode { get; protected set; }
    public Dictionary<(int, int), int> CoordValor { get; protected set; }
    public TableTriangular(Token token)
    {
        this.PlayNode = new HashSet<Node>();
        this.FreeNode = new HashSet<Node>();
        this.TableNode = new Dictionary<Coordenates, Node>();
        this.CoordValor = new Dictionary<(int, int), int>();
        Node node = this.CreateNode(new (int, int)[] { (0, 0), (2, 0), (1, -1) });
        node.ValueToken = token;
        this.Expand(node);
        this.PlayNode.Add(node);
    }
    /// <summary>Expandir un nodo</summary>
    /// <param name="node">Nodo que se desea expandir</param>
    protected override void Expand(Node node)
    {
        (int, int)[] listCoord = (node as NodeGeometry)!.Ubication.Coord;
        for (int i = 0; i < node.Conections.Length; i++)
        {
            (int, int)[] coordenates = Triangular(listCoord[i], listCoord[(i + 1 >= 3) ? i + 1 - 3 : i + 1], listCoord[(i + 2 >= 3) ? i + 2 - 3 : i + 2]);
            Coordenates aux = new Coordenates(coordenates);
            if (!this.TableNode.ContainsKey(aux))
            {
                this.FreeTable(this.CreateNode(coordenates));
            }
            int j = 0;
            while (node.Conections[j] != null)
            {
                j++;
                if (j == node.Conections.Length) break;
            }
            if (j == node.Conections.Length) break;
            this.UnionNode(node, this.TableNode[aux], j);
        }
    }
    public (int, int)[] Triangular((int, int) coord1, (int, int) coord2, (int, int) coord3)
    {
        bool nw = coord2.Item1 > coord1.Item1 && coord2.Item2 > coord1.Item2;
        bool w = coord2.Item2 == coord1.Item2 && coord2.Item1 > coord1.Item1;
        bool sw = coord2.Item1 > coord1.Item1 && coord2.Item2 < coord1.Item2;
        if (nw)
        {
            if (coord3.Item1 > coord1.Item1) return new (int, int)[] { coord1, (coord2.Item1 - 2, coord2.Item2), coord2 };
            else return new (int, int)[] { coord1, coord2, (coord1.Item1 + 2, coord1.Item2) };
        }
        else if (w)
        {
            if (coord3.Item2 > coord1.Item2) return new (int, int)[] { coord1, coord2, (coord3.Item1, coord3.Item2 - 2) };
            return new (int, int)[] { coord1, coord2, (coord3.Item1, coord3.Item2 + 2) };
        }
        else if (sw)
        {
            if (coord3.Item1 < coord1.Item1) return new (int, int)[] { coord1, (coord1.Item1 + 2, coord1.Item1), coord2 };
            else return new (int, int)[] { coord1, coord2, (coord2.Item1 - 2, coord2.Item2) };
        }
        else return Triangular(coord2, coord1, coord3);
    }
    /// <summary>Crear un nodo</summary>
    /// <param name="coordenates">Cooredenadas de la ficha</param>
    /// <returns>Nuevo nodo</returns>
    protected NodeGeometry CreateNode((int, int)[] coordenates)
    {
        NodeGeometry node = new NodeGeometry(coordenates);
        this.TableNode.Add(node.Ubication, node);
        return node;
    }
}
