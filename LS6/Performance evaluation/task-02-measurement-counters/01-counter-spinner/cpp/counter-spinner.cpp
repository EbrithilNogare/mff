#include <stdlib.h>
#include <iostream>
#include <cstdlib>
#include <ctime>

#include "harness.h"

class load_instructions : public workload
{
private:
    int sum = 0;
    int iterator = 0;
    volatile double arrayA [32] = {.1,.1,.1,.1,.1,.1,.1,.1,.1,.1,.1,.1,.1,.1,.1,.1,.1,.1,.1,.1,.1,.1,.1,.1,.1,.1,.1,.1,.1,.1,.1,.1};
    volatile double arrayB [32] = {.2,.2,.2,.2,.2,.2,.2,.2,.2,.2,.2,.2,.2,.2,.2,.2,.2,.2,.2,.2,.2,.2,.2,.2,.2,.2,.2,.2,.2,.2,.2,.2};
    volatile double arrayC [32] = {.3,.3,.3,.3,.3,.3,.3,.3,.3,.3,.3,.3,.3,.3,.3,.3,.3,.3,.3,.3,.3,.3,.3,.3,.3,.3,.3,.3,.3,.3,.3,.3};
    int megaArray[3*8*1024*1024]; // for machine with 8MBi of L3 and loading 2 pages at a time

public:
    const char *name() override { return "load_instructions"; }
    void before() override
    {
        std::srand(std::time(nullptr));
        
        const int pageSize = 1024;
        int length = std::end(megaArray) - std::begin(megaArray);
        for(int i = 0; i<length;i++)
            megaArray[i] = (i + pageSize) % length;
    }
    int execute()
    {
        return 0;
    }
    int execute_PAPI_L3_TCM()
    {
        iterator = megaArray[iterator];
        iterator = megaArray[iterator];
        iterator = megaArray[iterator];
        iterator = megaArray[iterator];
        iterator = megaArray[iterator];
        iterator = megaArray[iterator];
        iterator = megaArray[iterator];
        iterator = megaArray[iterator];
        iterator = megaArray[iterator];
        iterator = megaArray[iterator];
        iterator = megaArray[iterator];
        iterator = megaArray[iterator];
        iterator = megaArray[iterator];
        iterator = megaArray[iterator];
        iterator = megaArray[iterator];
        iterator = megaArray[iterator];
        return iterator;
    }
    int execute_FP_ARITH_SCALAR_DOUBLE()
    {
        for (size_t i = 0; i < 32; i++)
        {
            arrayA[i] = arrayB[i] * arrayB[i];
        }
        
        return arrayA[0];
    }
    int execute_PAPI_BR_MSP()
    {
        if (std::rand() % 2 == 0) {
            sum += iterator;
        } else {
            sum -= iterator;
        }
        iterator++;
        return sum;
    }
};

int main(int argc, char *argv[])
{
    harness_init();
    harness_run(new load_instructions(), "PAPI_L3_TCM");
    harness_done();

    return (0);
}
