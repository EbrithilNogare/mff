#!/bin/sh

run_echo() {
    echo "[make.sh]" "$@" >&2
    "$@"
}
RUN=run_echo

set -o errexit

if ! [ -e "ubench-agent.jar" ]; then
    $RUN rm -rf "java-ubench-agent"
    $RUN git clone https://github.com/d-iii-s/java-ubench-agent
    (
        set -o errexit
        $RUN cd java-ubench-agent
        $RUN ant lib
    )
    $RUN cp java-ubench-agent/out/lib/libubench-agent.so .
    $RUN cp java-ubench-agent/out/lib/ubench-agent.jar .
    $RUN chmod +x libubench-agent.so
fi

$RUN javac -cp ubench-agent.jar Measure.java Workload.java EmptyWorkload.java LoadInstructions.java
