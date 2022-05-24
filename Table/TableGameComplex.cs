namespace Table;

public class TableGameComplex : TableGame
{
    public TableGameComplex(Token token) : base(token)
    {

    }
    protected override void Expand(NodeGame node)
    {
        base.Expand(node);
        for (int i = 1; i < node.Conections.Length; i++)
        {
            if (node.Conections[i - 1].FirstConectionFree != -1 && node.Conections[i].FirstConectionFree != -1)
            {
                ExpandComplex(node.Conections[i - 1], node.Conections[i], i - 1, i);
            }
        }
        if (node.Conections[node.Conections.Length - 1].FirstConectionFree != -1 && node.Conections[0].FirstConectionFree != -1)
        {
            ExpandComplex(node.Conections[node.Conections.Length - 1], node.Conections[0], node.Conections.Length - 1, 0);
        }
    }
    private void ExpandComplex(NodeGame node1, NodeGame node2, int conection1, int conection2)
    {
        if (conection1 < conection2)
        {
            if (conection2 - conection1 > 1) ExpandConection(node1, node2, conection1);
            else ExpandConection(node2, node1, conection2);
        }
        else
        {
            if (conection1 - conection2 > 1) ExpandConection(node2, node1, conection2);
            else ExpandConection(node1, node2, conection1);
        }
    }
    private void ExpandConection(NodeGame nodeStart, NodeGame nodeStop, int conectionStart)
    {
        int bucle = nodeStart.Conections.Length;
        NodeGame nodeAux = nodeStart;
        int conectionAux = conectionStart + 1;
        for (int i = 0; i < 2 * bucle / (bucle - 2) - 3; i++)
        {
            if (conectionAux == bucle) conectionAux = 0;
            this.UnionNode(nodeAux, CreateNode(nodeStart.Conections.Length), conectionAux);
            nodeAux = nodeAux.Conections[conectionAux];
            conectionAux++;
        }
        if (conectionAux == bucle) conectionAux = 0;
        this.UnionNode(nodeAux, nodeStop, conectionAux);
    }
}