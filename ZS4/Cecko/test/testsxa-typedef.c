// testing TYPEIDF and typedef
// don't use new types - we don't fill the tables yet

//#include <stdio.h>

enum En {
	ALPHA,
	BETA = 729,
	GAMMA
};

typedef enum En ent;

typedef struct Str* str_ptr;

struct Str {
	const char* key;
	void * next;		// this should be str_ptr, we cannot use it yet
};

int main(int argc, char** argv)
{
	FILE * myout;	// this is a declaration

	int a, b;
	a = 1;
	b = 2;
	a * b;	// this is not a declaration, this is an expression

	return 0;
}

