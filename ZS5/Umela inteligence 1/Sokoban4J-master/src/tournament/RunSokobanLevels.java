package tournament;

import java.io.*;
import java.lang.management.ManagementFactory;
import java.lang.management.RuntimeMXBean;
import java.time.LocalDateTime;
import java.util.ArrayList;
import java.util.List;

import game.*;

/**
 * Runs levels executing a SEPARATE JVM for every level sequentially.
 * Stops executing levels once an agent fails to solve the level.
 *  
 * @author Jimmy
 */
public class RunSokobanLevels {
	private String levelset;
	private String agentClass;
	private File resultDir;
	private SokobanConfig config;
    private int maxFail;
	
	public RunSokobanLevels(SokobanConfig config, String agentClass, String levelset,
			                File resultDir, int maxFail) {
		this.config = config;
		this.agentClass = agentClass;
		this.levelset = levelset;
		this.resultDir = resultDir;
        this.maxFail = maxFail;
    }
    
    String idOrAgentClass() {
        return config.id == null ? agentClass : config.id;
    }

    boolean solveLevel(int i) {
        // CONFIGURE PROGRAM PARAMS
        List<String> args = new ArrayList<String>();
        args.add("java");

        args.add("-cp");
        args.add(System.getProperty("java.class.path"));

        // READ JAVA PARAMS
        RuntimeMXBean runtimeMXBean = ManagementFactory.getRuntimeMXBean();
        List<String> jvmArgs = runtimeMXBean.getInputArguments();
        for (String arg : jvmArgs) {
            if (arg.contains("agentlib") && arg.contains("suspend")) {
                // ECLIPSE DEBUGGING, IGNORE
                continue;
            }
            args.add(arg);
        }
        
        args.add("SokobanMain");       // class to run
        
        args.add(agentClass);

        if (config.id != null) {
            args.add("-id");
            args.add(config.id);
        }

        args.add("-levelset");
        args.add(levelset);

        args.add("-level");
        args.add("" + i);

        if (resultDir != null) {
            args.add("-resultdir");
            args.add(resultDir.getAbsolutePath());
        }

        if (config.requireOptimal)
            args.add("-optimal");

        if (config.timeoutMillis > 0) {
            args.add("-timeout");
            args.add("" + config.timeoutMillis);
        }

        if (config.verbose)
            args.add("-v");
        
        ProcessBuilder pb = new ProcessBuilder(args);
        pb.redirectOutput(ProcessBuilder.Redirect.INHERIT);
        pb.redirectErrorStream(true);

        Process p;

        try {
            p = pb.start();
            p.waitFor();
        } catch (Exception e) { throw new Error(e); }

        return p.exitValue() == 0;
    }

	public void run() {
        System.setProperty("java.util.logging.SimpleFormatter.format", "[%4$s] %1$tT.%1$tL %5$s%n");
        LocalDateTime start = LocalDateTime.now();

        int count = SokobanLevel.getLevelCount(Sokoban.findFile(levelset));
        int failed = 0;
		
		for (int i = 1; i <= count; ++i) {			
            if (!solveLevel(i) && ++failed == maxFail)
	    		break;
        }
        
        if (resultDir == null)
            return;

        SokobanResult highest = null;
        try (BufferedReader reader = SokobanResult.openResultFile(resultDir)) {
            while (true) {
                SokobanResult result = SokobanResult.read(reader);
                if (result == null)
                    break;
                if (!result.dateTime.isBefore(start) &&
                    result.getResult() == SokobanResultType.VICTORY) {

                    highest = result;
                    if (!highest.getId().equals(idOrAgentClass()) ||
                        !highest.getLevelFile().equals(levelset)) {
                            throw new Error("unexpected entry in result file");
                        }
                }
            }
        } catch (IOException e) {
            throw new Error(e);
        }

        File summary = new File(resultDir, "summary.csv");
        boolean exists = summary.exists();
        try (PrintWriter writer = new PrintWriter(new FileOutputStream(summary, true))) {
            if (!exists)
                writer.println(
                    "datetime;id;levelFile;timeout;maxFail;requireOptimal;highestSolved;steps;playTimeMillis");
            writer.printf("%s;%s;%s;%s;%s;%s;",
                          start, idOrAgentClass(), levelset,
                          config.timeoutMillis, maxFail, config.requireOptimal);
            if (highest != null)
                writer.printf(
                    "%s;%s;%s\n",
                    highest.getLevel(), highest.getSteps(), highest.getSimTimeMillis());
            else
                writer.println("-1;-1;-1");
        } catch (IOException e) {
            throw new Error(e);
        }
	}
}
