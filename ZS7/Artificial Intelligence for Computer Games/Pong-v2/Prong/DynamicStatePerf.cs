using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Prong
{
    class DynamicStatePerf
    {
        private DynamicState state;
        
        public DynamicStatePerf()
        {
            state = new DynamicState();
        }

        public void Run()
        {
            state.Clone();
        }

        public void Perf(long iterations)
        {
            TimeIt.ExecuteAndReport("DynamicState.Clone", this.Run, iterations);
        }

        public void Perf(double maxTimeSecs)
        {
            TimeIt.ExecuteAndReport("DynamicState.Clone", this.Run, maxTimeSecs);
        }

        public static void RunPerf()
        {
            DynamicStatePerf statePerf = new DynamicStatePerf();
            statePerf.Perf(1000000);
            statePerf.Perf(0.05);
        }
    }
}
