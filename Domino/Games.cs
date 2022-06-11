using Domino.Players;
using Domino.Boards;
using Domino.Rules;
using Domino.Tokens;
using Domino.Utils;
using Domino.Moves;

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
    public ITokenRule<IToken> TokenRules { get; }
    protected List<IToken> tokens = new List<IToken>();

    public BaseGame(
        IEnumerable<BasePlayer> players, 
        ITokenRule<IToken> tokenRules,
        BaseBoard board
    ) {
        this.CurrentState = GameState.LOADING;
        this.Players = players;
        this.Board = board;
        this.TokenRules = tokenRules;
        this.PlayerTokens = new Dictionary<BasePlayer, IEnumerable<IToken>>();
    }

    public abstract IEnumerable<BaseMove>  AvailableMoves();
    public abstract bool WinAchieved();
    public abstract IEnumerable<BasePlayer> GetWinners();
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

    public BasePlayer NextPlayer() {
        throw new NotImplementedException();
    }
    public BasePlayer PreviousPlayer() {
        throw new NotImplementedException();
    }
    public override IEnumerable<BaseMove> AvailableMoves()
    {
        List<BaseMove> moves = new List<BaseMove>();

        foreach (IToken token in this.PlayerTokens[this.CurrentPlayer]) {
            foreach (GraphLink<IToken> link in this.Board.Table.GetLinksWithFreeNodes()) {
                BaseMove move = new BaseMove(this.CurrentPlayer, token, link.To);
                if (this.TokenRules.IsValid(move, this.Board.Table))
                    moves.Add(move);
            }
        }

        return moves;
    }
    public override bool WinAchieved()
    {
        throw new NotImplementedException();
    }
    public override IEnumerable<BasePlayer> GetWinners()
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
