// int operations

_Bool b;	// false

int to_int(int a)
{
	return a;
}

char to_char(char a)
{
	return a;
}

const char* to_pointer(const char * a)
{
	return a;
}

int char_to_int(char a)
{
	return a;
}

char int_to_char(int a)
{
	return a;
}

int main(int argc, char** argv)
{
	{
		char x, y;
		int i, j, k;
		const char* p;

		x = 'X';
		k = 0x3F41;
		p = "Test";	// array-to-pointer
		i = b; // _Bool-to-int
		j = x; // char-to-int
		y = k; // int-to-char

		printf("to_int(x)=0x%x k=0x%x i=%d j=0x%x to_int(y)=0x%x\n", to_int(x), k, i, j, to_int(y));
		printf("x='%c' k=%d p=\"%s\" i=%d j=%d y='%c'\n", x, k, p, i, j, y);

	}
	{
		char x, y;
		int i, j, k;
		const char* p;

		x = to_char('X');
		k = to_int(0x3F41);
		p = to_pointer("Test");	// array-to-pointer
		i = to_int(b); // _Bool-to-int
		j = char_to_int(x); // char-to-int
		y = int_to_char(k); // int-to-char

		printf("to_int(x)=0x%x k=0x%x i=%d j=0x%x to_int(y)=0x%x\n", to_int(x), k, i, j, to_int(y));
		printf("x='%c' k=%d p=\"%s\" i=%d j=%d y='%c'\n", x, k, p, i, j, y);
	}

	return 0;
}
