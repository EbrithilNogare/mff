# Task: Prove False Sharing

## Overview

False sharing refers to situations where multiple threads access distinct
variables located close to each other in memory - so close that
the cache hardware is forced to handle the variables together.

False sharing can significantly impact performance when
the cache coherency protocols ship exclusively owned
cache lines between cores in response to what should
otherwise be independent thread execution.

## Assignment

The goal of the assignment is to prove, through the use of hardware performance event counters,
that false sharing actually takes place. Modify the false sharing example by adding
the required calls to the PAPI library, and choose appropriate events, or
use the `perf` tool (but then you will not enjoy coding with PAPI :-).

The example is run as follows:

```shell
# Run with distance 0 on CPU 0 and 1 to observe the effects of true sharing.
./false-sharing 0 1
# Run with distance 1 on CPU 0 and 1 to observe the effects of false sharing.
./false-sharing 1 1
# Run with distance 64 on CPU 0 and 1 to observe execution without sharing.
./false-sharing 64 1
# It is possible to specify a CPU list to use.
./false-sharing 1 1 2 3
```

## Miscellanea

If you decide to use PAPI for the task, please note that special handling is needed to use PAPI with multiple threads.
See `man PAPI_set_cmp_granularity` for setting default counting granularity
and `man PAPI_set_opt` for setting inheritance at event set level.

It also helps to think about how many events related to false sharing you should see,
and possibly run your measurements multiple times to get some idea of what
counter value differences are due to workload and what is just noise.

## What to Submit

The updated false sharing example with a log of standard output demonstrating your results.
A summary `SOLUTION` document describing your platform and explaining how your results prove false sharing.
Your explanation must be technically detailed and include comments relating the actual counter values to the executed loop counts.
