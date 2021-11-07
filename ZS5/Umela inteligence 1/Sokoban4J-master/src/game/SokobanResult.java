package game;

import java.io.*;
import java.time.LocalDateTime;

public class SokobanResult {
    public LocalDateTime dateTime;
	private String id = null;
    private String levelFile;
    private int level;
    private long timeout;
    private boolean requireOptimal;
	private SokobanResultType result = null;
	private Throwable exception;
	private int steps = 0;
	private long simTimeMillis;
    public String message;

    public SokobanResult(SokobanConfig config) {
        dateTime = LocalDateTime.now();
        id = config.id;
        levelFile = config.levelFileName();
        level = config.levelNumber;
        timeout = config.timeoutMillis;
        requireOptimal = config.requireOptimal;
    }

    public SokobanResult(String line) {
        parse(line);
    }

	/**
	 * Assigned ID given to this simulation.
	 */
	public String getId() {
		return id;
	}

	public void setId(String id) {
		this.id = id;
	}

    public String getLevelFile() {
        return levelFile;
    }

    public void setLevelFile(File levelFile) {
        this.levelFile = levelFile.getName();
    }

	public int getLevel() {
		return level;
	}

	public void setLevel(int level) {
		this.level = level;
    }
    
    public boolean getRequireOptimal() {
        return requireOptimal;
    }

    public void setRequireOptimal(boolean optimal) {
        requireOptimal = optimal;
    }

	/**
	 * Result of the simulation.
	 */
	public SokobanResultType getResult() {
		return result;
	}

	public void setResult(SokobanResultType result) {
		this.result = result;
	}

	/**
	 * How many steps an agent performed.
	 */
	public int getSteps() {
		return steps;
	}

	public void setSteps(int steps) {
		this.steps = steps;
	}

	/**
	 * Time the simulation started in milliseconds (obtained via {@link System#currentTimeMillis()}.
	 */
	public long getSimTimeMillis() {
		return simTimeMillis;
	}

	public void setSimTimeMillis(long simTimeMillis) {
		this.simTimeMillis = simTimeMillis;
	}

	/**
	 * Exception caught during the simulation; 
	 */
	public Throwable getException() {
		return exception;
	}

	public void setException(Throwable exception) {
		this.exception = exception;
	}
	
	@Override
	public String toString() {
		return "SokobanResult[" + getResult() + "]";
	}
    
    static final String Filename = "levels.csv";

    static final String Headers =
        "datetime;id;levelFile;timeout;requireOptimal;levelNumber;result;steps;playTimeMillis";

    public void outputResult(String resultDir) {
        new File(resultDir).mkdirs();
        File resultFile = new File(resultDir, "levels.csv");
        boolean header = !resultFile.exists();
        
        try (PrintWriter writer = new PrintWriter(new FileOutputStream(resultFile, true))) {
			if (header) {
                writer.println(Headers);
			}
            writer.println(dateTime + ";" + id + ";" +
                           levelFile + ";" + timeout + ";" +
                           requireOptimal + ";" + level + ";" + result + ";" + steps + ";" +
                           simTimeMillis);
		} catch (IOException e) {
            throw new RuntimeException("Failed to append to the result file: " +
                                       resultFile.getAbsolutePath());
		}
    }

    void parse(String line) {
        String[] fields = line.split(";");
        dateTime = LocalDateTime.parse(fields[0]);
        id = fields[1];
        levelFile = fields[2];
        timeout = Long.parseLong(fields[3]);
        requireOptimal = Boolean.parseBoolean(fields[4]);
        level = Integer.parseInt(fields[5]);
        result = SokobanResultType.valueOf(fields[6]);
        steps = Integer.parseInt(fields[7]);
        simTimeMillis = Long.parseLong(fields[8]);
    }

    public static BufferedReader openResultFile(File resultDir) throws IOException {
        File resultFile = new File(resultDir, "levels.csv");
        BufferedReader reader = new BufferedReader(new FileReader(resultFile));
        String line = reader.readLine();
        if (!line.equals(Headers))
            throw new Error("incompatible file format");
        return reader;
    }

    public static SokobanResult read(BufferedReader reader) throws IOException {
        String line = reader.readLine();
        return line == null ? null : new SokobanResult(line);
    }
}
