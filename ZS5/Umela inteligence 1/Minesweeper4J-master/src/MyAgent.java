import minesweeper4j.agents.ArtificialAgent;
import minesweeper4j.game.Action;
import minesweeper4j.game.Board;

public class MyAgent extends ArtificialAgent {
	
	/* Code your custom agent here.  The existing dummy implementation always asks for a hint.
     *
     * The Board object passed to thinkImpl gives you the current board state.
     */
			
	@Override
	protected Action thinkImpl(Board board, Board previousBoard) {
		return Action.advice();
	}
	
}
