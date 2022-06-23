namespace DominoEngine; 

public static class Utils {
	public static IEnumerable<(int, TSource)> Enumerate<TSource>(this IEnumerable<TSource> source) => source.Select((t, i) => (index:i, item:t));

	public static bool IsEmpty<TSource>(this IEnumerable<TSource> source) => (source.Take(1).Count() is 0);
}