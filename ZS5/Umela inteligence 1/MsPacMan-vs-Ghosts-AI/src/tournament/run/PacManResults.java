package tournament.run;

import java.util.ArrayList;
import java.util.List;

import tournament.EvaluationInfos;

public class PacManResults extends EvaluationInfos {
	
	private List<PacManRunResult> runResults = new ArrayList<PacManRunResult>();
	
	public void addRunResults(PacManRunResult... results) {
		for (PacManRunResult result : results) {
			runResults.add(result);
			addResult(result.getInfo());
		}
	}
	
	public List<PacManRunResult> getRunResults() {
		return runResults;
	}
}
