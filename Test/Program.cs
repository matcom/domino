// See https://aka.ms/new-console-template for more information
using Domino.Game;
using Domino.Tokens;
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

DominoGame<DominoToken> game = new DominoGame<DominoToken>(tokenValue, tokensPerPlayer, gen);

game.Result();
