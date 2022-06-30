namespace DominoEngine; 

public static class Utils {
	public static IEnumerable<(int, TSource)> Enumerate<TSource>(this IEnumerable<TSource> source) => source.Select((t, i) => (index:i, item:t));

	public static IEnumerable<TSource> Complement<TSource>(this IEnumerable<TSource> source, IEnumerable<TSource> another)
		=> source.Where(x => !another.Contains(x));

	public static bool IsEmpty<TSource>(this IEnumerable<TSource> source) => (source.Take(1).Count() is 0);

	public static void Make<TSource>(this IEnumerable<TSource> source, Action<TSource> function) {
		foreach (var item in source)
			function(item);
	}
}