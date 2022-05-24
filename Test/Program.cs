using Table;
using Player;
using Judge;
using Game;

/*Token f = new Token(new int[] { 1, 2, 3 });
Console.WriteLine(f.CantValues);
Console.WriteLine(f.Score);
StartGame game = new StartGame(4, 2, 10);
Console.WriteLine(game.Tokens.Count);
foreach (var item in game.Tokens)
{
    foreach (var item1 in item.Values)
    {
        Console.Write(item1 + " ");
    }
    System.Console.WriteLine();
}*/
Token t = new Token(new int[] { 1, 2, 3 });
//TableGame a = new TableGame(t);
TableGameComplex b = new TableGameComplex(t);
b.PlayTable(b.TableNode[1], new Token(new int[] { 4, 5, 6 }));
// System.Console.WriteLine(a.PlayNode.Count);
// System.Console.WriteLine(a.FreeNode.Count);
// System.Console.WriteLine(a.TableNode.Count);
System.Console.WriteLine(b.PlayNode.Count);
System.Console.WriteLine(b.FreeNode.Count);
System.Console.WriteLine(b.TableNode.Count);
System.Console.WriteLine("****");
for (int i = b.TableNode.Count() - 3; i < b.TableNode.Count(); i++)
{
    for (int j = 0; j < 3; j++)
    {
        if (b.TableNode[i].Conections[j] == null) System.Console.WriteLine(j + " " + i);
    }
}
