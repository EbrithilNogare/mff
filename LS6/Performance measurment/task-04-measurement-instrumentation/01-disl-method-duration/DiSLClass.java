import ch.usi.dag.disl.annotation.After;
import ch.usi.dag.disl.annotation.AfterReturning;
import ch.usi.dag.disl.annotation.Before;
import ch.usi.dag.disl.marker.BodyMarker;
import ch.usi.dag.disl.staticcontext.MethodStaticContext;
import ch.usi.dag.disl.annotation.SyntheticLocal;


public class DiSLClass {

	public static long timeBeforeInteresting;
	public static long timeSumOther;
	public static long timeBeforeOther;

	@Before(marker = BodyMarker.class, scope = "Main.interesting")
	public static void before(MethodStaticContext msc) {
		timeBeforeInteresting = System.nanoTime();
	}

	@After(marker = BodyMarker.class, scope = "Main.interesting")
	public static void after(DumbLoopContext dlc, MethodStaticContext msc) {
		long timeAfter = System.nanoTime();
		System.out.println("disl: " + msc.thisMethodName() + " took " + ((timeAfter - timeBeforeInteresting - timeSumOther) / 1000000) + " ms");
	}
	
	@Before(marker = BodyMarker.class, scope = "Main.subroutine")
	public static void beforeOther(MethodStaticContext msc) {
		timeBeforeOther = System.nanoTime();
	}


	@After(marker = BodyMarker.class, scope = "Main.subroutine")
	public static void afterOther(DumbLoopContext dlc, MethodStaticContext msc) {
		long timeAfter = System.nanoTime();
		timeSumOther += timeAfter - timeBeforeOther;
		System.out.println("disl: " + msc.thisMethodName() + " took " + ((timeAfter - timeBeforeOther) / 1000000) + " ms");
	}
}
