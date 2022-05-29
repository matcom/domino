namespace Table;

public class TableSquare : TableGeometry
{
    public TableSquare(Token token, (int, int)[] coordenates) : base(token, coordenates)
    {

    }
    protected override void Expand(Node node)
    {
        NodeGeometry geometry = (node as NodeGeometry)!;
        if (geometry == null) return;
        (int, int) center = FindCenter(geometry);
        (int, int)[] expand = FindCenterExpand(center);
        for (int i = 0; i < expand.Length; i++)
        {
            AsignCoordenates(geometry, ExpandGeometry(new (int, int)[] { expand[i] }));
        }
    }
    protected override (int, int)[] ExpandGeometry((int, int)[] coordenates)
    {
        (int, int)[] expand = new (int, int)[4];
        int[] x = new int[] { -1, 1, 1, -1 };
        int[] y = new int[] { -1, -1, 1, 1 };
        for (int i = 0; i < expand.Length; i++)
        {
            expand[i] = ((coordenates[0].Item1 + x[i]) / 2, (coordenates[0].Item2 + y[i]) / 2);
        }
        return expand;
    }
    /// <summary>Expandir el nodo a partir de las coorenadas de su centro</summary>
    /// <param name="coordenates">Coordenadas del nodo</param>
    /// <returns>Coordenadas de los nodos expandidos</returns>
    protected (int, int)[] FindCenterExpand((int, int) coordenates)
    {
        (int, int)[] expand = new (int, int)[4];
        int[] x = new int[] { -2, 0, 2, 0 };
        int[] y = new int[] { 0, -2, 0, 2 };
        for (int i = 0; i < expand.Length; i++)
        {
            expand[i] = (coordenates.Item1 + x[i], coordenates.Item2 + y[i]);
        }
        return expand;
    }
    /// <summary>Buscar las coordenadas del centro del nodo</summary>
    /// <param name="node">Nodo</param>
    /// <returns>Coordenadas del centro</returns>
    protected (int, int) FindCenter(NodeGeometry node)
    {
        int x = 0; int y = 0;
        for (int i = 1; i < node.Ubication.Coord.Length; i++)
        {
            if (node.Ubication.Coord[i].Item2 == node.Ubication.Coord[i - 1].Item2)
            {
                x = (node.Ubication.Coord[i].Item1 + node.Ubication.Coord[i - 1].Item1);
            }
            if (node.Ubication.Coord[i].Item1 == node.Ubication.Coord[i - 1].Item1)
            {
                y = (node.Ubication.Coord[i].Item2 + node.Ubication.Coord[i - 1].Item2);
            }
        }
        return (x, y);
    }
}