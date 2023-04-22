# Task: Spin Hardware Counters

## Overview

Write code that generates specific counter events as frequently as possible.

We have prepared a harness for your code that collects the counter values and computes a score.
The score is the number of instructions executed to fire one event, lower is better.
The best score in each event category will receive a grading bonus :-)

Since counter availability is platform dependent, you need to tailor the counter choice to your platform.
The following lists apply to various processor platforms in the student lab, pick at least 3 counters,
each from a different row in the same list, or their reasonable equivalents
if using an entirely different platform:

- Intel Skylake Processors (such as those in Rotunda Lab)
    - `PAPI_SP_OPS` or `PAPI_DP_OPS` or `PAPI_VEC_SP` or `PAPI_VEC_DP`
    - `PAPI_BR_PRC` or `PAPI_BR_MSP` or `ix86arch::MISPREDICTED_BRANCH_RETIRED`
    - `PAPI_L3_TCM` or `ix86arch::LLC_MISSES` or `perf::PERF_COUNT_HW_CACHE_MISSES`
    - `PAPI_TLB_DM` or `PAPI_TLB_IM` or `perf::PERF_COUNT_HW_CACHE_DTLB:MISS` or `perf::PERF_COUNT_HW_CACHE_ITLB:MISS`
    - `perf::PERF_COUNT_SW_PAGE_FAULTS` or `perf::PERF_COUNT_SW_CONTEXT_SWITCHES` or `perf::PERF_COUNT_SW_CPU_MIGRATIONS`

- AMD Ryzen 5 Processors (such as those in Linux Lab)
    - `PAPI_FP_INS` or `PAPI_FML_INS` or `PAPI_FDV_INS`
    - `PAPI_BR_MSP` or `RETIRED_NEAR_RETURNS_MISPREDICTED`
    - `perf::PERF_COUNT_HW_CACHE_LL:MISS` or `perf::PERF_COUNT_HW_CACHE_LL:PREFETCH`
    - `PAPI_TLB_DM` or `PAPI_TLB_IM` or `perf::PERF_COUNT_HW_CACHE_DTLB:MISS` or `perf::PERF_COUNT_HW_CACHE_ITLB:MISS`
    - `perf::PERF_COUNT_SW_PAGE_FAULTS` or `perf::PERF_COUNT_SW_CONTEXT_SWITCHES` or `perf::PERF_COUNT_SW_CPU_MIGRATIONS` or `RETIRED_BRANCH_RESYNCS`

## Example

Assume you want to generate the `PAPI_LD_INS` event, which counts the load instructions.
Start by inheriting from the `workload` class and overriding the `execute ()` method.
The new method will obviously need to load data, standard variable accesses should
do the trick.

### C++

```cpp
#include <stdlib.h>
#include "harness.h"

volatile int var1 = 1;
volatile int var2 = 2;
volatile int var3 = 3;
volatile int var4 = 4;
volatile int var5 = 5;
volatile int var6 = 6;

class load_instructions : public workload {
public:
    int execute() {
        return (var1 + var2 + var3 + var4 + var5 + var6);
    }
};

int main () {
    harness_init ();
    harness_run (new load_instructions (), "PAPI_LD_INS");
    harness_done();
    return (0);
}

```

### Java

```java
public class LoadInstructions extends Workload {
    public static volatile int VAR1 = 1;
    public static volatile int VAR2 = 2;
    public static volatile int VAR3 = 3;
    public static volatile int VAR4 = 4;
    public static volatile int VAR5 = 5;
    public static volatile int VAR6 = 6;

    @Override
    public int execute() {
        return (VAR1 + VAR2 + VAR3 + VAR4 + VAR5 + VAR6);
    }
}
```

## Internals

The actual measurement is implemented as follows:

```pseudo
for i in 1..10000:
    start-counters ()
    for j in 1..1000
        workload.execute ()
    done
    stop-and-store-counters ()
done
```

Please make sure your solution completes in reasonable time (at most seconds).

## Plotting

```shell
Rscript counter-spinner-plot.r out.csv out.pdf
```


## What to Submit

Your `workload` classes, with a script for building the executable.

The data file with the measurement results you have collected, and the pdf report.

A summary `SOLUTION` document describing your platform and the rationale behind your workloads.
