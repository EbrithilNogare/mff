package csp;

import java.util.*;

// A constraint that says that exactly a certain number (count) of a certain set
// of variables are true.
public class Constraint {
    public int count;
    public List<Integer> vars;

    public Constraint(int count, List<Integer> vars) {
        this.count = count;  this.vars = vars;
    }

    @Override
    public String toString() {
        ArrayList<String> varNums = new ArrayList<String>();
        for (int v : vars)
            varNums.add(Integer.toString(v));

        return String.format("%d of {%s}", count, String.join(" ", varNums));
    }
}
