# Measure Method Duration without Nested Calls (with DiSL)

## PC spec:
- PC in LAB: u-pl4
- OS: Linux
- CPU: Intel(R) Core(TM) i7-6700 CPU @ 3.40GHz
- java version: 11.0.18_p10

## How it works
I am inserting methods around interesting and non-interesting function.

these methods starts / end timer and final time is printed out.


### Program output:
```
disl: subroutine took 4000 ms
disl: subroutine took 3000 ms
disl: interesting took 5049 ms
Took 12052 ms [res 5].
The interesting method duration should be around 5 s.
The nested method duration should be around 7 s.
The total duration should be around 12 s.
```
