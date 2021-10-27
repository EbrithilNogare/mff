package game.core;

/*
 * Stores all information relating to a node in the graph, including all the indices required
 * to check and update the current state of the game.
 */
final class Node
{
	protected int x,y,nodeIndex,pillIndex,powerPillIndex,numNeighbours;
	protected int[] neighbours;
	
	protected Node(String nodeIndex,String x,String y,String pillIndex,String powerPillIndex,String[] neighbours)
	{
		this.nodeIndex=Integer.parseInt(nodeIndex);
		this.x=Integer.parseInt(x);
		this.y=Integer.parseInt(y);
		this.pillIndex=Integer.parseInt(pillIndex);
		this.powerPillIndex=Integer.parseInt(powerPillIndex);		
		
		this.neighbours=new int[neighbours.length];
		
		for(int i=0;i<neighbours.length;i++)
		{
			this.neighbours[i]=Integer.parseInt(neighbours[i]);
		
			if(this.neighbours[i]!=-1)
				numNeighbours++;
		}
	}
}
