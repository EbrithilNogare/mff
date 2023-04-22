# Task: Become Benchmark Designer

Your task is to consider several aspects of benchmark design, outlined below,
then pick one and implement a corresponding benchmark component in
the [Renaissance](https://renaissance.dev) benchmark suite.

Among the aspects to consider:

## Benchmark Workload Choice

The choice of workloads included in a benchmark suite depends on the purpose of the suite.
Typical constraints include:

- Size. The workload requirements and execution time should fit the suite.
- Realism. The workload should reasonably approximate a realistic application.
- Simplicity. The workload should be reasonably easy to run and integrate in the suite.
- Uniqueness. The workload should not be overly redundant with other workloads in the suite.
- Portability. The workload dependencies and portability should match that of the suite.
- Stability. The workload should be reasonably stable, or even deterministic.

If you are interested in this aspect of the benchmark design,
try your hand at designing a new workload module for Renaissance.
See the sources of the existing workloads in [the `benchmarks` directory](https://github.com/renaissance-benchmarks/renaissance/tree/master/benchmarks).

It is quite common to use components of existing open source applications as workloads.
Should you do that, please make sure to look at the relevant licensing conditions,
and disclose such use !

## Benchmark Workload Scaling

Given the commonly diverse range of platform parameters, a benchmark suite may need to scale
to run reasonably well on both small and large machines. Typical scaling axes include:

- Parallelism. When the available degree of parallelism changes,
  should the workload scale to perform the same computation
  more quickly, or should it scale to perform more work ?

- Memory usage. When significant memory consumption is desirable,
  how should the workload scale to consume more memory ? Should
  it work with larger data, or should it work with the same
  data in more copies ?

If you are interested in this aspect of the benchmark design,
try implementing scaling options for some of the workload modules in Renaissance.
See the configurable parameters of the existing workloads, implemented with the `@Parameter` annotation,
then pick a workload and provide more scaling options to it through these or additional configurable parameters.

## Benchmark Workload Validation

The typical purpose of a benchmark is to assess performance of a system on a particular task.
Among issues that can thwart this purpose, portability problems or unexpected optimizations
may lead to part of the workload being skipped with performance still reported.
To prevent such silent distortion of benchmark results,
a benchmark workload should perform result validation,
as a proof that the intended task was indeed
carried out.

If you are interested in this aspect of the benchmark design,
pick an existing workload in Renaissance that does not yet have validation and implement one.

## Benchmark Metric Collection

Sometimes, other metrics than just time need to be collected.
Examples may include hardware performance counter, garbage
collection activity, machine temperature, and so on.

If you are interested in adding more metrics to the benchmark harness, write a benchmark harness plugin that collects them.
See the sources of the existing plugins in [the `plugins` directory](https://github.com/renaissance-benchmarks/renaissance/tree/master/plugins).


## Alternatives

Your solution can pick a different suite or a different benchmark design aspect,
but you have to justify your choices. If not sure, consult prior to starting work.


## What to Submit

On the code side, either a patch to the current Renaissance sources,
or a link to a public repository with your modifications.
A report in `SOLUTION.md` that provides your reasoning,
and describes how to use your code.
