namespace Table;

public abstract class Node
{
    public abstract Token ValueToken { get; set; }
    public abstract Node[] Conections { get; set; }
}