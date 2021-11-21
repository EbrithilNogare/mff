package minesweeper4j.game;

public class Pos {

	public int x;
	public int y;
	
	private int hashCode;
	
	public Pos(int x, int y) {
		this.x = x;
		this.y = y;
		hashCode = x * 290317 + y * 97;
	}
	
	@Override
	public int hashCode() {
		return hashCode;
	}
	
	@Override
	public boolean equals(Object obj) {
		if (obj == null) return false;
		if (hashCode != obj.hashCode()) return false;
		if (!(obj instanceof Pos)) return false;
		Pos other = (Pos)obj;
		return x == other.x && y == other.y;
	}
	
	@Override
	public String toString() {
		return "Pos[" + x + "," + y + "]";
	}
	
}
