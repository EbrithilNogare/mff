#!/bin/sh

run_echo() {
    echo "[clean.sh]" "$@" >&2
    "$@"
}
RUN=run_echo

$RUN rm -f *.class

if [ "$1" = "-f" ]; then
    $RUN rm -rf java-ubench-agent
    $RUN rm -f libubench-agent.so ubench-agent.jar
fi

