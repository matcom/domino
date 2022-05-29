using Table;
using Player;
using Judge;
using Game;

Token t = new Token(new int[] { 1, 2, 3, 4, 5, 6 });
TableHexagonal a = new TableHexagonal(t, new (int, int)[] { (0, 0), (2, 0), (3, 1), (2, 2), (0, 2), (-1, 1) });
// TableTriangular a = new TableTriangular(t, new (int, int)[] { (0, 0), (1, 1), (2, 0) });
System.Console.WriteLine(a.FreeNode.Count);
//Node b = new NodeGeometry(new (int, int)[] { (0, 0) });
//System.Console.WriteLine("****");
//Console.WriteLine(a.FreeNode.Contains(b));

a.PlayTable(a.TableNode[1], new Token(new int[] { 0, 0, 0 }));
System.Console.WriteLine(a.FreeNode.Count);
a.PlayTable(a.TableNode[9], new Token(new int[] { 0, 0, 0 }));
System.Console.WriteLine(a.FreeNode.Count);
System.Console.WriteLine(a.FreeNode.Contains(a.TableNode[2]));


