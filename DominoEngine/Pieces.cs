namespace DominoEngine;

public abstract class Piece { }

public abstract class DominoPiecesGenerator<T> where T : ILinkerable
{
    List<DominoPiece<T>> pieces = new List<DominoPiece<T>>();
    public DominoPiecesGenerator(int number_of_sides, int max)
    {
        Generator(number_of_sides, max, 0, new List<T>(), new bool[max]);
    }

    public void Generator(int number_of_sides, int max, int asigned, List<T> sides, bool[] mask)
    {
        if (number_of_sides == asigned)
        {
            DominoPiece<T> new_piece = new DominoPiece<T>(sides);
            pieces.Add(new_piece);
            return;
        }

        for (int i = 0; i < max; i++)
        {
            if (!mask[i])
            {
                mask[i] = true;
                T new_side = Create(i);
                sides.Add(new_side);
                Generator(number_of_sides, max, asigned++, sides, mask);
                sides.Remove(new_side);
                mask[i] = false;
            }
        }
    }

    public abstract T Create(int a);
}

public class IntDominoPieceGenerator : DominoPiecesGenerator<Int>
{
    public IntDominoPieceGenerator(int number_of_sides, int max) : base(number_of_sides, max)
    {
    }

    public override Int Create(int a)
    {
        return new Int(a);
    }
}

public class DominoPiece<T> : Piece where T : ILinkerable
{
    public Dictionary<T, bool> elem;
    public DominoPiece(IList<T> sides)
    {
        elem = new Dictionary<T, bool>();

        foreach (var item in sides)
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

    public static void Match(DominoBoard<T> state, Tuple<DominoPiece<T>, DominoPiece<T>, T, T> tuple)
    {
        tuple.Item1.MarkAsUsed(tuple.Item3);
        tuple.Item2.MarkAsUsed(tuple.Item4);
        state.FindPiece(tuple.Item1).Push(tuple.Item2);
    }
}

