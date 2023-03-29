#!/bin/sh

run_echo() {
    echo "[run.sh]" "$@" >&2
    "$@"
}
RUN=run_echo

run_bench() {
    $RUN java \
        -cp .:ubench-agent.jar -agentpath:./libubench-agent.so \
        Measure "$@"
}

run_pair() {
    echo "benchmark,name,type,index,events,instructions"
    run_bench "$1" "$1" "work" "$2"
    run_bench "EmptyWorkload" "$1" "empty" "$2"
}

set -o errexit

(
    run_pair "LoadInstructions" "PAPI_LD_INS"
) > out.csv
