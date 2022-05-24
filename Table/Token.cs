namespace Table;

public class Token
{
    public IEnumerable<int> Values { get; private set; }
    public int CantValues { get { return Values.Count(); } }
    public int Score { get { return Values.Sum(); } }
    public Token(IEnumerable<int> values)
    {
        this.Values = values;
    }
}
