using System.Collections;

namespace DominoEngine;

public class Tournament<T> : IEnumerable<Game<T>>
{
    public Tournament()
    {
    }

    public IEnumerator<Game<T>> GetEnumerator()
    {
        throw new NotImplementedException();
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return GetEnumerator();
    }
}
