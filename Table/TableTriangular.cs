namespace Table;

public class TableTriangular : TableGeometry
{
    public TableTriangular(Token token, (int, int)[] coordenates) : base(token, coordenates)
    {

    }
    protected override void Expand(Node node)
    {
        NodeGeometry geometry = (node as NodeGeometry)!;
        if (geometry == null) return;
        for (int i = 0; i < node.Conections.Length; i++)
        {
            (int, int)[] coordenates = ExpandGeometry(CircularArray<(int, int)>.Circular(geometry.Ubication.Coord, i));
            AsignCoordenates(node, coordenates);
        }
    }
    protected override (int, int)[] ExpandGeometry((int, int)[] coordenates)
    {
        (int, int) coord1 = coordenates[0];
        (int, int) coord2 = coordenates[1];
        (int, int) coord3 = coordenates[2];
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
        else return ExpandGeometry(new (int, int)[] { coord2, coord1, coord3 });
    }
}
