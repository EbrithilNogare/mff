#define _GNU_SOURCE

#include <stdio.h>
#include <stdlib.h>
#include <malloc.h>
#include <pthread.h>
#include <inttypes.h>
#include <papi.h>

#define LOOPS 1000000


typedef struct {
    int index;
    int cpu;
    uint64_t timestamp;
    int misses;
} run_info_t;

static void run_info_print(run_info_t *info) {
    printf("CPU%2d L1 misses: [ %7llu ]", info->cpu, (unsigned long long) info->misses);
}


static volatile int *FALSE_SHARING_VARIABLE;

static pthread_barrier_t barrier;


static inline uint64_t rdtsc() {
    uint32_t lo, hi;
    __asm__ __volatile__ ("rdtsc" : "=a" (lo), "=d" (hi) : : );
    return ((uint64_t) hi << 32 | (uint64_t) lo);
}

static void *worker(void *arg) {
    run_info_t *info = arg;

    int retval, eventset = PAPI_NULL;
    long long values[1];
    int events[1] = {PAPI_L1_DCM};

    if ((retval = PAPI_create_eventset(&eventset)) != PAPI_OK) {
        printf("PAPI_create_eventset failed: %s\n", PAPI_strerror(retval));
        return NULL;
    }

    if ((retval = PAPI_add_events(eventset, events, 1)) != PAPI_OK) {
        printf("PAPI_add_events failed: %s\n", PAPI_strerror(retval));
        return NULL;
    }

    pthread_barrier_wait(&barrier);

    if ((retval = PAPI_start(eventset)) != PAPI_OK) {
        printf("PAPI_start failed: %s\n", PAPI_strerror(retval));
        return NULL;
    }

    uint64_t begin = rdtsc();
    for (int i = 0; i < LOOPS; i++) {
        FALSE_SHARING_VARIABLE[info->index] += i;
    }
    uint64_t end = rdtsc();

    if ((retval = PAPI_stop(eventset, values)) != PAPI_OK) {
        printf("PAPI_stop failed: %s\n", PAPI_strerror(retval));
        return NULL;
    }

    PAPI_cleanup_eventset(eventset);

    info->timestamp = end - begin;
    info->misses = values[0];

    return NULL;
}


static void pthread_create_with_affinity(pthread_t *thread,
        void *(*worker) (void *),
        void *arg,
        int affinity) {

    pthread_attr_t attr;
    pthread_attr_init(&attr);

    cpu_set_t cpus;
    CPU_ZERO(&cpus);
    CPU_SET(affinity, &cpus);

    pthread_attr_setaffinity_np(&attr, sizeof(cpu_set_t), &cpus);

    pthread_create(thread, &attr, worker, arg);
}

int main(int argc, char *argv[]) {
    if (argc == 1) {
        fprintf(stderr, "Usage: %s distance CPU [CPU [CPU ... ]]\n", argv[0]);
        return 1;
    }

    int count = argc - 2;

    int distance = atoi(argv[1]);

    int *cpus = malloc(sizeof(int) * count);
    for (int i = 0; i < count; i++) {
        cpus[i] = atoi(argv[i + 2]);
    }

    run_info_t *info_one = malloc(sizeof(run_info_t) * count);
    run_info_t *info_two = malloc(sizeof(run_info_t) * count);

    FALSE_SHARING_VARIABLE = memalign(64, sizeof(int) * 256);

    int retval;

    // Initialize PAPI library
    if ((retval = PAPI_library_init(PAPI_VER_CURRENT)) != PAPI_VER_CURRENT) {
        printf("PAPI_library_init failed: %s\n", PAPI_strerror(retval));
        return 1;
    }

    // Initialize thread support for PAPI
    if ((retval = PAPI_thread_init(pthread_self)) != PAPI_OK) {
        printf("PAPI_thread_init failed: %s\n", PAPI_strerror(retval));
        return 1;
    }

    for (int i = 0; i < count; i++) {
        pthread_t thread_one, thread_two;

        info_one[i].index = 0;
        info_one[i].cpu = 0;

        info_two[i].index = distance;
        info_two[i].cpu = cpus[i];


        pthread_barrier_init(&barrier, NULL, 2);

        pthread_create_with_affinity(&thread_one, worker, &info_one[i], 0);
        pthread_create_with_affinity(&thread_two, worker, &info_two[i], cpus[i]);

        pthread_join(thread_one, NULL);
        pthread_join(thread_two, NULL);

        pthread_barrier_destroy(&barrier);
    }

    for (int i = 0; i < count; i++) {
        run_info_print(&info_one[i]);
        printf("  :  ");
        run_info_print(&info_two[i]);
        puts("");
    }

    return 0;
}
