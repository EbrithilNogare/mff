#include <iostream>
#include <fstream>
#include <map>
#include "pin.H"
using std::cerr;
using std::endl;
using std::ios;
using std::ofstream;
using std::string;


struct MemPoint {
    ADDRINT size;
    bool read = false;
    bool written = false;
    bool readAfterLastWrite = false;
};


ofstream OutFile;
std::map<ADDRINT, MemPoint> memoryPoints;
int readBeforeWritten = 0;


VOID MemoryRead(ADDRINT addr, size_t size)
{
    if (memoryPoints.find(addr) == memoryPoints.end()){
        memoryPoints.insert(std::pair<ADDRINT, MemPoint>(addr, {size, false, false, false}));
    }

    memoryPoints[addr].read = true;
    memoryPoints[addr].readAfterLastWrite = true;
}

VOID MemoryWritten(ADDRINT addr, size_t size)
{
    if (memoryPoints.find(addr) == memoryPoints.end()){
        memoryPoints.insert(std::pair<ADDRINT, MemPoint>(addr, {size, false, false, false}));
    }

    memoryPoints[addr].written = true;
    memoryPoints[addr].readAfterLastWrite = false;
}

VOID BeforeMalloc(ADDRINT ret, ADDRINT start, ADDRINT end, size_t size)
{
    for (ADDRINT i = start; i <= end; i += size) {
        if (memoryPoints.find(i) == memoryPoints.end()){
            memoryPoints.insert({i, {size, false, false, false}});
        } else {
            std::cout << "!!! Allocating already alocated memory" << endl;
        }
    }
}

VOID MemMap(ADDRINT ret, ADDRINT start, ADDRINT end, size_t size)
{
    for (ADDRINT i = start; i <= end; i += size) {
        if (memoryPoints.find(i) == memoryPoints.end()){
            memoryPoints.insert({i, {size, false, false, false}});
        } else {
            std::cout << "!!! Allocating already alocated memory" << endl;
        }
    }
}

VOID Instruction(INS ins, VOID *v)
{
    if (INS_IsCall(ins) && RTN_Valid(INS_Rtn(ins)) && (RTN_Name(INS_Rtn(ins)) == "malloc" || RTN_Name(INS_Rtn(ins)) == "calloc" ||RTN_Name(INS_Rtn(ins)) == "realloc")){
        INS_InsertCall(ins, IPOINT_AFTER, (AFUNPTR)MemMap, IARG_REG_VALUE, REG_RAX, IARG_FUNCARG_ENTRYPOINT_VALUE, 0, IARG_FUNCARG_ENTRYPOINT_VALUE, 1, IARG_END);
    }
    if (INS_IsMemoryRead(ins)) {
        INS_InsertCall(ins, IPOINT_BEFORE, (AFUNPTR)MemoryRead, IARG_MEMORYREAD_EA, IARG_MEMORYREAD_SIZE, IARG_END);
    }
    if(INS_IsMemoryWrite(ins)){
        INS_InsertCall(ins, IPOINT_BEFORE, (AFUNPTR)MemoryWritten, IARG_MEMORYWRITE_EA, IARG_MEMORYWRITE_SIZE, IARG_END);
    }
}

VOID Routine(RTN rtn, VOID *v)
{
    // instrument memory mapping functions
    if (RTN_Name(rtn) == "mmap" || RTN_Name(rtn) == "mremap" || RTN_Name(rtn) == "munmap" || RTN_Name(rtn) == "calloc" || RTN_Name(rtn) == "realloc") {
        RTN_Open(rtn);
        RTN_InsertCall(rtn, IPOINT_AFTER, (AFUNPTR)MemMap, IARG_REG_VALUE, REG_RAX, IARG_FUNCARG_ENTRYPOINT_VALUE, 0, IARG_FUNCARG_ENTRYPOINT_VALUE, 1, IARG_END);
        RTN_Close(rtn);
    }
}


VOID Fini(INT32 code, VOID* v)
{
    size_t memoryAlocated = 0;
    size_t alocatedButNeverAccessed = 0;
    size_t writedOnly = 0;
    size_t notReadAfterLastWrite = 0;
    for (auto const& [key, value] : memoryPoints) {
        memoryAlocated += value.size;
        if(!value.read && !value.written)
            alocatedButNeverAccessed += value.size;
        if(!value.read && value.written)
            writedOnly += value.size;
        if(value.readAfterLastWrite)
            notReadAfterLastWrite += value.size;
    }

    OutFile.setf(ios::showbase);
    OutFile << "the total amount of memory that was allocated: " << memoryAlocated << "b" << endl;
    OutFile << "the amount of memory that was allocated but never accessed (neither read nor written): " << alocatedButNeverAccessed << "b" << endl;
    OutFile << "the amount of memory that was written but never read: " << writedOnly << "b" << endl;
    OutFile << "the amount of memory that was read before being written: " << readBeforeWritten << "b" << endl;
    OutFile << "the amount of memory that was not read after last write: " << notReadAfterLastWrite << "b" << endl;
    OutFile.close();
}

KNOB< string > KnobOutputFile(KNOB_MODE_WRITEONCE, "pintool", "o", "inscount.out", "specify output file name");

INT32 Usage()
{
    cerr << "This tool counts the number of dynamic instructions executed" << endl;
    cerr << endl << KNOB_BASE::StringKnobSummary() << endl;
    return -1;
}

int main(int argc, char* argv[])
{
    // Initialize pin
    if (PIN_Init(argc, argv)) return Usage();

    OutFile.open(KnobOutputFile.Value().c_str());

    // Register Instruction to be called to instrument instructions
    INS_AddInstrumentFunction(Instruction, 0);
    RTN_AddInstrumentFunction(Routine, 0);

    // Register Fini to be called when the application exits
    PIN_AddFiniFunction(Fini, 0);

    // Start the program, never returns
    PIN_StartProgram();

    return 0;
}