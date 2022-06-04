using System.Collections.Generic;

namespace Domino.Tokens;


public interface IPlaceableToken {
    public void Place();
}

public interface IMovableToken {
    public void Move();
}


public interface IMultiValueToken<T> {
    public IEnumerable<T> Values();
}

public interface IToken {
    // Value
    public abstract double Value();
}

public class DominoToken : IToken, IPlaceableToken, IMultiValueToken<int> {
    readonly List<int> values;

    public DominoToken(int value1, int value2) {
        this.values = new int[]{value1, value2}.ToList<int>();
    }

    public IEnumerable<int> Values() {
        return this.values;
    }
    public double Value()
    {
        double value = 0;

        foreach (int elem in this.Values())
            value += elem;

        return value;
    }
    public void Place() {
        throw new NotImplementedException();
    }

    public bool IsDouble() {
        return this.values[0] == this.values[1];
    }
}
