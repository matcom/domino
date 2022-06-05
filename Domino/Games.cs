using Domino.Players;
using Domino.Boards;
using Domino.Rules;
using Domino.Tokens;
using Domino.Utils;
using Domino.Moves;

namespace Domino.Games;

public delegate IDictionary<BasePlayer<BaseMove>, IEnumerable<IToken>> DealTokens(IEnumerable<IToken> tokens, IEnumerable<BasePlayer<BaseMove>> players);

public enum GameState {
    LOADING,
    IN_PROGRESS,
    FINISHED
}

public abstract class BaseGame {
    public GameState CurrentState { get; protected set; }
    // Players
    public IEnumerable<BasePlayer<BaseMove>> Players { get; }
    public IDictionary<BasePlayer<BaseMove>, IEnumerable<IToken>> PlayerTokens { get; protected set; }
    // Board
    public BaseBoard Board { get; }
    // Rules
    public ITokenRule<IToken> TokenRules { get; }
    protected List<IToken> tokens = new List<IToken>();

    public BaseGame(
        IEnumerable<BasePlayer<BaseMove>> players, 
        ITokenRule<IToken> tokenRules,
        BaseBoard board
    ) {
        this.CurrentState = GameState.LOADING;
        this.Players = players;
        this.Board = board;
        this.TokenRules = tokenRules;
        this.PlayerTokens = new Dictionary<BasePlayer<BaseMove>, IEnumerable<IToken>>();
    }

    public abstract IEnumerable<IToken>  AvailableMoves();
    public abstract bool WinAchieved();
    public abstract IEnumerable<BasePlayer<BaseMove>> GetWinners();
    public abstract void Start();
}

public interface ITurnableGame<TPlayer> {
    public TPlayer CurrentPlayer { get; }

    public TPlayer NextPlayer();
    public TPlayer PreviousPlayer();
}


public class DominoGame : BaseGame, ITurnableGame<BasePlayer<BaseMove>> {
    public BasePlayer<BaseMove> CurrentPlayer { get; protected set; }

    public DominoGame(
        IEnumerable<BasePlayer<BaseMove>> players,
        ITokenRule<IToken> tokenRules,
        DominoBoard board,
        DealTokens tokenDealer,
        int maxTokenValue,
        int maxTokensPerPlayer
    ): base(players, tokenRules, board) {
        this.CurrentPlayer = this.Players.First();

        List<int[]> tokenValuesList = new List<int[]>();

        Combinatorics.GenerateVariations(2, new int[maxTokenValue], 0, tokenValuesList);
        
        foreach(int[] values in tokenValuesList)
            this.tokens.Add(new DominoToken(values[0], values[1]));

        this.PlayerTokens = tokenDealer(this.tokens, this.Players);

        this.Start();
    }

    public BasePlayer<BaseMove> NextPlayer() {
        throw new NotImplementedException();
    }
    public BasePlayer<BaseMove> PreviousPlayer() {
        throw new NotImplementedException();
    }
    public override IEnumerable<IToken> AvailableMoves()
    {
        return this.PlayerTokens[this.CurrentPlayer]
            .Where(
                token => this.TokenRules.IsValid(token, this.Board.Table)
            );
    }
    public override bool WinAchieved()
    {
        throw new NotImplementedException();
    }
    public override IEnumerable<BasePlayer<BaseMove>> GetWinners()
    {
        throw new NotImplementedException();
    }
    public override void Start()
    {
        this.CurrentState = GameState.IN_PROGRESS;

        while (!this.WinAchieved()) {
            this.Board.AddMove(this.CurrentPlayer.Strategy.MakeMove(this.AvailableMoves(), this.Board));
        }

        this.CurrentState = GameState.FINISHED;
    }
}
