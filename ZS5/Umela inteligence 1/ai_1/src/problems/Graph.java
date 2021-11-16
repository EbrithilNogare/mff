package problems;

import java.util.*;

import search.Problem;

// A tiny undirected graph, viewed as a state search problem.
// The minimum-cost solution is 6.0, passing through vertices
//    0 -> 2 -> 1 -> 3 -> 4.

class Edge {
  int dest;
  int weight;

  public Edge(int dest, int weight) {
    this.dest = dest; this.weight = weight;
  }
}

public class Graph implements Problem<Integer, Edge> {
  // adjacency list for each vertex
  ArrayList<ArrayList<Edge>> adj = new ArrayList<ArrayList<Edge>>();
  
  public Graph() {
    for (int i = 0 ; i < 5 ; ++i)
      adj.add(new ArrayList<Edge>());

    // build the graph
    edge(0, 1, 5);
    edge(0, 2, 3);
    edge(1, 2, 1);
    edge(1, 3, 1);
    edge(1, 4, 3);
    edge(2, 3, 4);
    edge(3, 4, 1);
  }

  void edge(int v, int w, int weight) {
    adj.get(v).add(new Edge(w, weight));
    adj.get(w).add(new Edge(v, weight));
  }

  public Integer initialState() { return 0; }

  public List<Edge> actions(Integer state) { return adj.get(state); }

  public Integer result(Integer state, Edge action) { return action.dest; }

  public boolean isGoal(Integer state) { return state == 4; }

  public double cost(Integer state, Edge action) { return action.weight; }
}
