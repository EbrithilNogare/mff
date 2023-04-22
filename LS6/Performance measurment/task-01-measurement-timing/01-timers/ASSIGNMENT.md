# Task: How Much Can We Trust Our Timers ?


## Overview

It is useful to know how much we can trust the timing functions we use in experiments.
The goal of this assignment is to determine the actual precision of some timers
available in your favorite programming languages or environments.

As shown during lectures, collect multiple back-to-back time measurements,
plot the collected measurements and analyze the results.


## Details

Pick two different timers in two reasonably different programming languages or environments
(so not the same code compiled using C and C++ or the same script run using `sh` and `bash` :-).
Be creative, try to find other timers than those shown during lectures
(for example, how about the coarse timer variants in `clock_gettime`,
the `Date` class in JavaScript in various browsers, and so on ?).

Experimentally (or otherwise) determine:
- the resolution of the timers (related to units returned)
- the granularity of the timers (related to minimum increment)
- the overhead of the timer access (related to minimum delta)
- the stability of the timer (both during and after warm up)
- the overflow period of the timer
- anything else you find interesting

Run the actual measurement enough times in a loop to make sure
you avoid distorting your results by the warm up artifacts
of your environment. Things you might encounter:
- thermal throttling changing the speed of your computer during measurement
- power management changing the speed of your computer during measurement
- runtime environment compiling your code during measurement
- ...


## What to Submit

The sources of your implementation, with a script for building the executables where applicable.

The data file with the measurement results you have collected.

A summary `SOLUTION` notebook describing your platform, plotting and giving the results per the list above.
