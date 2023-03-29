public class LoadInstructions extends Workload {
    public static volatile int VAR1 = 42;
    public static volatile int VAR2 = 1;
    public static volatile int VAR3 = 2;
    public static volatile int VAR4 = 3;
    public static volatile int VAR5 = 4;
    public static volatile int VAR6 = 5;
    
    @Override
    public int execute () {
        return (VAR1 + VAR2 + VAR3 + VAR4 + VAR5 + VAR6);
    }
}
