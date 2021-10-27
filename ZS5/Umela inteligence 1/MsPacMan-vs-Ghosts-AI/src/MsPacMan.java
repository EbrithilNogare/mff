import static java.lang.System.out;

import java.io.File;

import game.*;
import controllers.ghosts.game.GameGhosts;
import controllers.pacman.*;
import tournament.EvaluateAgent;

public class MsPacMan {
    static void usage() {
        out.println("usage: mspac [<agent-classname>] [<option>...]");
        out.println("options:");
        out.println("  -id <name> : agent ID for reporting");
        out.println("  -level <num> : starting level");
        out.println("  -resultdir <path> : directory for results in CSV format");
        out.println("  -seed <num> : random seed");
        out.println("  -sim <count> : simulate a series of games without visualization");
        out.println("  -v : verbose");
        System.exit(1);
    }
    public static void main(String[] args) throws Exception {
        String agentClass = null;
        String agentId = null;
        int level = 1;
        String resultdir = null;
        int seed = 0;
        boolean seedSpecified = false;
        int sim = 0;
        boolean verbose = false;

        for (int i = 0 ; i < args.length ; ++i) {
            String s = args[i];
            switch (s) {
                case "-id":
                    agentId = args[++i];
                    break;
                case "-level":
                    level = Integer.parseInt(args[++i]); 
                    break;
                case "-resultdir":
                    resultdir = args[++i];
                    break;
                case "-seed":
                    seed = Integer.parseInt(args[++i]);
                    seedSpecified = true;
                    break;
                case "-sim":
                    sim = Integer.parseInt(args[++i]);
                    break;
                case "-v":
                    verbose = true;
                    break;
                default:
                    if (s.startsWith("-"))
                        usage();
                    agentClass = s;
            }
        }

		SimulatorConfig config = new SimulatorConfig();
        config.ghostsController = new GameGhosts(4);
        config.game.startingLevel = level;

        if (sim > 0) {
            if (agentClass == null) {
                System.out.println("must specify agent with -sim");
                return;
            }
            
            config.visualize = false;
            EvaluateAgent evaluate =
                new EvaluateAgent(seedSpecified ? seed : 0, config, sim,
                                  resultdir == null ? null : new File(resultdir));

            if (agentId == null)
                agentId = agentClass.substring(agentClass.lastIndexOf(".") + 1);
            evaluate.evaluateAgent(agentId, agentClass, verbose);		
        } else {
            if (agentClass == null)
                config.pacManController = new HumanPacMan();
            else
                config.pacManController = 
                    (IPacManController) Class.forName(agentClass).getConstructor().newInstance();

            config.game.seed = seedSpecified ? seed : -1;
            PacManSimulator.play(config);
        }
    }
}
