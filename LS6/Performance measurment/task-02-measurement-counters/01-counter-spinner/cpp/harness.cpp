#include <stdio.h>
#include <assert.h>
#include <stdlib.h>
#include <papi.h>
#include "harness.h"

#define PAPI_CHECKED_VERBOSE(call, fmt, ...) \
	do { \
		int _papi_rc = call; \
		if (_papi_rc != PAPI_OK) { \
			fprintf (stderr, "Error: %s [" fmt "] returned %d, aborting.\n", #call, __VA_ARGS__, _papi_rc); \
			exit (1); \
		} \
	} while (0)

#define PAPI_CHECKED(call) \
	do { \
		int _papi_rc = call; \
		if (_papi_rc != PAPI_OK) { \
			fprintf (stderr, "Error: %s returned %d, aborting.\n", #call, _papi_rc); \
			exit (1); \
		} \
	} while (0)


class empty_workload : public workload {
public:
    const char *name () override { return "empty"; }
    int execute () override { return 0; }
};


volatile int blackhole = 0;

static void measure (workload *work, int eventset, int outer, int inner, long long *counters) {
	work->before ();

	int accum = 0;

	for (int i = 0 ; i < outer ; i++) {
		PAPI_CHECKED (PAPI_start(eventset));
		for (int j = 0 ; j < inner ; j++) {
			accum += work->execute ();
		}
		PAPI_CHECKED (PAPI_stop(eventset, counters + 2 * i));
	}

	work->after ();

	blackhole = accum;
}

static void dump (const char *benchmark, const char *name, long long *counters, int size, const char *type) {
	for (int i = 0 ; i < size ; i++) {
		long long counter_value = counters [2 * i];
		long long instructions = counters [2 * i + 1];

		printf ("%s,%s,%s,%d,%lld,%lld\n", benchmark, name, type, i, counter_value, instructions);
	}
}

void harness_init () {
	PAPI_library_init (PAPI_VER_CURRENT);
	printf ("benchmark,name,type,index,events,instructions\n");
}

void harness_run (workload *work, const char *counter_name) {
	harness_run (work, counter_name, 10000, 1000);
}

void harness_run (workload *work, const char *counter_name, int outer, int inner) {
	int monitored_events = PAPI_NULL;

	fprintf (stderr, "Measuring %s %s.\n", work->name (), counter_name);

	PAPI_CHECKED (PAPI_create_eventset(&monitored_events));
	PAPI_CHECKED (PAPI_assign_eventset_component(monitored_events, 0));

	int counter_code;
	PAPI_CHECKED_VERBOSE (PAPI_event_name_to_code((char *) counter_name, &counter_code), "event=%s", counter_name);
	PAPI_CHECKED (PAPI_add_event(monitored_events, counter_code));
	PAPI_CHECKED (PAPI_add_event(monitored_events, PAPI_TOT_INS));

	long long *counters_work = (long long *) malloc (sizeof (long long) * outer * 2);
	long long *counters_empty = (long long *) malloc (sizeof (long long) * outer * 2);

	workload *empty = new empty_workload ();

	// Run twice interleaved and discard first repetition results.
	measure (work, monitored_events, outer / 2, inner, counters_work);
	measure (empty, monitored_events, outer / 2, inner, counters_empty);

	measure (work, monitored_events, outer, inner, counters_work);
	measure (empty, monitored_events, outer, inner, counters_empty);

	dump (work->name (), counter_name, counters_work, outer, "work");
	dump (work->name (), counter_name, counters_empty, outer, "empty");

	delete empty;
	free (counters_work);
	free (counters_empty);

	PAPI_CHECKED (PAPI_cleanup_eventset(monitored_events));
	PAPI_CHECKED (PAPI_destroy_eventset(&monitored_events));
}


void harness_done () {
	PAPI_shutdown ();
}
