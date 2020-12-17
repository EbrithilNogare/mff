// int operations

_Bool gb;	// false
char gx;
int gi;
int* gpi;

int main(int argc, char** argv)
{
	char lx;
	int li;

	_Bool* pgb;
	char* pgx, * plx;
	int* pgi, * pli;
	int** pgpi;

	pgb = &gb;
	pgx = &gx;
	pgi = &gi;
	plx = &lx;
	pli = &li;

	{
		int j, k, l, m, n;

		j = *pgb;
		k = *pgx;
		l = *pgi;

		*pgx = 'L';
		*pgi = 123;

		m = gx;
		n = gi;

		printf("j=%d k=%d l=%d m='%c' n=%d\n", j, k, l, m, n);
	}

	{
		int k, l, m, n;

		*plx = 'Z';
		*pli = 729;

		k = *plx;
		l = *pli;

		m = lx;
		n = li;

		printf("k='%c' l=%d m='%c' n=%d\n", k, l, m, n);
	}

	gpi = pgi;
	pgpi = &gpi;

	{
		int j, k, l, m;

		j = **pgpi;
		**pgpi = 333;
		k = gi;
		*pgpi = pli;
		l = **pgpi;
		**pgpi = 444;
		m = li;

		printf("j=%d k=%d l=%d m=%d\n", j, k, l, m);
	}

	return 0;
}
