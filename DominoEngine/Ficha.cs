using System.Collections;

namespace DominoEngine;


public record Ficha<T>(T Head, T Tail)
{
    bool IEquatable<Ficha<T>>.Equals(Ficha<T>? ficha)
    {
        return (Head!.Equals(ficha!.Head) && Tail!.Equals(ficha.Tail)) || (Head!.Equals(ficha.Tail) && Tail!.Equals(ficha.Head));
    }

    public T Other(T other)
    {
        return (other!.Equals(Head)) ? Tail : Head;
    }
}
