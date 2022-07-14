using DominoEngine;

namespace Rules;

public class ClassicGenerator : IGenerator<int>
{
    private readonly int _number;

    public ClassicGenerator(int number) {
        _number = number;
    }

    IEnumerable<Token<int>> IGenerator<int>.Generate() {
        var tokens = new List<Token<int>>();

        for (var i = 0; i < _number; i++)
            for (var j = i; j < _number; j++)
                tokens.Add(new Token<int>(i, j));

        return tokens;
    }
}

public class SumPrimeGenerator : IGenerator<int>
{
    private readonly int _number;

    public SumPrimeGenerator(int number) {
        _number = number;
    }

    IEnumerable<Token<int>> IGenerator<int>.Generate() {
        var tokens = new List<Token<int>>();

        for (var i = 0; i < _number; i++)
            for (var j = i; j < _number; j++)
                tokens.Add(new Token<int>(i, j));

        return tokens.Where(token => IsPrime(token.Head + token.Tail)).ToList();
    }

    private static bool IsPrime(int a) {
        for (var i = 2; i <= Math.Sqrt(a); i++) {
            if (a % i is 0) return false;
        }
        return true;
    }
}
