using Domino.Players;
using Domino.Tokens;
using Domino.Referee;

namespace Domino.Game;

public class DominoGame {
    IList<DominoPlayer> players;
    IEnumerable<DominoToken>[] playerTokens;
    List<DominoMove> moves;
    DominoToken? startToken;
    int[] freeValues;
    int currentPlayer;
    int passCounter = 0;
    bool ended = false;
    IWinner winnerCheck;
    IWinCondition winCondition;

    public DominoGame(
        int tokenValues, 
        int tokenAmount, 
        DominoToken token, 
        IList<DominoPlayer> players, 
        IWinner winnerCheck, 
        IWinCondition winCondition
    ) {
        this.winCondition = winCondition;
        this.winnerCheck = winnerCheck;
        this.currentPlayer = 0;
        this.freeValues = new int[2];
        this.players = players;

        this.playerTokens = new IEnumerable<DominoToken>[this.players.Count];
        this.moves = new List<DominoMove>();

        IList<DominoToken> tokens = token.GenerateTokens(tokenValues);

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
        }

        StartGame();
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

    bool CanPlay(int playerIndex) {
        foreach(DominoToken token in this.playerTokens[playerIndex]) {
            if (this.freeValues.Contains(token.Right) || this.freeValues.Contains(token.Left)) {
                return true;
            }
        }

        return false;
    }

    void NextPlayer() {
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
            while (!this.winCondition.Achieved(
                this.players, this.playerTokens, this.passCounter, this.freeValues, this.moves
            )) {
                if (CanPlay(this.currentPlayer)) {
                    Console.Clear();

                    System.Console.WriteLine("Moves:");
                    foreach(DominoMove m in this.moves)
                        System.Console.WriteLine(m);

                    this.passCounter = 0;

                    AddMove(
                        players[this.currentPlayer].Play(
                        playerTokens[this.currentPlayer], this.moves, this.freeValues
                    ));                    
                    
                    NextPlayer();
                }
                else {
                    this.passCounter++;
                    NextPlayer();
                }
            }
            ended = true;
        }

        System.Console.Clear();
        System.Console.WriteLine(this.moves.Count);
        System.Console.WriteLine("Moves:");
        foreach(DominoMove move in this.moves) System.Console.WriteLine(move);
        System.Console.WriteLine($"Winner: {winnerCheck.GetWinner(this.players, this.playerTokens)}");
    }
}

/// <summary>
///     Object representing a move: A reference to a player and the token he played
/// </summary>
public class DominoMove {
    public DominoPlayer Player { get; }
    public DominoToken Token { get; }

    public DominoMove(DominoPlayer player, DominoToken token) {
        this.Player = player;
        this.Token = token;
    }

    /// <returns>
    ///     Given a token and a game state, returns if that token can be played
    /// </returns>
    public static bool IsValid(DominoToken token, int[] freeValues) {
        if (freeValues.Contains(token.Left) || freeValues.Contains(token.Right))
            return true;
        return false;
    }

    public override string ToString() {
        return $"{this.Player.ToString()} => {this.Token.ToString()}";
    }
}
