import search.*;

import problems.*;

class AStarTest {
  static <S, A> void runTest(HeuristicProblem<S, A> prob) {
    long start = System.currentTimeMillis();
    Solution<S, A> solution = AStar.search(prob);
    long elapsed = System.currentTimeMillis() - start;
    if (Solution.report(solution, prob))
      System.out.printf("solved in %d ms\n", elapsed);
  }

  public static void main(String[] args) {
    System.out.println("== Cube ==");

    runTest(new Cube());

    System.out.println("== NPuzzle ==");

        // shortest solution = 46 steps
        // A* with the taxicab heuristic will explore about 600,000 states
        int[] puzzle = {
            2, 11, 14,  3,
            8,  6,  7, 13,
            0,  5,  4, 15,
            1,  9, 10, 12
        };

        // shortest solution = 44 steps
        // A* with the taxicab heuristic will explore about 1,400,000 states 
        int[] puzzle2 = {
            12, 9, 6, 2, 
            10, 5, 4, 3, 
            1,  8, 11, 14, 
            7,  0, 13, 15
        };

        int[][] puzzles = { puzzle, puzzle2 };

        for (int[] p : puzzles) {
            PuzzleState ps = new PuzzleState(4, p);
            System.out.println("\n" + ps);
      
            NPuzzle np = new NPuzzle(ps);
            runTest(np);
        }
  }
}
