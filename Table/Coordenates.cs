namespace Table;

public class Coordenates
{
    /// <summary>Lista de coordenadas</summary>
    public (int, int)[] Coord { get; private set; }
    /// <summary>Lista de coordenadas ordenadas</summary>
    private (int, int)[] _listCoord { get; set; }
    public Coordenates((int, int)[] list)
    {
        (int, int)[] listCopy = new (int, int)[list.Length];
        (int, int)[] listCopy1 = new (int, int)[list.Length];
        Array.Copy(list, listCopy, list.Length);
        Array.Copy(list, listCopy1, list.Length);
        Array.Sort(listCopy);
        this._listCoord = listCopy;
        this.Coord = listCopy1;
    }
    public override bool Equals(object? obj)
    {
        Coordenates aux = (obj as Coordenates)!;
        if (aux == null) return false;
        bool equal = true;
        for (int i = 0; i < this._listCoord.Length; i++)
        {
            equal = equal && this._listCoord[i] == aux._listCoord[i];
        }
        return equal;
    }
    public override int GetHashCode()
    {
        return this._listCoord[0].GetHashCode();
    }
}