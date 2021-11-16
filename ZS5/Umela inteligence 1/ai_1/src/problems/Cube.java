package problems;

import java.util.*;

import search.HeuristicProblem;

// A simple problem involving movement in a cube.
//
// The start position is (1000, 1000, 1000).  The goal is to get to (0, 0, 0).  At each step
// the following moves are possible:
//
// - decrease X by 1 (cost: 1000)
// - decrease Y by 1 (cost: X)
// - decrease Z by 1 (cost: max(X, Y))
//
// The optimal strategy from any point is to first decrease X to 0, then decrease Y, then decrease Z.
// So the optimal cost from the start position of (1000, 1000, 1000) is 1,000,000.
// 
// Uninformed uniform-cost search will be very expensive since it will explore
// millions of positions before finding the goal. With the right heuristic,
// A* will find the goal immediately, expanding exactly 3000 nodes.

class CPos {
    int x, y, z;

    CPos(int x, int y, int z) {
        this.x = x; this.y = y; this.z = z;
    }

    @Override
    public boolean equals(Object o) {
        if (!(o instanceof CPos))
            return false;

        CPos q = (CPos) o;
        return x == q.x && y == q.y && z == q.z;
    }

    @Override
    public int hashCode() { return x + y + z; }
}

public class Cube implements HeuristicProblem<CPos, Integer> {

    public CPos initialState() {
        return new CPos(1000, 1000, 1000);
    }

    public List<Integer> actions(CPos state) {
        List<Integer> l = new ArrayList<Integer>();
        if (state.x > 0)
            l.add(1);
        if (state.y > 0)
            l.add(2);
        if (state.z > 0)
            l.add(3);
        return l;
    }

    public CPos result(CPos s, Integer action) {
        switch (action) {
        case 1: return new CPos(s.x - 1, s.y, s.z);
        case 2: return new CPos(s.x, s.y - 1, s.z);
        case 3: return new CPos(s.x, s.y, s.z - 1);

        default: throw new Error("unknown action");
        }
    }

    public boolean isGoal(CPos state) {
        return state.x == 0 && state.y == 0 && state.z == 0;
    }

    public double cost(CPos state, Integer action) {
        switch (action) {
        case 1: return 1000;
        case 2: return state.x;
        case 3: return Math.max(state.x, state.y);

        default: throw new Error("unknown action");
        }
    }

    public double estimate(CPos state) {
        // The heuristic (1000 * state.x) is optimal and will lead to the goal immediately.
        //
        // A non-optimal heuristic such as (980 * state.x) will also lead to the goal pretty quickly.
        // Performance will worsen as you decrease the constant.
        // 
        // A heuristic of 0 degenerates to uniform-cost search, which will be hopelessly slow.

        return 1000 * state.x;
    }
}
