namespace DominoEngine;

public abstract class Piece {}

public class IntDominoPiecesGenerator
{
    List<DominoPiece<Int>> pieces = new List<DominoPiece<Int>>();
    public IntDominoPiecesGenerator(int n)
    {
        Generator(n, 0, new Int[n], new bool[n]);
    }

    public void Generator(int n, int init, Int[] arr, bool[] mask)
    {
        if(true)

        for (int i = init; i < n; i++)
        {
            if (!mask[i])
            {
                arr[init] = new Int(i);
                mask[i] = true;
                Generator(n, init++, arr, mask);
                mask[i] = false;
                arr[init] = null!;
            }
        }
    }
}

public class DominoPiece<T> : Piece where T : ILinkerable
{
    public Dictionary<T, bool> elem;
    public DominoPiece(T[] arr)
    {
        elem = new Dictionary<T, bool>();

        foreach (var item in arr)
        {
            if (!elem.ContainsKey(item)) 
                elem.Add(item, true);
        }
    }

    public List<T> AskAvailables()
    {
        List<T> availables = new List<T>();

        foreach (var item in elem)
        {
            if (item.Value) availables.Add(item.Key);
        }

        return availables;
    }

    public void MarkAsUsed(T e)
    {
        elem[e] = false;
    }

    public static void Match(DominoBoard<T> state, Tuple<DominoPiece<T>,DominoPiece<T>,T,T> tuple)
    {
        tuple.Item1.MarkAsUsed(tuple.Item3);
        tuple.Item2.MarkAsUsed(tuple.Item4);
        state.FindPiece(tuple.Item1).Push(tuple.Item2);
    }
}

