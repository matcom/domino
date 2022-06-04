// See https://aka.ms/new-console-template for more information
using Domino.Utils;
using System;
using System.Collections.Generic;

#region VariationTest

List<int[]> tokenValuesList = new List<int[]>();

Combinatorics.GenerateVariations(7, new int[2], 0, tokenValuesList);

Console.WriteLine(tokenValuesList.Count);

foreach (int[] values in tokenValuesList)
    Console.WriteLine($"{values[0]} - {values[1]}");

#endregion