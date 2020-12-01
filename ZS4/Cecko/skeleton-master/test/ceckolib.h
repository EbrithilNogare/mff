#ifndef ceckolib_h_
#define ceckolib_h_

//#include <stdio.h>
//#include <process.h>
//#include <string.h>

typedef unsigned __int64 size_t;
int atexit(void(*)(void));
void exit(int);
void abort(void);
int printf(const char*, ...);
int sprintf(char*, const char*, ...);
int sscanf(const char*, const char*, ...);
int scanf(const char*, ...);
//int fprintf(FILE*, const char*, ...);
void* memset(void*, int, size_t);
void* memcpy(void*, const void*, size_t);

#endif
