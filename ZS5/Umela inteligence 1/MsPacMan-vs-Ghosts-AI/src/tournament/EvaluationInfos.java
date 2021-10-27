package tournament;

import game.core.Game;

import java.util.ArrayList;
import java.util.List;

public class EvaluationInfos {
	private List<Game> results = new ArrayList<Game>();
	
    public int totalScore;
    public int totalLevelReached;
	public int totalTimeSpent;
		
	public List<Game> getResults() {
		return results;
	}

    double avg(int i) {
        return (double) i / results.size();
    }

    public double avgScore() {
        return avg(totalScore);
    }

	public void addResult(Game result) {
        totalScore     += result.getScore();
        totalLevelReached += result.getCurLevel();		
		totalTimeSpent += result.getTotalTime();
		
		results.add(result);
	}
	
	public void addResults(List<Game> results) {
		for (Game info : results) {
			addResult(info);
		}
	}
	
	public String getCSVHeader() {
		return "games;avgScore;avgLevelReached;avgTimeSpent";
	}
	
	public String getCSV() {
        return String.format("%d;%.2f;%.2f;%.2f",
            results.size(), avgScore(), avg(totalLevelReached), avg(totalTimeSpent));
	}
	
	@Override
	public String toString() {
		return String.format(
            "avg level reached = %.1f, avg score = %.1f",
            avg(totalLevelReached), avg(totalScore));
	}
	
}
