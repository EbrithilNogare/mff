# Task: Analyze Memory Accesses (with Pin)

## Overview

The goal of this assignment is to provide some opportunity to play with binary program instrumentation.
The tool of choice is [Pin](http://software.intel.com/en-us/articles/pintool).

To familiarize yourself with Pin, take a look at the examples in the `source/tools/SimpleExamples` and `source/tools/ManualExamples` directories.

## Assignment

Write a Pin instrumentation tool that will analyze memory accesses of a program.
The assignment offers three alternatives with different point grades, pick one.

### Basic Level (-2 points)

Intercept all memory accesses of the program and produce a histogram of the values read and written.
For example, if a program writes `0x12345678` into an integer variable, your instrumentation should
count that as writing `0x12` once, `0x34` once, `0x56` once, and `0x78` once.

The output of the tool should be a CSV, JSON or YAML file with the number of reads and writes
performed for each of the 256 possible values. Including a script to produce a histogram
is a plus.

### Standard Level (baseline)

Intercept all memory accesses of the program and identify locations where non aligned accesses happen.
Each location where a non aligned access happens should only be listed once, the locations should be
identified by function name (if available), instruction address in code, and instruction disassembly.

### Advanced Level (+2 points)

Intercept all memory accesses of the program and the invocations of the `mmap` (and `mremap` and `munmap`)
function that allocate anonymous memory, and, at the end of the program, print:

- the total amount of memory that was allocated
- the amount of memory that was allocated but never accessed (neither read nor written)
- the amount of memory that was written but never read
- the amount of memory that was read before being written
- the amount of memory that was not read after last write

(Feel free to come up with other similar statistics as you see fit.)

## Hints

Multiple examples shipped with Pin intercept memory accesses, looking for `INS_IsMemoryRead` and `INS_IsMemoryWrite` might help.

Your tool does not have to support programs with multiple threads, but thread safety is a plus.

Remember that your tool implementation uses a runtime library provided by Pin,
rather than the standard runtime library, and must limit interaction with
the system to what this library provides.

## What to Submit

A report in `SOLUTION.md` showing the output of your tool and explaining the basic design of your implementation.

The implementation of your instrumentation, together with a build file. Ideally,
use the makefile scripts from the Pin distribution examples, so that setting
`PIN_ROOT` to the Pin distribution and doing `make` works.
