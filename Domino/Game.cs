using Domino.Players;
using Domino.Tokens;
using Domino.Utils;

namespace Domino.Game;

public class DominoGame<TToken> where TToken : DominoToken {
    IList<DominoPlayer> players;
    IEnumerable<DominoToken>[] playerTokens;
    List<DominoMove> moves;
    DominoToken? startToken;
    int[] freeValues;
    int currentPlayer;
    int passCounter = 0;
    bool ended = false;

    public DominoGame(
        int tokenValues, int tokenAmount, ITokenGenerator<TToken> generator, IList<DominoPlayer> players
    ) {
        this.currentPlayer = 0;
        this.freeValues = new int[2];
        this.players = players;

        this.playerTokens = new IEnumerable<DominoToken>[this.players.Count];
        this.moves = new List<DominoMove>();

        IList<TToken> tokens = generator.GenerateTokens(tokenValues);

        for(int i = 0; i < this.players.Count; i++) {
            List<DominoToken> playerTokenList = new List<DominoToken>();
            Random randObject = new Random();

            for (int j = 0; j < tokenAmount; j++)
            {
                int index = randObject.Next(tokens.Count);
                playerTokenList.Add(tokens[index]);
                tokens.RemoveAt(index);
            }

            this.playerTokens[i] = playerTokenList;

            StartGame();
        }
    }
    
    void StartGame() {
        this.startToken = this.players[this.currentPlayer].PlayStartToken(playerTokens[this.currentPlayer]);
        this.playerTokens[this.currentPlayer] = playerTokens[this.currentPlayer].Where(
            token => token != this.startToken
        );
        this.moves.Add(new DominoMove(this.players[this.currentPlayer], this.startToken));
        this.freeValues[0] = this.startToken.Left;
        this.freeValues[1] = this.startToken.Right;
        NextPlayer();
    }
    bool IsValid(DominoMove move) {
        if (this.freeValues.Contains(move.Token.Left) || this.freeValues.Contains(move.Token.Right))
            return true;
        return false;
    }

    bool CanPlay(int playerIndex) {
        foreach(DominoToken token in this.playerTokens[playerIndex]) {
            if (this.freeValues.Contains(token.Right) || this.freeValues.Contains(token.Left)) {
                return true;
            }
        }

        return false;
    }

    protected virtual bool WinCondition() {
        if (passCounter == this.players.Count)
            return true;
        return false;
    }

    public virtual DominoPlayer GetWinner() {
        int[] playerScores = new int[this.players.Count];

        for(int i = 0; i < this.players.Count; i++)
        {
            playerScores[i] += this.playerTokens[i].Sum(token => token.Value());
        }

        return this.players[Array.IndexOf(playerScores, playerScores.Min())];
    }

    protected virtual void NextPlayer() {
        if (this.currentPlayer == this.players.Count - 1)
            this.currentPlayer = 0;
        else
            this.currentPlayer++;
    }

    void AddMove(DominoMove move) {
        this.moves.Add(move);

        for (int i = 0; i < this.freeValues.Length; i++)
        {
            if (this.freeValues[i] == move.Token.Right) {
                this.freeValues[i] = move.Token.Left;
                break;
            } else if (this.freeValues[i] == move.Token.Left) {
                this.freeValues[i] = move.Token.Right;
                break;
            }
        }

        this.playerTokens[this.currentPlayer] = this.playerTokens[this.currentPlayer].Where(
            token => token != move.Token
        );
    }

    public void Result() {
        if (!ended) {
            while (!WinCondition()) {
                if (CanPlay(this.currentPlayer)) {
                    this.passCounter = 0;

                    DominoMove move = players[this.currentPlayer].PlayToken(
                        playerTokens[this.currentPlayer], this.moves, this.freeValues
                    );
                    
                    AddMove(move);
                    
                    NextPlayer();
                }
                else {
                    this.passCounter++;
                    NextPlayer();
                }
            }
            ended = true;
        }
        
        foreach(DominoMove move in this.moves)
            System.Console.WriteLine(move);
        System.Console.WriteLine($"Winner: {GetWinner()}");
        System.Console.WriteLine(this.moves.Count);
    }
}

public class DominoMove {
    public DominoPlayer Player { get; }
    public DominoToken Token { get; }

    public DominoMove(DominoPlayer player, DominoToken token) {
        this.Player = player;
        this.Token = token;
    }

    public static bool IsValid(DominoToken token, int[] freeValues) {
        if (freeValues.Contains(token.Left) || freeValues.Contains(token.Right))
            return true;
        return false;
    }

    public override string ToString() {
        return $"{this.Player.ToString()} => {this.Token.ToString()}";
    }
}
