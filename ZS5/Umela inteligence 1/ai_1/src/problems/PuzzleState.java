package problems;

import java.util.ArrayList;
import java.util.Arrays;
import java.util.List;
import java.util.Random;

public class PuzzleState {
	final int size;
	final int[] squares;
	final int empty;  // index of empty square 
	
	public PuzzleState(int size, int[] squares, int empty) {
		this.size = size; this.squares = squares; this.empty = empty;
	}
	
	static int findEmpty(int[] a) {
		for (int i = 0 ; i < a.length ; ++i)
			if (a[i] == 0)
				return i;
		throw new Error("no empty square");
	}
	
	public PuzzleState(int size, int[] squares) {
		this(size, squares, findEmpty(squares));
	}
	
	// Construct a puzzle where all the tiles are in reverse order.
	public static PuzzleState reversed(int size) {
		int[] a = new int[size * size];
		
		for (int i = 0 ; i < size * size ; ++i)
			a[i] = size * size - 1 - i;
		
		return new PuzzleState(size, a);
	}

	// Construct a puzzle by making a number of random moves from the goal state.
	public static PuzzleState random(int size, int num) {
		Random rand = new Random();
		
		int[] a = new int[size * size];
		for (int i = 0 ; i < size * size ; ++i)
			a[i] = i;
		PuzzleState state = new PuzzleState(size, a);
		
		for (int i = 1 ; i <= num ; ++i) {
			List<Dir> l = state.possibleDirections();
			int which = rand.nextInt(l.size());
			state = state.slide(l.get(which));
		}

		return state;
	}	

	@Override
	public boolean equals(Object o) {
		if (o instanceof PuzzleState) {
			PuzzleState s = (PuzzleState) o;
			return Arrays.equals(squares, s.squares);
		}
		
		return false;
	}
	
	@Override
	public int hashCode() {
		return Arrays.hashCode(squares);
	}
	
	public List<Dir> possibleDirections() {
		List<Dir> dirs = new ArrayList<Dir>();
		int r = empty / size;
		int c = empty % size;
		
		if (r > 0)
			dirs.add(Dir.Down);
		if (r < size - 1)
			dirs.add(Dir.Up);
		if (c > 0)
			dirs.add(Dir.Right);
		if (c < size - 1)
			dirs.add(Dir.Left);
		
		return dirs;
	}
	
	public PuzzleState slide(Dir dir) {
		int d;
		
		switch (dir) {
		case Left: d = -1; break;
		case Right: d = 1; break;
		case Up: d = - size; break;
		case Down: d = size; break;
		default: throw new Error();
		}
		
		int[] s = squares.clone();
		s[empty] = s[empty - d];
		s[empty - d] = 0;
		return new PuzzleState(size, s, empty - d);
	}
	
	public boolean isGoal() {
		for (int i = 0 ; i < size * size ; ++i)
			if (squares[i] != i)
				return false;
		return true;
	}
	
	@Override
	public String toString() {
		StringBuffer sb = new StringBuffer();
		
		for (int i = 0 ; i < size ; ++i) {
			for (int j = 0 ; j < size ; ++j)
				sb.append(String.format("%d ", squares[i * size + j]));
			sb.append("\n");
		}
		
		return sb.toString();
	}
}
