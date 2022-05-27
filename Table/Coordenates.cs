namespace Table;

public class Coordenates
{
    public (int, int)[] Coord { get; private set; }
    private (int, int)[] ListCoord { get; set; }
    public Coordenates((int, int)[] list)
    {
        (int, int)[] listCopy = new (int, int)[list.Length];
        (int, int)[] listCopy1 = new (int, int)[list.Length];
        Array.Copy(list, listCopy, list.Length);
        Array.Copy(list, listCopy1, list.Length);
        Array.Sort(listCopy);
        this.ListCoord = listCopy;
        this.Coord = listCopy1;
    }
    public override bool Equals(object? obj)
    {
        Coordenates aux = (obj as Coordenates)!;
        if (aux == null) return false;
        bool equal = true;
        for (int i = 0; i < this.ListCoord.Length; i++)
        {
            equal = equal && this.ListCoord[i] == aux.ListCoord[i];
        }
        return equal;
    }
    public override int GetHashCode()
    {
        return this.ListCoord[0].GetHashCode();
    }
}