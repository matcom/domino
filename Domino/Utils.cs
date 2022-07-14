namespace Domino.Utils;

public static class Utils {
    /// <summary>
    ///     Given a number representing the game signature e.g: 9 for 9-based domino, generates
    ///     a collection of all possible pairs of values for that game signature
    /// </summary>
    public static void GenerateTokenValues(int n, List<int[]> collection) {
        for (int i = 0; i <= n; i++) {
            for (int j = i; j <= n; j++) {
                collection.Add(new int[]{i, j});
            }
        }
    }
}
