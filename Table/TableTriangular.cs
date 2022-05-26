namespace Table;

public class a
{

}
// namespace Table;

// public class TableTriangular : TableGame
// {
//     /// <summary>Nodos que contienen una ficha</summary>
//     public override HashSet<Node> PlayNode { get; protected set; }
//     /// <summary>Nodos disponibles para jugar</summary>
//     public override HashSet<Node> FreeNode { get; protected set; }
//     /// <summary>Nodos contenidos en el grafo</summary>
//     public List<Node> TableNode { get; protected set; }
//     public TableTriangular(Token token)
//     {
//         this.PlayNode = new HashSet<Node>();
//         this.FreeNode = new HashSet<Node>();
//         this.TableNode = new List<Node>();
//         INode<Token, NodeGeometry> node = CreateNode(new (int, int)[] { (0, 0), (0, 2), (1, 1) });
//         node.ValueToken = token;
//         Expand(node);
//         this.PlayNode.Add(node);
//     }
//     /// <summary>Colocar una ficha en el tablero</summary>
//     /// <param name="node">Nodo por el cual se juega la ficha</param>
//     public void PlayTable(INode<Token, Node> node, Token token)
//     {
//         node.ValueToken = token;
//         this.Expand(node);
//         this.PlayNode.Add(node);
//         FreeNode.Remove(node);
//     }
//     /// <summary>Indica que un nodo esta libre para jugar</summary>
//     /// <param name="node">Nodo para realizar la operacion</param>
//     public void FreeTable(INode<Token, Node> node)
//     {
//         if (this.PlayNode.Contains(node)) return;
//         this.FreeNode.Add(node);
//     }
//     /// <summary>Recorrer el grafo</summary>
//     /// <param name="node">Nodo inicial</param>
//     /// <param name="ind"Arista del nodo al se quiere ir</param>
//     /// <returns>Node vecino al cual se realiza el movimiento</returns>
//     public Node MoveTable(Node node, int ind)
//     {
//         return node.Conections[ind];
//     }
//     /// <summary>Unir dos nodos</summary>
//     /// <param name="right">Nodo para realizar la union</param>
//     /// <param name="left">Nodo para realizar la union</param>
//     /// <param name="ind">Arista por la que los nodos se conectan</param>
//     protected void UnionNode(Node right, Node left, int ind)
//     {
//         right.Conections[ind] = left;
//         left.Conections[ind] = right;
//     }
//     /// <summary>Expandir un nodo</summary>
//     /// <param name="node">Nodo que se desea expandir</param>
//     public override void Expand(Node node)
//     {
//         for (int i = 0; i < node.Conections.Length; i++)
//         {
//             if (node.Conections[i] == null)
//             {
//                 this.UnionNode((node as Node)!, (CreateNode(node.Conections.Length) as Node)!, i);
//             }
//             this.FreeTable(node.Conections[i]);
//         }
//     }
//     /// <summary>Crear un nodo</summary>
//     /// <param name="coordenates">Cooredenadas de la ficha</param>
//     /// <returns>Nuevo nodo</returns>
//     protected Node CreateNode((int, int)[] coordenates)
//     {
//         Node node = new NodeGeometry(coordenates);
//         this.TableNode.Add(node);
//         return node;
//     }
// }
