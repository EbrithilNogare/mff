package tournament.run;

import java.time.LocalDateTime;

import game.core.Game;

public class PacManRunResult {
    public LocalDateTime dateTime;
    private int seed;	
    private Game info;
		
	public PacManRunResult(int seed, Game info) {
        dateTime = LocalDateTime.now();
        this.seed = seed;
        this.info = info;
    }
    
    public Game getInfo() { return info; }
	
	public String getCSVHeader() {
		return "seed;levelReached;score;timeSpent";
	}
	
	public String getCSV() {
        return seed + ";" + info.getCurLevel() + ";" + info.getScore() + ";" + info.getTotalTime();
	}
	
}
