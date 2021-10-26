package csp;

import java.util.*;

// A constraint satisfaction problem over Boolean variables.
public class BooleanCSP {
    public int numVars;                    // variables are numbered 0 .. (numVars - 1)
    public Boolean[] value;                // value of each variable, or null if unknown
    public Set<Constraint> constraints;    // all constraints

    public ArrayList<Set<Constraint>> varConstraints;  // constraints affecting each variable
    public Queue<Constraint> unchecked;    // constraints not yet checked by forward checking

    public BooleanCSP(int numVars) {
        this.numVars = numVars;

        value = new Boolean[numVars];

        constraints = new HashSet<Constraint>();

        varConstraints = new ArrayList<Set<Constraint>>();
        for (int v = 0 ; v < numVars ; ++v)
            varConstraints.add(new HashSet<Constraint>());

        unchecked = new ArrayDeque<Constraint>();
    }

    public void addConstraint(Constraint c) {
        constraints.add(c);
        for (int v : c.vars)
            constraints(v).add(c);
        unchecked.add(c);
    }

    public Set<Constraint> constraints(int var) {
        return varConstraints.get(var);
    }

    public void set(int var, boolean val) {
        value[var] = val;
        unchecked.addAll(constraints(var));    
    }

    public void reset(Collection<Integer> vars) {
        for (int v : vars)
            value[v] = null;
    }

    @Override
    public String toString() {
        ArrayList<String> a = new ArrayList<String>();
        for (Constraint c : constraints)
            a.add(c.toString());
        return numVars + " vars: " + String.join(", ", a);
    }
}
