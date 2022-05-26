using Table;
using Player;
using Judge;
using Game;

Token t = new Token(new int[] { 1, 2, 3 });
TableGame a = new TableDimension(t);
//TableComplex b = new TableComplex(t);
a.PlayTable(a.TableNode[1], new Token(new int[] { 4, 5, 6 }));
// System.Console.WriteLine(a.PlayNode.Count);
// System.Console.WriteLine(a.FreeNode.Count);
// System.Console.WriteLine(a.TableNode.Count);
System.Console.WriteLine(a.PlayNode.Count);
System.Console.WriteLine(a.FreeNode.Count);
System.Console.WriteLine(a.TableNode.Count);
// System.Console.WriteLine("****");
// for (int i = b.TableNode.Count() - 3; i < b.TableNode.Count(); i++)
// {
//     for (int j = 0; j < 3; j++)
//     {
//         if (b.TableNode[i].Conections[j] == null) System.Console.WriteLine(j + " " + i);
//     }
// }
// Coordenates a = new Coordenates(new (int, int)[] { (1, 2), (1, 3), (0, 3) });
// Coordenates b = new Coordenates(new (int, int)[] { (1, 2), (0, 3), (1, 3) });

// HashSet<Coordenates> d = new HashSet<Coordenates>();
// d.Add(a);

// System.Console.WriteLine(a.Equals(b));
// System.Console.WriteLine(d.Contains(b));
// System.Console.WriteLine(a.GetHashCode());
// System.Console.WriteLine(b.GetHashCode());
// System.Console.WriteLine(new int[2] { 1, 2 }.GetHashCode());
// System.Console.WriteLine(new int[2] { 1, 2 }.GetHashCode());

