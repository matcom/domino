using Domino.Game;
using Domino.Tokens;
using Domino.Players;
using Domino.Referee;

namespace UI;

static void Main() {
    System.Console.WriteLine("Welcome to this Domino Game");
    System.Console.WriteLine("To play you must select this relevant game options from a list:");
    System.Console.WriteLine();

    string[] options = new string[] {
        "- Token Amount. Ex: Double 9",
        "- Tokens in Starting Hand per Player",
        "- Amount of players",
        "- Player strategies, as well as play against them",
        "- Win Condition for game termination",
        "- How to select a Winner",
        "- Token Value implementation"
    };

    foreach(string option in options)
        System.Console.WriteLine(option);

    System.Console.WriteLine();
    System.Console.WriteLine("To continue press any key...");

    Console.ReadLine();
    Console.Clear();

    int playerCount = SetPlayerCount();
    int value = SetValueAmount();
    int tokensAmount = SetTokensPerPlayer();
    DominoPlayer[] players = SetPlayers(playerCount);
    IWinCondition win = SetWinCondition();
    IWinner winner = SetWinner();
    DominoToken token = SetToken();

    PrepareGame(value, tokensAmount, token, players, winner, win);
}

// Generic method to ask user for input
static int GetInput(int default, int max = -1) {
    string? input = Console.ReadLine();
    int result = int.Parse(input! == "" ? (string)default : input!);
    
    if (max > -1) {
        while(result > max) {
            Console.WriteLine($"{result} is not a valid input");
            Console.WriteLine("Plase select a valid value");

            input = Console.ReadLine();
            result = int.Parse(input! == "" ? (string)default : input!);
        }
    }
    
    return result;
}

// Logic to allow user to enter the player amount
static int SetPlayerCount() {
    System.Console.WriteLine("Enter player amount: ");
    System.Console.WriteLine("Default: 4 players");
    System.Console.WriteLine();

    int playerCount = GetInput(4);

    Console.Clear();

    return playerCount;
}

// Logic to allow user to enter game type by token value
static int SetValueAmount() {
    System.Console.WriteLine("Enter max value amount: ");
    System.Console.WriteLine("Default: Double 7");
    System.Console.WriteLine();

    int tokenValue = GetInput(4);

    Console.Clear();

    return tokenValue;
}

// Logic to allow user to enter max amount of tokens to deal to each player
static int SetTokensPerPlayer() {
    int maxTokensPerPlayer = (tokenValue * (tokenValue + 1)) / (2 * playerCount);

    System.Console.WriteLine("Enter player tokens amount: ");
    System.Console.WriteLine("Default: 7");
    System.Console.WriteLine();
    System.Console.WriteLine("NOTE: This is for standard 4 players");
    System.Console.WriteLine($"Otherwise this amount must be less or equal than {maxTokensPerPlayer}");
    System.Console.WriteLine();

    int tokensPerPlayer = GetInput(7, maxTokensPerPlayer);

    Console.Clear();

    return tokensPerPlayer;
}

// Logic to allow user to enter player types based on previosly selected amount
static DominoPlayer[] SetPlayers(int playerCount) {
    DominoPlayer[] players = new DominoPlayer[playerCount];
    IWinCondition win;
    IWinner winner;
    DominoToken token;

    for (int i = 0; i < playerCount; i++) {
        Console.Clear();
        System.Console.WriteLine($"There are already {i} players");
        System.Console.WriteLine();
        System.Console.WriteLine("Select a player to add: ");

        DominoPlayer[] dominoPlayers = new DominoPlayer[]{
            new HumanDominoPlayer($"Player {i + 1}"),
            new RandomDominoPlayer($"Player {i + 1}"),
            new GreedyDominoPlayer($"Player {i + 1}"),
            new DataDominoPlayer($"Player {i + 1}", tokenValue)
        };

        System.Console.WriteLine($"Default: {dominoPlayers[0]}");
        System.Console.WriteLine();

        for (int j = 0; j < dominoPlayers.Length; j++) {
            System.Console.WriteLine($"{j} - {dominoPlayers[j]}");
        }

        int playerIndex = GetInput(0, playerCount);

        players[i] = dominoPlayers[playerIndex];
    }

    Console.Clear();

    return players;
}

// Logic to allow user to select a win condition for the game
static IWinCondition SetWinCondition() {
    System.Console.WriteLine("Select a Win Condition: ");

    IWinCondition[] winConditions = new IWinCondition[]{
        new StandardWinCondition(),
        new FourRoundsWinCondition()
    };

    System.Console.WriteLine($"Default: {winConditions[0]}");
    System.Console.WriteLine();

    for (int j = 0; j < winConditions.Length; j++) {
        System.Console.WriteLine($"{j} - {winConditions[j]}");
    }

    int winIndex = GetInput(0, winConditions.Length - 1);

    Console.Clear();

    return winConditions[winIndex];
}

// Logic to allow user to set a winner criteria for the game
static IWinner SetWinner() {
    System.Console.WriteLine("Select how to choose Winner: ");

    IWinner[] winners = new IWinner[]{
        new MinValueWinner(),
        new MoreTokensWinner()
    };

    System.Console.WriteLine($"Default: {winners[0]}");
    System.Console.WriteLine();

    for (int j = 0; j < winners.Length; j++) {
        System.Console.WriteLine($"{j} - {winners[j]}");
    }

    int winnerIndex = GetInput(0, winners.Length - 1);

    Console.Clear();

    return winners[winnerIndex];
}

// Logic to allow user to select the token value calculation
static DominoToken SetToken() {

    System.Console.WriteLine("Select a Token: ");

    DominoToken[] tokens = new DominoToken[]{
        new DominoToken(),
        new SixUnvaluableDominoToken(),
        new DoubledValueDominoToken()
    };

    System.Console.WriteLine($"Default: {tokens[0]}");
    System.Console.WriteLine();

    for (int j = 0; j < tokens.Length; j++) {
        System.Console.WriteLine($"{j} - {tokens[j]}");
    }

    int tokenIndex = GetInput(0, tokens.Length - 1);

    Console.Clear();

    return tokens[tokenIndex];
}

// Just wrapping the instantiation and execution of the game instance
static void PrepareGame(tokenValue, tokensPerPlayer, token, players, winner, win) {
    DominoGame game = new DominoGame(
        tokenValue, tokensPerPlayer, token, players, winner, win);

    game.Result();
}
