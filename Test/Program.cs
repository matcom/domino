using Table;
using Player;
using Judge;
using Game;

Token t = new Token(new int[] { 1, 2, 3 });
TableTriangular a = new TableTriangular(t);
System.Console.WriteLine(a.FreeNode.Count);
int i = 0;
//Node b = new NodeGeometry(new (int, int)[] { (0, 0) });
System.Console.WriteLine("****");
//Console.WriteLine(a.FreeNode.Contains(b));

//System.Console.WriteLine(a.FreeNode.Contains(a.e[1]));
NodeGeometry b = null;
foreach (var item in a.FreeNode)
{
    i++;
    b = item as NodeGeometry;
    if (i == 3) break;
    // System.Console.WriteLine(a.TableNode.ContainsKey(b.Ubication));
    System.Console.WriteLine(a.FreeNode.Contains(item));
}
a.PlayTable(b, new Token(new int[] { 0, 0, 0 }));
foreach (var item in a.FreeNode)
{
    i++;
    b = item as NodeGeometry;
    if (i == 3) break;
    // System.Console.WriteLine(a.TableNode.ContainsKey(b.Ubication));
    System.Console.WriteLine(a.FreeNode.Contains(item));
}
a.PlayTable(b, new Token(new int[] { 0, 0, 0 }));
System.Console.WriteLine(a.FreeNode.Count);


