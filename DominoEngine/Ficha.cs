namespace DominoEngine; 

public record Ficha<T>(T Head, T Tail) : IEquatable<Ficha<T>>
{
    public bool Equal(Ficha<T> ficha)
    {
        return (this.Head!.Equals(ficha.Head) && this.Tail!.Equals(ficha.Tail)) || (this.Head!.Equals(ficha.Tail) && this.Tail!.Equals(ficha.Head));
    }
    
    public override string ToString()
    {
        return $"({Head!.ToString()}|{Tail!.ToString()})";
    }
}