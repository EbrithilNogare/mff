import java.io.File;

import game.board.compact.*;
import game.board.oop.*;

public class DeadSquareTest {
    static void printLevelWithTargets(Board board) {
		for (int y = 0; y < board.height; ++y) {
			for (int x = 0; x < board.width; ++x) {
				EPlace place = board.tiles[x][y].place;
				ESpace space = board.tiles[x][y].space;
				
				if (place == EPlace.BOX_1) {
					System.out.print('.');
				} else
				if (space != null) {
					System.out.print(space.getSymbol());
				} else {
					System.out.print("?");
				}
			}
			System.out.println();
		}
    }

    public static void main(String[] args) {
        File levels = new File("C:/git/mff/ZS5/Umela inteligence 1/Sokoban4J-master/levels/Aymeric_du_Peloux_1_Minicosmos.sok");
        if (!levels.canRead()) {
            System.out.printf("can't find level file %s\n", levels.getAbsolutePath());
            return;
        }

        System.out.printf("testing levels in %s\n\n", levels.getName());
        for (int i = 1 ; i <= 10 ; ++i) {
            System.out.printf("== level %d ==\n\n", i);
            Board board = Board.fromFileSok(levels, i);

            printLevelWithTargets(board);
            System.out.println();

            BoardCompact bc = board.makeBoardCompact();

            boolean[][] dead = DeadSquareDetector.detect(bc);
            
            System.out.println("dead squares: \n");
            for (int y = 0 ; y < bc.height() ; ++y) {
                for (int x = 0 ; x < bc.width() ; ++x)
                    System.out.print(CTile.isWall(bc.tile(x, y)) ? '#' : (dead[x][y] ? 'X' : '_'));
                System.out.println();
            }
            System.out.println();
        }
    }
}