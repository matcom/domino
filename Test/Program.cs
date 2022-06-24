// See https://aka.ms/new-console-template for more information
using Domino.Game;
using Domino.Tokens;
using Domino.Players;
using System;

System.Console.WriteLine("Enter max value amount: ");
string? input = Console.ReadLine();
int tokenValue = int.Parse(input == null ? "" : input);

Console.Clear();

System.Console.WriteLine("Enter player tokens amount: ");
input = Console.ReadLine();
int tokensPerPlayer = int.Parse(input == null ? "" : input);

Console.Clear();

DominoTokenGenerator gen = new DominoTokenGenerator();
DominoPlayer[] players = new DominoPlayer[]{
    new GreedyDominoPlayer("Player 1"), 
    new RandomDominoPlayer("Player 2"), 
    new RandomDominoPlayer("Player 3"), 
    new GreedyDominoPlayer("Player 4")
};

DominoGame<DominoToken> game = new DominoGame<DominoToken>(tokenValue, tokensPerPlayer, gen, players);

game.Result();
