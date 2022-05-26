namespace Table;

public abstract class TableGame
{
    /// <summary>Nodos que contienen una ficha</summary>
    public abstract HashSet<Node> PlayNode { get; protected set; }
    /// <summary>Nodos disponibles para jugar</summary>
    public abstract HashSet<Node> FreeNode { get; protected set; }
    /// <summary>Nodos contenidos en el grafo</summary>
    public abstract List<Node> TableNode { get; protected set; }
    /// <summary>Colocar una ficha en el tablero</summary>
    /// <param name="node">Nodo por el cual se juega la ficha</param>
    public abstract void PlayTable(Node node, Token token);
    /// <summary>Indica que un nodo esta libre para jugar</summary>
    /// <param name="node">Nodo para realizar la operacion</param>
    public abstract void FreeTable(Node node);
    /// <summary>Expandir un nodo</summary>
    /// <param name="node">Nodo que se desea expandir</param>
    protected abstract void Expand(Node node);
}