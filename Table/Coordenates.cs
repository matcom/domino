namespace Table;

public class Coordenates
{
    public (int, int)[] ListCoord { get; private set; }
    public Coordenates((int, int)[] list)
    {
        (int, int)[] listCopy = new (int, int)[list.Length];
        Array.Copy(list, listCopy, list.Length);
        // Array.Sort(listCopy);
        this.ListCoord = listCopy;
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