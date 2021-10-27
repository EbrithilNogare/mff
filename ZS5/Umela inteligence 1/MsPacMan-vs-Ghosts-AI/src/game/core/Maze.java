package game.core;

import java.io.BufferedReader;
import java.io.IOException;
import java.io.InputStreamReader;

/*
 * Stores the actual mazes, each of which is simply a connected graph. The differences between the mazes are the connectivity
 * and the x,y coordinates (used for drawing or to compute the Euclidean distance. There are 3 built-in distance functions in
 * total: Euclidean, Manhattan and Dijkstra's shortest path distance. The latter is pre-computed and loaded, the others are
 * computed on the fly whenever getNextDir(-) is called.
 */
final class Maze
{
	private String[] nodeNames={"a","b","c","d"};
	private String[] distNames={"da","db","dc","dd"};
    
    //Information for the controllers
    protected int[] distances,pillIndices,powerPillIndices,junctionIndices;				
    
    protected Node[] graph;			//The actual maze, stored as a graph (set of nodes)
    
    //Maze-specific information
    protected int initialPacPosition,lairPosition,initialGhostsPosition,width,height;	
    
	protected String name;			//Name of the Maze
				
	/*
	 * Each maze is stored as a (connected) graph: all nodes have neighbours, stored in an array of length 4. The
	 * index of the array associates the direction the neighbour is located at: '[up,right,down,left]'.
	 * For instance, if node '9' has neighbours '[-1,12,-1,6]', you can reach node '12' by going right, and node
	 * 6 by going left. The directions returned by the controllers should thus be in {0,1,2,3} and can be used
	 * directly to determine the next node to go to.
	 */		
	protected Maze(int index)
	{
		loadNodes(nodeNames[index]);
		loadDistances(distNames[index]);
	}
	
	//Loads all the nodes from files and initialises all maze-specific information.
	private void loadNodes(String fileName)
	{
        try
        {         	
        	BufferedReader br=new BufferedReader(new InputStreamReader(this.getClass().getResourceAsStream("resources/data/"+fileName)));
            String input=br.readLine();		
            
            //preamble
            String[] pr=input.split("\t");       
            this.name=pr[0];
            this.initialPacPosition=Integer.parseInt(pr[1]);
            this.lairPosition=Integer.parseInt(pr[2]);
            this.initialGhostsPosition=Integer.parseInt(pr[3]);	            
            this.graph=new Node[Integer.parseInt(pr[4])];	            
            this.pillIndices=new int[Integer.parseInt(pr[5])];
            this.powerPillIndices=new int[Integer.parseInt(pr[6])];
            this.junctionIndices=new int[Integer.parseInt(pr[7])];
            this.width=Integer.parseInt(pr[8]);
            this.height=Integer.parseInt(pr[9]);
            
            input=br.readLine();	

            int nodeIndex=0;
        	int pillIndex=0;
        	int powerPillIndex=0;	        	
        	int junctionIndex=0;

            while(input!=null)
            {	
                String[] nd=input.split("\t");    
                Node node=new Node(nd[0],nd[1],nd[2],nd[7],nd[8],new String[]{nd[3],nd[4],nd[5],nd[6]});
                
                graph[nodeIndex++]=node;
                
                if(node.pillIndex>=0)
                	pillIndices[pillIndex++]=node.nodeIndex;
                else if(node.powerPillIndex>=0)
                	powerPillIndices[powerPillIndex++]=node.nodeIndex;
                
                if(node.numNeighbours>2)
                	junctionIndices[junctionIndex++]=node.nodeIndex;
                
                input=br.readLine();
            }
        }
        catch(IOException ioe)
        {
            ioe.printStackTrace();
        }
	}
	
	/*
	 * Loads the shortest path distances which have been pre-computed. The data contains the shortest distance from
	 * any node in the maze to any other node. Since the graph is symmetric, the symmetries have been removed to preserve
	 * memory and all distances are stored in a 1D array; they are looked-up using getDistance(-). 
	 */		
	private void loadDistances(String fileName)
	{
		this.distances=new int[((graph.length*(graph.length-1))/2)+graph.length];
		
        try
        {	        		        	
        	BufferedReader br=new BufferedReader(new InputStreamReader((this.getClass().getResourceAsStream("resources/data/"+fileName))));
        	
            String input=br.readLine();
            
            int index=0;
            
            while(input!=null)
            {	
            	distances[index++]=Integer.parseInt(input);
                input=br.readLine();
            }
        }
        catch(IOException ioe)
        {
            ioe.printStackTrace();
        }
	}
}
