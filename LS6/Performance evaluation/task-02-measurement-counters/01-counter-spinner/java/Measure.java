import cz.cuni.mff.d3s.perf.Benchmark;

import java.util.ArrayList;
import java.util.List;

public class Measure {
    private static final int MEASURED_LOOPS = 30000;
    private static final int INNER_LOOPS = 1000;
    private static final int [] STEPS = {
            100, 1000, 10000, MEASURED_LOOPS
    };
    private static final String INSTRUCTION_COUNTER = "PAPI_TOT_INS";

    public static class Empty extends Workload {
        @Override
        public int execute() {
            return (0);
        }
    }

    public static void main(String[] args) {
        if (args.length < 3) {
            die("Wrong invocation (provide name, workload class and counter).");
        }

        String workloadClassName = args [0];
        String benchmarkName = args [1];
        String workloadTypeName = args [2];
        String counter = args [3];

        Class<?> workloadClass = null;
        try {
            workloadClass = Class.forName (workloadClassName);
        } catch (ClassNotFoundException e) {
            die ("Unable to load class %s.", workloadClassName);
        }
        Workload workload = null;
        try {
            workload = (Workload) workloadClass.newInstance ();
        } catch (ReflectiveOperationException e) {
            die ("Unable to instantiate %s (%s).", workloadClass.getName (), e.getMessage ());
        }

        List<List<long []>> results = new ArrayList<> (STEPS.length);

        for (int loops : STEPS) {
            results.add (measure(workload, counter, loops, INNER_LOOPS));
        }

        List<long []> finals = results.get (results.size() - 1);

        int i = 0;
        for (long [] row : finals) {
            System.out.printf ("%s,%s,%s,%d,%d,%d\n", benchmarkName, counter, workloadTypeName, i, row [0], row [1]);
            i ++;
        }
    }

    public static volatile int BLACKHOLE = 0;

    private static List<long []> measure (Workload work, String counter, int outer, int inner) {
        String [] events = { counter, INSTRUCTION_COUNTER };

        Benchmark.init (outer, events);

        work.before ();

        int accum = 0;

        for (int i = 0 ; i < outer ; i++) {
            Benchmark.start ();
            for (int j = 0 ; j < inner ; j++) {
                accum += work.execute ();
            }
            Benchmark.stop ();
        }

        work.after ();

        BLACKHOLE = accum;

        List<long []> results =  Benchmark.getResults ().getData ();

        if (results.size () != outer) {
            die ("Collected %d samples instead of %d.", results.size (), outer);
        }

        for (long [] row : results) {
            if (row.length != 2) {
                die ("Expected 2 samples, got %d.", row.length);
            }

            if ((row [0] < 0) || (row [1] < 0)) {
                die ("Got negative counter value, probably problem with PAPI.");
            }
        }

        return (results);
    }

    private static void die (String fmt, Object... args) {
        System.err.printf (fmt + " Aborting.\n", args);
        System.exit (1);
    }
}
