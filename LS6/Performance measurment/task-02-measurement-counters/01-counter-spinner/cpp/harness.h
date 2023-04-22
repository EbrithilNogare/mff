#ifndef HARNESS_H_GUARD
#define HARNESS_H_GUARD

class workload {
public:
    virtual int execute () = 0;
    virtual const char *name () = 0;
    virtual void before () {};
    virtual void after () {};
    virtual ~workload () {};
};

void harness_init ();
void harness_run (workload *work, const char *counter);
void harness_run (workload *work, const char *counter, int outer, int inner);
void harness_done ();

#endif
