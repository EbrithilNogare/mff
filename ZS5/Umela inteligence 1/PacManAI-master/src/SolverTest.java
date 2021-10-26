import static java.lang.System.out;
import java.util.*;

import csp.*;

class Assignment {
    int var;
    boolean value;

    public Assignment(int var, boolean value) {
        this.var = var;  this.value = value;
    }
}

class SolverTest {
    Solver solver = new Solver();

    void fail(String s) {
        System.out.println(s);
        System.exit(0);
    }

    // Some small problems that are solvable by forward checking.
    String[] easy = { 
        "1 var: 1 of {0}",
        "1 var: 0 of {0}",
        "3 vars: 1 of {0}, 1 of {1}, 0 of {2}",
        "4 vars: 1 of {0 1}, 1 of {1 2}, 1 of {2 3}, 0 of {3}",
        "4 vars: 2 of {0 1 2 3}, 0 of {0 3}",
        "5 vars: 2 of {0 1 2 3 4}, 2 of {1 4}",
        "8 vars: 1 of {0 1 3 4}, 2 of {0 1 2 4 5 6 7}, 2 of {1 5}",
        "8 vars: 3 of {0 2 3 4 5 7}, 2 of {1 3 6 7}, 3 of {3 4 7}"
    };

    // These problems can't be solved by forward checking, but they are all satisfiable,
    // so a backtracking search should find a solution for all of them.  Also, some
    // inferences are possible in most of them.
    String[] harder = {
        "4 vars: 1 of {0 1}, 1 of {1 2}, 1 of {2 3}",

        "4 vars: 1 of {0 1}, 1 of {1 2}, 1 of {2 3}, 2 of {0 2 3} / 0=T, 1=F, 2=T, 3=F",

        // a situation from a 3 x 3 Minesweeper board
        "9 vars: 0 of {2 5 6}, 1 of {1 4 5}, 1 of {1 2 4 7 8}, 1 of {3 4 7} / 7=F, 8=F",

        // a situation from a 4 x 4 Minesweeper board
        "16 vars: 0 of {0 1 2 3 4 5 6 7}, 1 of {0 1 5 8 9}, 1 of {0 1 2 4 6 8 9 10}, " +
                 "1 of {1 2 3 5 7 9 10 11}, 1 of {2 3 6 10 11} / 8=T, 9=F, 10=F, 11=T",

        // a situation from a 4 x 4 Minesweeper board
        "16 vars: 0 of {4 5 8 9 12 13}, 1 of {0 1 5 8 9}, 2 of {0 1 2 4 6 8 9 10}, " +
                 "2 of {4 5 6 8 10 12 13 14}, 2 of {8 9 10 12 14} / 2=F, 6=F, 10=T, 14=T",

        // a situation from a 5 x 5 Minesweeper board
        "25 vars: 0 of {10 11 12 13 14 15 16 18 19 20 21 23 24}, " +
                 "1 of {5 6 11 15 16}, 2 of {5 6 7 10 12 15 16 17}, " +
                 "1 of {10 11 12 15 17 20 21 22}, 1 of {15 16 17 20 22}, " +
                 "3 of {6 7 8 11 13 16 17 18}, " +
                 "2 of {7 8 9 12 14 17 18 19}, 1 of {8 9 13 18 19}, " +
                 "1 of {12 13 14 17 19 22 23 24}, 1 of {17 18 19 22 24} " +
                 "/ 5=F, 6=T, 8=T, 9=F"
    };

    // More problems where some inferences are possible.
    String[] extra = {
        "8 vars: 1 of {0 1}, 2 of {0 1 2}, 1 of {1 2 3}, 3 of {2 3 4 5 6}, " +
                "2 of {5 6 7}, 1 of {6 7} / 0=T, 1=F, 2=T, 3=F, 5=T",

        "8 vars: 1 of {0 1}, 1 of {0 1 2}, 1 of {1 2 3}, 1 of {2 3 4 5 6}, " +
                "2 of {5 6 7}, 1 of {6 7} / 0=F, 1=T, 2=F, 3=F, 4=F, 5=T, 6=F, 7=T",

        "8 vars: 1 of {0 1}, 2 of {0 1 2}, 2 of {1 2 3 4 5}, 1 of {4 5 6}, " +
                "1 of {5 6 7} / 2=T",

        "7 vars: 1 of {0 1}, 1 of {0 1 2}, 1 of {1 2 3}, 1 of {2 3 4 5 6}, " +
                "1 of {5 6} / 0=F, 1=T, 2=F, 3=F, 4=F"
    };

    // Generate a random CSP that can be solved by forward checking.
    BooleanCSP randomForwardProb(int size) {
        Random random = new Random(0);

        ArrayList<Integer> varMap = new ArrayList<Integer>();
        for (int v = 0 ; v < size ; ++v)
            varMap.add(v);
        Collections.shuffle(varMap);

        ArrayList<Integer> vals = new ArrayList<Integer>();

        BooleanCSP csp = new BooleanCSP(size);
        int n = 0;
        while (n < size) {
            int prev = Math.min(n, 1 + random.nextInt(4));
            int newVars = Math.min(size - n, 1 + random.nextInt(4));
            
            int sum = 0;
            ArrayList<Integer> cvars = new ArrayList<Integer>();
            while (cvars.size() < prev) {
                int i = random.nextInt(prev);
                if (!cvars.contains(varMap.get(i))) {
                    cvars.add(varMap.get(i));
                    sum += vals.get(i);
                }
            }

            int val = sum == 0 ? 1 : sum == prev ? 0 : random.nextInt(2);
            for (int i = 0 ; i < newVars ; ++i) {
                cvars.add(varMap.get(n + i));
                vals.add(val);
                sum += val;
            }

            Collections.sort(cvars);
            csp.addConstraint(new Constraint(sum, cvars));
            n += newVars;
        }

        return csp;
    }

    // Generate a random CSP that is satisfiable.
    BooleanCSP randomSatisfiable(int size) {
        Random random = new Random(0);

        Boolean[] vals = new Boolean[size];
        for (int v = 0 ; v < size ; ++v)
            vals[v] = random.nextInt(2) == 1;
        out.print("actual values: ");
        printValues(vals);
        
        BooleanCSP csp = new BooleanCSP(size);
        for (int i = 0 ; i < 2 * size / 3; ++i) {
            int count = Math.min(2 + random.nextInt(4), size);
            ArrayList<Integer> vars = new ArrayList<Integer>();

            int sum = 0;
            while (vars.size() < count) {
                int v = random.nextInt(size);
                if (!vars.contains(v)) {
                    vars.add(v);
                    sum += vals[v] ? 1 : 0;
                }
            }

            csp.addConstraint(new Constraint(sum, vars));
        }

        System.out.println(csp);
        return csp;
    }

    BooleanCSP parse(String s, List<Assignment> inferences) {
        String[] top = s.split("/");

        String[] parts = top[0].split(":");
        String v = parts[0];
        int i = v.indexOf(' ');
        int numVars = Integer.parseInt(v.substring(0, i));

        BooleanCSP csp = new BooleanCSP(numVars);

        String[] constraints = parts[1].split(",");
        for (String c : constraints) {
            c = c.trim();
            i = c.indexOf(' ');
            int count = Integer.parseInt(c.substring(0, i));

            int j = c.indexOf('{'), k = c.indexOf('}');
            String[] nums = c.substring(j + 1, k).split(" +");
            ArrayList<Integer> vars = new ArrayList<Integer>();
            for (String n : nums)
                vars.add(Integer.parseInt(n));
            csp.addConstraint(new Constraint(count, vars));
        }

        if (top.length > 1 && inferences != null) {
            String[] assignments = top[1].split(",");
            for (String a : assignments) {
                i = a.indexOf('=');
                int var = Integer.parseInt(a.substring(0, i).trim());
                String b = a.substring(i + 1).trim();
                inferences.add(new Assignment(var, b.equals("T")));
            }
        }

        return csp;
    }

    void printValues(Boolean[] a) {
        for (int v = 0 ; v < a.length ; ++v) {
            if (v > 0)
                System.out.print(", ");
            if (v == 100) {
                System.out.print("...");
                break;
            }

            Boolean b = a[v];
            String s = b == null ? "X" : b ? "T" : "F";
            System.out.printf("%d = %s", v, s);
        }
        System.out.println();
    }

    boolean checkSolved(BooleanCSP csp) {
        for (int v = 0 ; v < csp.numVars ; ++v)
            if (csp.value[v] == null && !csp.constraints(v).isEmpty()) {
                out.println("no value for var " + v);
                return false;
            }
        
        for (Constraint c : csp.constraints) {
            int count = 0;
            for (int v : c.vars)
                if (csp.value[v])
                    count += 1;
            
            if (count != c.count) {
                out.println("constraint not satisfied");
                return false;
            }
        }

        return true;
    }

    boolean test_forward(BooleanCSP csp) {
        List<Integer> found = solver.forwardCheck(csp);
        if (found == null) {
            out.println("failed to find a solution");
            return false;
        }
        printValues(csp.value);
        if (found.size() != csp.numVars) {
            out.println("failed to solve all variables");
            return false;
        }
        return checkSolved(csp);
    }

    boolean test_forward_easy() {
        out.println("testing forward checking");
        for (String p : easy) {
            out.println(p);
            if (!test_forward(parse(p, null)))
                return false;
        }
        return true;
    }

    boolean test_forward_random() {
        out.println("testing forward checking on random problems");
        for (int size = 10 ; size <= 100 ; size += 10) {
            BooleanCSP csp = randomForwardProb(size);
            if (!test_forward(csp))
                return false;
        }
        return true;
    }

    boolean test_solve(BooleanCSP csp) {
        List<Integer> found = solver.solve(csp);
        if (found == null) {
            out.println("failed to find a solution");
            return false;
        }
        System.out.printf("found solution: ");
        printValues(csp.value);
        System.out.println();
        return checkSolved(csp);
    }

    boolean test_solve_fixed() {
        out.println("\ntesting solver");

        String[][] lists = { easy, harder };
        for (String[] list : lists)
            for (String p : list) {
                out.println(p);
                if (!test_solve(parse(p, null)))
                    return false;
            }
        
        return true;
    }

    boolean test_solve_random() {
        out.println("testing solver on random problems");
        for (int size = 100 ; size <= 1000 ; size += 100) {
            BooleanCSP csp = randomSatisfiable(size);
            if (!test_solve(csp))
                return false;
        }
        return true;
    }

    boolean test_infer(String[] probs) {
        for (String p : probs) {
            out.println(p);

            ArrayList<Assignment> expected = new ArrayList<Assignment>();
            BooleanCSP csp = parse(p, expected);

            // Repeatedly call forwardCheck() and inferVar() to infer as much as possible.
            while (true) {
                if (solver.forwardCheck(csp) == null) {
                    out.println("forward inference failed");
                    return false;
                }
                if (solver.inferVar(csp) == -1)
                    break;
            }
            printValues(csp.value);

            for (int v = 0 ; v < csp.numVars ; ++v) {
                Boolean b = null;
                for (Constraint c : csp.constraints(v))
                    if (c.count == 0) {
                        b = false;  // all variables in this constraint must be false
                        break;
                    }
                if (b == null)
                    for (Assignment a : expected)
                        if (a.var == v) {
                            b = a.value;
                            break;
                        }
                Boolean c = csp.value[v];
                if (b == null && c != null) {
                    out.println("should not have inferrred value for var " + v);
                    return false;
                } else if (b != null && c == null) {
                    out.println("should have inferred value for var " + v);
                    return false;
                } else if (b != c) {
                    out.println("inferred wrong value for var " + v);
                    return false;
                }
            }
        }

        return true;
    }

    boolean test_infer_fixed() {
        out.println("\ntesting inference");
        return test_infer(harder);
    }

    boolean test_infer_extra() {
        out.println("\ntesting inference (extra)");
        return test_infer(extra);
    }

    void run() {
        if (test_forward_easy() && test_forward_random() &&
            test_solve_fixed() && test_solve_random() &&
            test_infer_fixed() && test_infer_extra())
            out.println("all tests passed");
    }

    public static void main(String[] args) {
        new SolverTest().run();
    }
}
