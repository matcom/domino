namespace Domino.Game;

public class DominoGame {
    DominoPlayer[] players;
    IEnumerable<DominoToken>[] playerTokens;
    List<DominoMove> moves;
    DominoToken startToken;
    int[] freeValues;
    int currentPlayer;

    public DominoGame(int tokenValues, int tokenAmount) {
        this.currentPlayer = 0;
        this.freeValues = new int[2];
        this.players = new DominoPlayer[]{
            new DominoPlayer("Player 1"), 
            new DominoPlayer("Player 2"), 
            new DominoPlayer("Player 3"), 
            new DominoPlayer("Player 4")
        };
        this.playerTokens = new IEnumerable<DominoToken>[this.players.Length];
        this.moves = new List<DominoMove>();

        List<int[]> tokenValuesList = new List<int[]>();
        List<DominoToken> tokens = new List<DominoToken>();

        Utils.GenerateTokenValues(tokenValues, tokenValuesList);

        // System.Console.WriteLine(tokenValuesList.Count);
        
        foreach(int[] values in tokenValuesList) {
            tokens.Add(new DominoToken(values[0], values[1]));
        }

        for(int i = 0; i < this.players.Length; i++) {
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

        this.startToken = this.players[this.currentPlayer].PlayStartToken(playerTokens[this.currentPlayer]);
        this.playerTokens[this.currentPlayer] = playerTokens[this.currentPlayer].Where(
            token => token != this.startToken
        );
        this.moves.Add(new DominoMove(this.players[this.currentPlayer], this.startToken));
        this.freeValues[0] = this.startToken.Left;
        this.freeValues[1] = this.startToken.Right;
        this.currentPlayer++;
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

    DominoPlayer GetWinner() {
        int[] playerScores = new int[this.players.Length];

        for(int i = 0; i < this.players.Length; i++)
        {
            playerScores[i] += this.playerTokens[i].Sum(token => token.Value());
        }

        return this.players[Array.IndexOf(playerScores, playerScores.Max())];
    }

    public void Result() {
        int passCounter = 0;

        while (passCounter < 4) {
            if (CanPlay(this.currentPlayer)) {
                passCounter = 0;
                bool valid = false;
                while(!valid) {
                    DominoMove move = players[this.currentPlayer].PlayToken(
                        playerTokens[this.currentPlayer], this.moves, this.freeValues
                    );
                    if (IsValid(move)) {
                        valid = true;
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
                        if (this.currentPlayer == this.players.Length - 1)
                            this.currentPlayer = 0;
                        else
                            this.currentPlayer++;
                    }
                }
            }
            else
                passCounter++;
        }

        foreach(DominoMove move in this.moves)
            System.Console.WriteLine(move);
        System.Console.WriteLine(GetWinner());
    }
}

class DominoPlayer {
    string identifier;
    public DominoPlayer(string name) {
        this.identifier = name;
    }
    public virtual DominoMove PlayToken(
        IEnumerable<DominoToken> tokens, IEnumerable<DominoMove> moves, int[] freeValues
    ) {
        IEnumerable<DominoToken> availableTokens = tokens.Where(
            token => freeValues.Contains(token.Left) || freeValues.Contains(token.Right)
        );
        DominoToken selected = availableTokens.OrderBy(token => token.Value()).First();

        return new DominoMove(this, selected);
    }

    public virtual DominoToken PlayStartToken(IEnumerable<DominoToken> tokens) {
        return tokens.First();
    }

    public override string ToString() {
        return this.identifier;
    }
}

class DominoMove {
    public DominoPlayer Player { get; }
    public DominoToken Token { get; }

    public DominoMove(DominoPlayer player, DominoToken token) {
        this.Player = player;
        this.Token = token;
    }

    public override string ToString() {
        return $"{this.Player.ToString()} => {this.Token.ToString()}";
    }
}

class DominoToken {
    public int Left { get; }
    public int Right { get; }

    public DominoToken(int left, int right) {
        this.Left = left;
        this.Right = right;
    }
    public int Value() {
        return Left + Right;
    }

    public override string ToString()
    {
        return $"({Left} | {Right})";
    }
}

static class Utils {
    public static void GenerateVariations(int n, int[] variation, int length, List<int[]> collection) {
        if (length == variation.Length) {
            collection.Add(new int[] {variation[0], variation[1]});
            return;
        }
        for (int i = 0; i <= n; i++) {
            variation[length] = i;
            GenerateVariations(n, variation, length + 1, collection);
        }
    }

    public static void GenerateTokenValues(int n, List<int[]> collection) {
        for (int i = 0; i <= n; i++) {
            for (int j = i; j <= n; j++) {
                collection.Add(new int[]{i, j});
            }
        }
    }
}