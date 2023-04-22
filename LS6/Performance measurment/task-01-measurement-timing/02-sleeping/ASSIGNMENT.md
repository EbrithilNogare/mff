# Task: How Accurate Is Sleeping ?


## Overview

Many measurement experiments involve subjecting the system under test (SUT) to a well defined workload.
Such workload may include delays (for example to simulate user think time), possibly implemented using
one of the thread sleeping functions provided by the operating system.

Your task is to evaluate how accurate are the delays implemented using these functions.


## Details

Pick a programming language or environment and implement a program that measures the actual duration
of a delay implemented using one of the available thread sleep functions (such as `Thread.sleep` in
Java or `nanosleep` in POSIX). Be creative, try to find an unusual sleep function, but focus only
on functions with sub second sleep ability.

Experimentally (or otherwise) determine:
- the overhead of the sleep (related to minimum duration)
- the granularity of the sleep (related to minimum increment)
- anything else you find interesting


## What to Submit

The sources of your implementation, with a script for building the executables where applicable.

The data file with the measurement results you have collected.

A summary `SOLUTION` notebook describing your platform, plotting and giving the results per the list above.
