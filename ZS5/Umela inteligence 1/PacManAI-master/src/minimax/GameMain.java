package minimax;

import static java.lang.System.out;

import java.lang.reflect.Constructor;
import java.util.*;

public class GameMain<S, A> {
    static void error(String message) {
        out.println(message);
        System.exit(0);
    }

    // A hack.  Use reflection to find the Minimax class since it is in the default package
    // and we are not.
    @SuppressWarnings("unchecked")
    Strategy<S, A> newMinimax(HeuristicGame<S, A> game, int depth) {
        try {
            Class<?> minimaxClass = Class.forName("Minimax");
            Constructor<?> constructor =
                minimaxClass.getConstructor(HeuristicGame.class, int.class);

            return (Strategy<S, A>) constructor.newInstance(game, depth);
        } catch (ClassNotFoundException e) {
            error("can't find Minimax class");
        } catch (NoSuchMethodException e) {
            error("can't find Minimax constructor");
        } catch (ReflectiveOperationException e) {
            throw new Error(e);
        }
        return null;
    }

    // A hack.  Use reflection to find the Mcts class since it is in the default package
    // and we are not.
    @SuppressWarnings("unchecked")
    Strategy<S, A> newMcts(AbstractGame<S, A> game, Strategy<S, A> base, int limit) {
        try {
            Class<?> mctsClass = Class.forName("Mcts");
            Constructor<?> constructor =
                mctsClass.getConstructor(AbstractGame.class, Strategy.class, int.class);

            return (Strategy<S, A>) constructor.newInstance(game, base, limit);
        } catch (ClassNotFoundException e) {
            error("can't find Mtcs class");
        } catch (NoSuchMethodException e) {
            error("can't find Mcts constructor");
        } catch (ReflectiveOperationException e) {
            throw new Error(e);
        }
        return null;
    }

    Strategy<S, A> strategy(
        String name, HeuristicGame<S, A> game, List<NamedStrategy<S, A>> extraStrategies) {

        int arg = -1;

        String base = null;
        int i = name.indexOf('/');
        if (i >= 0) {
            base = name.substring(i + 1);
            name = name.substring(0, i);
        }

        i = name.indexOf(':');
        if (i >= 0) {
            arg = Integer.parseInt(name.substring(i + 1));
            name = name.substring(0, i);
        }
        switch (name) {
            case "mcts":
                if (arg < 0)
                    error("must specify number of iterations for mcts");
                if (base == null)
                    error("must specify base strategy for mcts");
                Strategy<S, A> baseStrategy = strategy(base, game, extraStrategies);
                return newMcts(game, baseStrategy, arg);

            case "minimax":
                if (arg < 0)
                    error("must specify search depth for minimax");
                return newMinimax(game, arg);

            case "random": return new RandomStrategy<>(game);

            default:
                for (NamedStrategy<S, A> s : extraStrategies)
                    if (s.name.equals(name))
                        // We retrieve a new instance of the strategy here so that e.g.
                        // there will be separate strategy objects when a strategy plays
                        // against itself.
                        return s.strategy.get();

                error("unknown strategy");
                return null;
        }
    }

    void usage(String program, List<NamedStrategy<S, A>> extraStrategies) {
        out.printf("usage: %s <strategy1> [<strategy2>] [<option> ...]\n", program);
        out.println("options:");
        out.println("  -seed <num> : random seed");
        out.println("  -sim <count> : simulate a series of games without visualization");
        out.println("  -v : verbose output");
        out.println();

        out.println("available strategies:");
        for (NamedStrategy<S, A> s : extraStrategies)
            out.printf("  %s\n", s.name);
        out.println("  random");
        out.println("  minimax:<depth>");
        out.println("  mcts:<iterations>/<base-strategy>");
        System.exit(0);
    }
    
    public void main(
        String program, HeuristicGame<S, A> game, UI<S, A> ui,
        List<NamedStrategy<S, A>> extraStrategies, String[] args) {

        ArrayList<Strategy<S, A>> strategies = new ArrayList<>();
        strategies.add(null);   // dummy entry

        int games = 0;
        int seed = -1;
        boolean verbose = false;

        for (int i = 0; i < args.length ; ++i) {
            if (args[i].startsWith("-"))
                switch (args[i]) {
                    case "-seed":
                        seed = Integer.parseInt(args[++i]);
                        break;
                    case "-sim":
                        games = Integer.parseInt(args[++i]);
                        break;
                    case "-v":
                        verbose = true;
                        break;
                    default:
                        usage(program, extraStrategies);
                }
            else
                strategies.add(strategy(args[i], game, extraStrategies));
        }

        int players = strategies.size() - 1;
        if (players == 0)
            usage(program, extraStrategies);

        if (games > 0) {
            if (players != 2)
                error("must specify 2 strategies with -sim");
            Runner.play(game, strategies.get(1), strategies.get(2),
                        games, seed >= 0 ? seed : 0, verbose);
        } else {
            if (strategies.isEmpty())
                usage(program, extraStrategies);

            if (ui == null) {
                out.println("no UI available for this game");
                return;
            }

            if (players == 1)
                strategies.add(0, null);  // add human player

            for (int p = 1 ; p < strategies.size() ; ++p) {
                var s = strategies.get(p);
                if (s != null)
                    Runner.seed(s, seed, p);
            }
            ui.init(seed, strategies);

            ui.run();
        }
    }
}
