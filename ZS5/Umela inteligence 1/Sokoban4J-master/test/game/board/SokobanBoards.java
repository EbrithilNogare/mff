package game.board;

import java.io.File;
import java.util.ArrayList;
import java.util.List;

import game.board.oop.Board;

public class SokobanBoards {

	public static List<Board> loadBoards() {
		List<Board> boards = new ArrayList<Board>();
		
		File sokFile = new File("Levels/A.K.K._Informatika.sok");
		
		int levelNumber = 1;
		while (true) {			
			Board board;
			
			try {
				board = Board.fromFileSok(sokFile, levelNumber);
				if (board != null) {
					boards.add(board);
				} else {
					break;
				}
			} catch (Exception e) {
				break;
			}
			++levelNumber;
		}
		
		return boards;
	}
	
	
}
