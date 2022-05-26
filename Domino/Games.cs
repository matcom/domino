using Domino.Players;
using Domino.Boards;
using Domino.Rules;
using Domino.Tokens;
using Domino.Utils;

namespace Domino.Games;

public interface ITokenDealer {
    public void DealTokens(
        IList<BaseToken> tokens, 
        IEnumerable<BasePlayer> players,
        int maxTokensPerPlayer
    );
}

public abstract class BaseGame {
    // Players
    public IEnumerable<BasePlayer> Players { get; }
    // Board
    public BaseBoard Board { get; }
    // Rules
    public IEnumerable<BaseRule> RuleSet { get; }

    public BaseGame(
        IEnumerable<BasePlayer> players, 
        IEnumerable<BaseRule> rules,
        BaseBoard board
    ) {
        this.Players = players;
        this.Board = board;
        this.RuleSet = rules;
    }
}

public class DominoGame : BaseGame, ITokenDealer {
    public DominoGame(
        IEnumerable<BasePlayer> players,
        IEnumerable<BaseRule> rules,
        DominoBoard board,
        int maxTokenValue,
        int maxTokensPerPlayer
    ) : base(players, rules, board) {
        List<int[]> tokenValuesList = new List<int[]>();
        List<BaseToken> tokenList = new List<BaseToken>();

        Combinatorics.GenerateVariations(2, new int[maxTokenValue], 0, tokenValuesList);
        
        foreach(int[] values in tokenValuesList)
            tokenList.Add(new DominoToken(values[0], values[1]));

        this.DealTokens(tokenList, players, maxTokensPerPlayer);
    }

    public void DealTokens(IList<BaseToken> tokens, IEnumerable<BasePlayer> players, int maxTokensPerPlayer) {
        Random randObject = new Random();

        foreach(BasePlayer player in players) {
            BaseToken[] playerTokens = new BaseToken[maxTokensPerPlayer];

            for(int i = 0; i < playerTokens.Length; i++) {
                int tokenIndex = randObject.Next(tokens.Count());
                playerTokens[i] = tokens[tokenIndex];
                tokens.RemoveAt(tokenIndex);
            }

            player.AvailableTokens = playerTokens.ToList<BaseToken>();
        }
    }
}
