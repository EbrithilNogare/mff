# Task: Measure Method Duration without Nested Calls (with DiSL)

## Overview

The goal of this assignment is to provide some opportunity to play with bytecode instrumentation.
The tool of choice is [DiSL](https://gitlab.ow2.org/disl/disl). Please note that DiSL currently
supports Java versions 8 to 15 only.

To familiarize yourself with DiSL, take a look at the examples in the `examples` directory.
All examples can be run with `ant run` and the features demonstrated in the examples
should be enough to solve the assignment.

## Assignment

Use DiSL to write an instrumentation that will measure the duration of a method call,
minus the duration of any nested method calls encountered. For example:

```
void inner () {
    // Something that takes 5s
}

void measured_method () {
    // Something that takes 3s
    inner ();
    // Something that takes 4s
}
```

Your instrumentation should report the method duration of 7 s, not 12 s.

The `Main.java` file contains an example application that
executes nested method calls and reports the duration.
Your solution can use this application for testing,
but it should work with any application and any
method (the measured method should be selected
with a compile time constant).

In the example, ignore the call to `System.currentTimeMillis`.

## What to Submit

A report in `SOLUTION.md` showing the output of your tool on the `Main.java` example,
together with the `build.xml` file and `src-app` and `src-inst` directories that follow
the structure of DiSL distribution examples. Ideally, copying your solution to the DiSL
distribution examples directory and doing `ant run` should just work.
