namespace Table;

public class TableHexagonal : TableGeometry
{
    public TableHexagonal(Token token, (int, int)[] coordenates) : base(token, coordenates)
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
        (int, int)[] expand = new (int, int)[6];
        int[] x = new int[] { -2, -1, 1, 2, 1, -1 };
        int[] y = new int[] { 0, -1, -1, 0, 1, 1 };
        for (int i = 0; i < expand.Length; i++)
        {
            expand[i] = (coordenates[0].Item1 + x[i], coordenates[0].Item2 + y[i]);
        }
        return expand;
    }
    /// <summary>Expandir el nodo a partir de las coorenadas de su centro</summary>
    /// <param name="coordenates">Coordenadas del nodo</param>
    /// <returns>Coordenadas de los nodos expandidos</returns>
    protected (int, int)[] FindCenterExpand((int, int) coordenates)
    {
        (int, int)[] expand = new (int, int)[6];
        int[] x = new int[] { -3, 0, 3, 3, 0, -3 };
        int[] y = new int[] { -1, -2, -1, 1, 2, 1 };
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
        for (int i = 1; i < node.Conections.Length; i++)
        {
            if (node.Ubication.Coord[i].Item2 == node.Ubication.Coord[i - 1].Item2)
            {
                x = (node.Ubication.Coord[i].Item1 + node.Ubication.Coord[i - 1].Item1) / 2;
            }
            if (i == 1) continue;
            if (node.Ubication.Coord[i].Item1 == node.Ubication.Coord[i - 2].Item1)
            {
                y = (node.Ubication.Coord[i].Item2 + node.Ubication.Coord[i - 2].Item2) / 2;
            }
        }
        return (x, y);
    }
}
