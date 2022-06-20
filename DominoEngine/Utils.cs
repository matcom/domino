namespace DominoEngine; 

public static class Utils {
	public static IEnumerable<(int, TSource)> Enumerate<TSource>(this IEnumerable<TSource> source) => source.Select((t, i) => (index:i, item:t));
}