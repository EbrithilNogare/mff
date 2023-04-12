public class Main {

    private static int S_TO_MS = 1000;

    private static int INTERESTING_TIME = 5;
    private static int NESTED_TIME_ONE = 4;
    private static int NESTED_TIME_TWO = 3;

    public static long subroutine(int len) {
        long res = System.currentTimeMillis();
        long end = res + len * S_TO_MS;
        while (true) {
            long now = System.currentTimeMillis();
            res ^= now;
            if (now > end) {
                break;
            }
        }
        return res;
    }

    public static long interesting() {
        long res = subroutine(NESTED_TIME_ONE);
        long end = System.currentTimeMillis() + INTERESTING_TIME * S_TO_MS;
        while (true) {
            long now = System.currentTimeMillis();
            res ^= now;
            if (now > end) {
                break;
            }
        }
        return res ^ subroutine(NESTED_TIME_TWO);
    }

    public static void main(String[] args) {
        long start = System.nanoTime();
        long res = interesting();
        long end = System.nanoTime();

        // The res variable is present to avoid excess optimization.
        // Just ignore the value, it is not relevant to the example.

        System.out.printf ("Took %d ms [res %d].\n", (end - start) / 1000 / 1000, res % 17);
        System.out.printf ("The interesting method duration should be around %d s.\n", INTERESTING_TIME);
        System.out.printf ("The nested method duration should be around %d s.\n", NESTED_TIME_ONE + NESTED_TIME_TWO);
        System.out.printf ("The total duration should be around %d s.\n", INTERESTING_TIME + NESTED_TIME_ONE + NESTED_TIME_TWO);
    }
}
