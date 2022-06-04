using Domino.Players;
using Domino.Boards;
using Domino.Rules;
using Domino.Tokens;
using Domino.Utils;

namespace Domino.Games;

public delegate IDictionary<BasePlayer, IEnumerable<IToken>> DealTokens(IEnumerable<IToken> tokens, IEnumerable<BasePlayer> players);

public enum GameState {
    LOADING,
    IN_PROGRESS,
    FINISHED
}

public abstract class BaseGame {
    public GameState CurrentState { get; protected set; }
    // Players
    public IEnumerable<BasePlayer> Players { get; }
    public IDictionary<BasePlayer, IEnumerable<IToken>> PlayerTokens { get; protected set; }
    // Board
    public BaseBoard Board { get; }
    // Rules
    public IRuleSet<IRule> RuleSet { get; }
    protected List<IToken> tokens = new List<IToken>();

    public BaseGame(
        IEnumerable<BasePlayer> players, 
        IRuleSet<IRule> rules,
        BaseBoard board
    ) {
        this.CurrentState = GameState.LOADING;
        this.Players = players;
        this.Board = board;
        this.RuleSet = rules;
        this.PlayerTokens = new Dictionary<BasePlayer, IEnumerable<IToken>>();
    }

    public abstract void Start();
}

public interface ITurnableGame<TPlayer> {
    public TPlayer CurrentPlayer { get; }

    public TPlayer NextPlayer();
    public TPlayer PreviousPlayer();
}


public class DominoGame : BaseGame, ITurnableGame<BasePlayer> {
    public BasePlayer CurrentPlayer { get; protected set; }

    public DominoGame(
        IEnumerable<BasePlayer> players,
        IRuleSet<IRule> rules,
        DominoBoard board,
        DealTokens tokenDealer,
        int maxTokenValue,
        int maxTokensPerPlayer
    ): base(players, rules, board) {
        this.CurrentPlayer = this.Players.First();

        List<int[]> tokenValuesList = new List<int[]>();

        Combinatorics.GenerateVariations(2, new int[maxTokenValue], 0, tokenValuesList);
        
        foreach(int[] values in tokenValuesList)
            this.tokens.Add(new DominoToken(values[0], values[1]));

        this.PlayerTokens = tokenDealer(this.tokens, this.Players);

        this.CurrentState = GameState.IN_PROGRESS;
    }

    public BasePlayer NextPlayer() {
        throw new NotImplementedException();
    }
    public BasePlayer PreviousPlayer() {
        throw new NotImplementedException();
    }
    public override void Start()
    {
        throw new NotImplementedException();
    }
}
