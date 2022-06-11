using System.Collections.Generic;

namespace Domino.Tokens;

public class TokenComparer : IComparer<IToken> {
    public int Compare(IToken x, IToken y) {
        if (x.Value() > y.Value())
            return 1;
        if (x.Value() == y.Value())
            return 0;
        return -1;
    }
}


public interface IPlaceableToken {
    public void Place();
}

public interface IMovableToken {
    public void Move();
}

public interface IToken {
    public bool IsMultiValue { get; }
    // Value
    public double Value();
    public IEnumerable<double> Values();
}

public class DominoToken : IToken, IPlaceableToken {
    readonly List<double> values;
    public bool IsMultiValue { get { return true; } }

    public DominoToken(double value1, double value2) {
        this.values = new double[]{value1, value2}.ToList<double>();
    }

    public IEnumerable<double> Values() {
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
