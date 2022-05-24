namespace Table;

public class TableGame
{
    /// <summary>Nodos que contienen una ficha</summary>
    public HashSet<NodeGame> PlayNode { get; private set; }
    /// <summary>Nodos disponibles para jugar</summary>
    public HashSet<NodeGame> FreeNode { get; private set; }
    /// <summary>Nodos contenidos en el grafo</summary>
    public List<NodeGame> TableNode { get; private set; }
    public TableGame(Token token)
    {

        this.PlayNode = new HashSet<NodeGame>();
        this.FreeNode = new HashSet<NodeGame>();
        this.TableNode = new List<NodeGame>();
        NodeGame node = CreateNode(token.CantValues);
        node.ValueToken = token;
        Expand(node);
        this.PlayNode.Add(node);
    }
    /// <summary>Colocar una ficha en el tablero</summary>
    /// <param name="node">Nodo por el cual se juega la ficha</param>
    public void PlayTable(NodeGame node, Token token)
    {
        node.ValueToken = token;
        this.Expand(node);
        this.PlayNode.Add(node);
        FreeNode.Remove(node);
    }
    /// <summary>Recorrer el grafo</summary>
    /// <param name="node">Nodo inicial</param>
    /// <param name="ind"Arista del nodo al se quiere ir</param>
    /// <returns>Node vecino al cual se realiza el movimiento</returns>
    public NodeGame MoveTable(NodeGame node, int ind)
    {
        return node.Conections[ind];
    }
    /// <summary>Unir dos nodos</summary>
    /// <param name="right">Nodo para realizar la union</param>
    /// <param name="left">Nodo para realizar la union</param>
    /// <param name="ind">Arista por la que los nodos se conectan</param>
    protected void UnionNode(NodeGame right, NodeGame left, int ind)
    {
        right.Conections[ind] = left;
        left.Conections[ind] = right;
    }
    /// <summary>Indica que un nodo esta libre para jugar</summary>
    /// <param name="node">Nodo para realizar la operacion</param>
    protected void FreeTable(NodeGame node)
    {
        if (this.PlayNode.Contains(node)) return;
        this.FreeNode.Add(node);
    }
    /// <summary>Expandir un nodo</summary>
    /// <param name="node">Nodo que se desea expandir</param>
    protected virtual void Expand(NodeGame node)
    {
        for (int i = 0; i < node.Conections.Length; i++)
        {
            if (node.Conections[i] == null)
            {
                this.UnionNode(node, CreateNode(node.Conections.Length), i);
            }
            FreeTable(node.Conections[i]);
        }
    }
    /// <summary>Crear un nodo</summary>
    /// <param name="n">Numero de aristas</param>
    /// <returns>Nuevo nodo</returns>
    protected NodeGame CreateNode(int n)
    {
        NodeGame node = new NodeGame(n);
        this.TableNode.Add(node);
        return node;
    }
}
