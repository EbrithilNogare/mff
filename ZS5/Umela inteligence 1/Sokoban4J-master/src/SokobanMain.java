import static java.lang.System.out;

import java.io.File;

import game.*;
import tournament.*;

public class SokobanMain {
    static String describe(SokobanResultType type) {
        switch (type) {
            case VICTORY: return "solved";
            case TIMEOUT: return "TIMEOUT";
            default: return "FAILED";
        }
    }

    public static SokobanResultType runLevel(
        IAgent agent, String agentId, String levelset, int level,
        String resultDir, int timeout, boolean verbose, boolean optimal) {

        agent.init(optimal, verbose);

        if (verbose)
            System.out.println("====================");
        System.out.printf("solving level %d... ", level);
        if (verbose)
            System.out.println();

        SokobanResult result =
            Sokoban.simAgentLevel(agentId, levelset, level, timeout, agent, verbose, optimal);

        SokobanResultType resultType = result.getResult();
        System.out.printf("%s in %.1f ms",
            describe(resultType), (double) result.getSimTimeMillis());

        if (resultType == SokobanResultType.VICTORY)
            System.out.printf(" (%d steps)", result.getSteps());
        if (result.message != null)
            System.out.printf(" (%s)", result.message);

        System.out.println();

        if (resultDir != null) {
            result.outputResult(resultDir);
        }

        return resultType;
    }

    static void runLevelSet(String agentId, String className, String levelset, int maxFail, String resultDir,
                            int timeout, boolean verbose, boolean optimal) {
        System.out.printf("Running %s on levels in %s\n", className, levelset);

        SokobanConfig config = new SokobanConfig();
        config.id = agentId;
        config.requireOptimal = optimal;
        config.timeoutMillis = timeout;
        config.verbose = verbose;

        RunSokobanLevels run = new RunSokobanLevels(
            config, className, levelset,
            resultDir == null ? null : new File(resultDir), maxFail);
        run.run();
    }

    static void usage() {
        out.println("usage: sokoban [<agent-classname>] [<option>...]");
        out.println("options:");
        out.println("  -id <name> : agent ID for reporting");
        out.println("  -level <num> : level number to play");
        out.println("  -levelset <name> : set of levels to play");
        out.println("  -maxfail <num> : maximum level failures allowed");
        out.println("  -optimal : require move-optimal solutions");
        out.println("  -resultdir <path> : directory for results in CSV format");
        out.println("  -timeout <num> : maximum thinking time in milliseconds");
        out.println("  -v : verbose output");
        System.exit(1);
    }

	public static void main(String[] args) throws Exception {
        String agentId = null;
        String className = null;
        String levelset = "easy.sok";
        int level = 0;
        int maxFail = 0;
        boolean optimal = false;
        String resultDir = null;
        int timeout = 0;
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
                case "-levelset":
                    levelset = args[++i];
                    if (levelset.indexOf('.') == -1)
                        levelset += ".sok";
                    break;
                case "-maxfail":
                    maxFail = Integer.parseInt(args[++i]);
                    break;
                case "-optimal":
                    optimal = true;
                    break;
                case "-resultdir":
                    resultDir = args[++i];
                    break;
                case "-timeout":
                    timeout = Integer.parseInt(args[++i]);
                    break;
                case "-v":
                    verbose = true;
                    break;
                default:
                    if (s.startsWith("-"))
                        usage();
                    className = s;
            }
        }

        if (className == null)
            if (level > 0)
                Sokoban.playHumanLevel(levelset, level);
            else
                Sokoban.playHumanFile(levelset);
        else
            if (level > 0) {
                IAgent agent = (IAgent) Class.forName(className).getConstructor().newInstance();
                SokobanResultType resultType = runLevel(
                    agent, agentId, levelset, level, resultDir, timeout, verbose, optimal);
                System.exit(resultType.getExitValue());	    	    
            }
            else
                runLevelSet(agentId, className, levelset, maxFail,
                            resultDir, timeout, verbose, optimal);
    }
}
