using Domino.Game;
using Domino.Tokens;
using Domino.Players;
using Domino.Referee;

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


System.Console.WriteLine("Enter player amount: ");
System.Console.WriteLine("Default: 4 players");
System.Console.WriteLine();

string? input = Console.ReadLine();
int playerCount = int.Parse(input! == "" ? "4" : input!);

Console.Clear();

System.Console.WriteLine("Enter max value amount: ");
System.Console.WriteLine("Default: Double 7");
System.Console.WriteLine();

input = Console.ReadLine();
int tokenValue = int.Parse(input! == "" ? "7" : input!);

Console.Clear();

int maxTokensPerPlayer = (tokenValue * (tokenValue + 1)) / (2 * playerCount);

System.Console.WriteLine("Enter player tokens amount: ");
System.Console.WriteLine("Default: 7");
System.Console.WriteLine();
System.Console.WriteLine("NOTE: This is for standard 4 players");
System.Console.WriteLine($"Otherwise this amount must be less or equal than {maxTokensPerPlayer}");
System.Console.WriteLine();

input = Console.ReadLine();
int tokensPerPlayer = int.Parse(input! == "" ? "7" : input!);

Console.Clear();


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

    input = Console.ReadLine();
    int playerIndex = int.Parse(input! == "" ? "0" : input!);

    players[i] = dominoPlayers[playerIndex];
}

Console.Clear();
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

input = Console.ReadLine();
int winIndex = int.Parse(input! == "" ? "0" : input!);

win = winConditions[winIndex];

Console.Clear();
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

input = Console.ReadLine();
int winnerIndex = int.Parse(input! == "" ? "0" : input!);

winner = winners[winnerIndex];

Console.Clear();
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

input = Console.ReadLine();
int tokenIndex = int.Parse(input! == "" ? "0" : input!);
token = tokens[tokenIndex];

DominoGame game = new DominoGame(
    tokenValue, tokensPerPlayer, token, players, winner, win);

game.Result();
