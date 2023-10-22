using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Prong
{
    delegate void Run();

    class TimeIt
    {
        private static long nanoTime()
        {
            long nano = 10000L * Stopwatch.GetTimestamp();
            nano /= TimeSpan.TicksPerMillisecond;
            nano *= 100L;
            return nano;
        }

        public static long ExecuteIterations(Run run, long iterations)
        {
            long nanos = 0;

            for (long i = 0; i < iterations; ++i)
            {
                long start = nanoTime();
                run();
                nanos += nanoTime() - start;
            }

            return nanos;
        }

        public static long ExecuteTime(Run run, double timeSecs)
        {
            long iterations = 0;
            long timeNanos = (long)(timeSecs * 1000000000L);

            while (timeNanos > 0)
            {
                long start = nanoTime();
                run();
                timeNanos -= nanoTime() - start;
                if (timeNanos > 0)
                {
                    iterations += 1;
                }
            }
            return iterations;
        }

        public static long ExecuteAndReport(string name, Run run, long iterations)
        {
            long nanos = ExecuteIterations(run, iterations);
            double average = nanos / (double)iterations;
            System.Console.WriteLine($"TimeIt[{name}] {iterations} iterations in => total: {nanos / 1000000000.0}s, avg: {average / 1000000000.0}s");
            return nanos;
        }

        public static long ExecuteAndReport(string name, Run run, double maxTimeSecs)
        {
            long iterations = ExecuteTime(run, maxTimeSecs);          
            System.Console.WriteLine($"TimeIt[{name}] in {maxTimeSecs}s we managed to run {iterations} iterations");
            return iterations;
        }
    }
}
