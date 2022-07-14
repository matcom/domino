// See https://aka.ms/new-console-template for more information
using Domino.Game;
using Domino.Tokens;
using Domino.Players;
using Domino.Referee;


System.Console.WriteLine("Enter max value amount: ");
string? input = Console.ReadLine();
int tokenValue = int.Parse(input == null ? "" : input);

Console.Clear();

System.Console.WriteLine("Enter player tokens amount: ");
string? playerTokens = Console.ReadLine();
int tokensPerPlayer = int.Parse(playerTokens == null ? "" : playerTokens);

DominoPlayer[] players = new DominoPlayer[4];
IWinCondition win;
IWinner winner;
DominoToken token;

for (int i = 0; i < 4; i++) {
    Console.Clear();
    System.Console.WriteLine("Select a player to add: ");

    DominoPlayer[] dominoPlayers = new DominoPlayer[]{
        new HumanDominoPlayer($"Player {i + 1}"),
        new RandomDominoPlayer($"Player {i + 1}"),
        new GreedyDominoPlayer($"Player {i + 1}"),
        new DataDominoPlayer($"Player {i + 1}", tokenValue)
    };

    for (int j = 0; j < dominoPlayers.Length; j++) {
        System.Console.WriteLine($"{j} - {dominoPlayers[j]}");
    }

    input = Console.ReadLine();
    int playerIndex = int.Parse(input == null ? "" : input);

    players[i] = dominoPlayers[playerIndex];
}

Console.Clear();
System.Console.WriteLine("Select a Win Condition: ");

IWinCondition[] winConditions = new IWinCondition[]{
    new StandardWinCondition(),
    new FourRoundsWinCondition()
};

for (int j = 0; j < winConditions.Length; j++) {
    System.Console.WriteLine($"{j} - {winConditions[j]}");
}

input = Console.ReadLine();
int winIndex = int.Parse(input == null ? "" : input);

win = winConditions[winIndex];

Console.Clear();
System.Console.WriteLine("Select how to choose Winner: ");

IWinner[] winners = new IWinner[]{
    new MinValueWinner(),
    new MoreTokensWinner()
};

for (int j = 0; j < winners.Length; j++) {
    System.Console.WriteLine($"{j} - {winners[j]}");
}

input = Console.ReadLine();
int winnerIndex = int.Parse(input == null ? "" : input);

winner = winners[winnerIndex];

Console.Clear();
System.Console.WriteLine("Select a Token: ");

DominoToken[] tokens = new DominoToken[]{
    new DominoToken(),
    new SixUnvaluableDominoToken(),
    new DoubledValueDominoToken()
};

for (int j = 0; j < tokens.Length; j++) {
    System.Console.WriteLine($"{j} - {tokens[j].Represent()}");
}

input = Console.ReadLine();
int tokenIndex = int.Parse(input == null ? "" : input);
token = tokens[tokenIndex];

DominoGame game = new DominoGame(
    tokenValue, tokensPerPlayer, token, players, winner, win);

game.Result();
