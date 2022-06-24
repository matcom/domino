namespace Domino.Utils;

public static class Utils {
    public static void GenerateTokenValues(int n, List<int[]> collection) {
        for (int i = 0; i <= n; i++) {
            for (int j = i; j <= n; j++) {
                collection.Add(new int[]{i, j});
            }
        }
    }
}
