import problems.*;
import search.*;

class UcsTest {
  static <S, A> void runTest(Problem<S, A> prob) {
		Solution<S, A> solution = Ucs.search(prob);
		Solution.report(solution, prob);
  }

  public static void main(String[] args) {
    System.out.println("== Empty ==");
    runTest(new Empty());

    System.out.println("== Graph ==");
    runTest(new Graph());
    
    System.out.println("== Line ==");
    runTest(new Line());
    
    System.out.println("== Grid ==");
    runTest(new Grid());
    
  }
}
