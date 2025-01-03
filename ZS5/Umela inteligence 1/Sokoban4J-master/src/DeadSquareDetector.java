import java.util.ArrayList;
import java.util.Arrays;
import java.util.List;

import game.actions.EDirection;
import game.board.compact.BoardCompact;
import game.board.compact.CTile;

class DeadSquareDetector{
	public static boolean[][] detect(BoardCompact board){
		boolean[][] deaths = new boolean[board.width()][board.height()];
		for (boolean[] row: deaths)
    		Arrays.fill(row, true);

		List<Pair<Integer,Integer>> goals = new ArrayList<Pair<Integer,Integer>>();

		for (int x = 1; x < board.width()-1; x++)
			for (int y = 1; y < board.height()-1; y++)
				if(CTile.forSomeBox(board.tile(x,y)))
					goals.add(new Pair<Integer,Integer>(x, y));
				

		List<Pair<Integer, Integer>> toBeSearched = new ArrayList<Pair<Integer, Integer>>();
		for(Pair<Integer, Integer> goal : goals)
			toBeSearched.add(goal);
		

		while(toBeSearched.size() != 0){
			int x = toBeSearched.get(0).first;
			int y = toBeSearched.get(0).second;
			if(!deaths[x][y]){
				toBeSearched.remove(0);
				continue;
			}
			deaths[x][y] = false;
			
			for (EDirection dir : EDirection.arrows())
			if(!CTile.isWall(board.tile(x + dir.dX, y + dir.dY)) && !CTile.isWall(board.tile(x + 2*dir.dX, y + 2*dir.dY)))
				toBeSearched.add(new Pair<Integer, Integer>(x + dir.dX, y + dir.dY));
		}

		return deaths;
	}
}

class Pair<U, V> {
    public U first;
    public V second;
	Pair(U u, V v){
		first = u;
		second = v;
	}
}