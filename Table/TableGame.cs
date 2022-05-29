namespace Table;

public abstract class TableGame
{
    /// <summary>Nodos que contienen una ficha</summary>
    public abstract HashSet<Node> PlayNode { get; protected set; }
    /// <summary>Nodos disponibles para jugar</summary>
    public abstract HashSet<Node> FreeNode { get; protected set; }
    /// <summary>Nodos contenidos en el grafo</summary>
    public abstract List<Node> TableNode { get; protected set; }
    /// <summary>Expandir un nodo</summary>
    /// <param name="node">Nodo que se desea expandir</param>
    protected abstract void Expand(Node node);
    /// <summary>Colocar una ficha en el tablero</summary>
    /// <param name="node">Nodo por el cual se juega la ficha</param>
    public void PlayTable(Node node, Token token)
    {
        node.ValueToken = token;
        this.Expand(node);
        this.PlayNode.Add(node);
        FreeNode.Remove(node);
    }
    /// <summary>Indica que un nodo esta libre para jugar</summary>
    /// <param name="node">Nodo para realizar la operacion</param>
    public void FreeTable(Node node)
    {
        if (this.PlayNode.Contains(node)) return;
        this.FreeNode.Add(node);
    }
    /// <summary>Recorrer el grafo</summary>
    /// <param name="node">Nodo inicial</param>
    /// <param name="ind"Arista del nodo al se quiere ir</param>
    /// <returns>Node vecino al cual se realiza el movimiento</returns>
    public Node MoveTable(Node node, int ind)
    {
        return node.Conections[ind];
    }
    /// <summary>Unir dos nodos</summary>
    /// <param name="right">Nodo para realizar la union</param>
    /// <param name="left">Nodo para realizar la union</param>
    /// <param name="ind">Arista por la que los nodos se conectan</param>
    protected void UnionNode(Node right, Node left, int ind)
    {
        right.Conections[ind] = left;
        left.Conections[ind] = right;
    }
}