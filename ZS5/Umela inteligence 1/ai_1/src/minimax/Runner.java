package minimax;

import org.apache.commons.math3.stat.interval.*;

public class Runner {
    static <S, A> String name(Strategy<S, A> strat) {
        String s = strat.toString();
        return s.contains("@") ? strat.getClass().getSimpleName() : s;
    }

    public static <S, A> void seed(Strategy<S, A> strategy, int seed, int player) {
        if (strategy instanceof SeededStrategy) {
            if (seed >= 0 && player == 2)
                seed += 1_000_000;
            ((SeededStrategy<S, A>) strategy).setSeed(seed);
        }
    }

    public static <S, A> int[] play(AbstractGame<S, A> game, Strategy<S, A> strat1, Strategy<S, A> strat2,
                                    int count, int startSeed, boolean verbose) {
        System.out.printf("playing %d games: %s (player 1) vs. %s (player 2)\n",
                          count, name(strat1), name(strat2));

        int[] wins = new int[3];
        long[] time = new long[3];
        int[] totalMoves = new int[3];
		
		for (int i = 0 ; i < count ; ++i) {
            int seed = startSeed + i;
            seed(strat1, seed, 1);
            seed(strat2, seed, 2);

            S s = game.initialState(seed);
            int moves = 0;
			while (!game.isDone(s)) {
                int player = game.player(s);
                long start = System.currentTimeMillis();
                A a = player == 1 ? strat1.action(s) : strat2.action(s);
                time[player] += System.currentTimeMillis() - start;

                game.apply(s, a);
                moves += 1;
                totalMoves[player] += 1;
			}

            if (verbose)
                System.out.printf("seed %d: ", seed);
			double o = game.outcome(s);
			if (o == AbstractGame.DRAW) {
                ++wins[0];
                if (verbose)
                    System.out.print("draw");
            }
			else if (o > AbstractGame.DRAW) {
                ++wins[1];
                if (verbose)
                    System.out.printf("%s won", name(strat1));
            }
            else {
                ++wins[2];
                if (verbose)
                    System.out.printf("%s won", name(strat2));
            }
            if (verbose)
                System.out.printf(" in %d moves\n", moves);
		}
		
		System.out.format("    %s won %d (%.1f%%), ", name(strat1), wins[1], 100.0 * wins[1] / count);
        
        int draws = wins[0];
		if (draws > 0)
			System.out.format("%d draws (%.1f%%), ", draws, 100.0 * draws / count);
		
        System.out.format("%s won %d (%.1f%%)\n", name(strat2), wins[2], 100.0 * wins[2] / count);
        
        if (verbose)
            System.out.printf("    %s took %.1f ms/move, %s took %.1f ms/move\n",
                name(strat1), 1.0 * time[1] / totalMoves[1],
                name(strat2), 1.0 * time[2] / totalMoves[2]);

        return wins;
    }

    public static <S, A> int[][] play2(
			AbstractGame<S, A> game, Strategy<S, A> strat1, Strategy<S, A> strat2, int count) {
        int[][] wins = new int[3][];
        wins[1] = play(game, strat1, strat2, count, 0, false);
        wins[2] = play(game, strat2, strat1, count, 0, false);
        int totalDraws = wins[1][0] + wins[2][0];

        int confidence = 99;
        System.out.printf("\nwith %d%% confidence, %s\n", confidence, name(strat1));

        for (int asPlayer = 1 ; asPlayer <= 3 ; ++asPlayer) {
            int games = asPlayer < 3 ? count : 2 * count;
            int win = asPlayer < 3 ? wins[asPlayer][asPlayer] : wins[1][1] + wins[2][2];

            ConfidenceInterval ci =
                IntervalUtils.getWilsonScoreInterval(games, win, confidence / 100.0);
            double lo = ci.getLowerBound() * 100, hi = ci.getUpperBound() * 100;

            int draw = asPlayer < 3 ? wins[asPlayer][0] : totalDraws;

            ConfidenceInterval ci2 =
            IntervalUtils.getWilsonScoreInterval(games, draw, confidence / 100.0);
            double lo2 = ci2.getLowerBound() * 100, hi2 = ci2.getUpperBound() * 100;

            System.out.printf("    %s: ",
                              asPlayer < 3 ? "as player " + asPlayer : "overall    ");
            System.out.printf(" wins %4.1f%% - %4.1f%%", lo, hi);
            if (totalDraws > 0)
                System.out.printf(", draws %4.1f%% - %4.1f%%", lo2, hi2);
            System.out.println();
        }

        return wins;
    }
}
