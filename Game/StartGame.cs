using Table;

namespace Game;

public class StartGame
{
    public List<Token> Tokens { get; private set; }
    public StartGame(int players, int n, int k)
    {
        this.Tokens = CreateTokens(n, k);
    }
    private List<Token> CreateTokens(int n, int k)
    {
        List<Token> tokens = new List<Token>();
        Combination(0, 0, n, k, new int[n], tokens);
        return tokens;
    }
    private void Combination(int ind, int start, int n, int k, int[] aux, List<Token> tokens)
    {
        if (ind == n)
        {
            int[] aux1 = new int[n];
            Array.Copy(aux, aux1, n);
            tokens.Add(new Token(aux1));
            return;
        }
        for (int i = start; i < k; i++)
        {
            aux[ind] = i;
            Combination(ind + 1, i, n, k, aux, tokens);
        }
    }
}