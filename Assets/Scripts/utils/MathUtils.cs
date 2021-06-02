class MathUtils {
    public static int SignedCantorPair(int x, int y) {
        x = x >= 0 ? 2 * x : -2 * x + 1;
        y = y >= 0 ? 2 * y : -2 * y + 1;

        return (((x + y) * (x + y + 1)) / 2) + y;
    }
}