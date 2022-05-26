using System.Collections.Generic;

namespace Domino.Tokens;


interface IPlaceable {
    public void Place();
}

public interface IMovable {
    public void Move();
}

public interface IMultiValue<T> {
    public IEnumerable<T> Values();
}

public abstract class BaseToken {
    // Value
    public abstract double Value();
}

public class DominoToken : BaseToken, IPlaceable, IMultiValue<int> {
    readonly List<int> values;

    public DominoToken(int value1, int value2) {
        this.values = new int[]{value1, value2}.ToList<int>();
    }

    public IEnumerable<int> Values() {
        return this.values;
    }
    public override double Value()
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
