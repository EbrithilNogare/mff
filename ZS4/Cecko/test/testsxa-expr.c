// basic expression

int fce(int p, int q)
{
	++p;
	--q;
	p++;
	q--;
	int rv;
	rv = p;
	if(p<10 && q>20 || p>=30 && q<=10 || !(p==q))
		rv *= q;
	else
		rv += q;
	return rv;
}

int main(int argc, char** argv)
{
	int a;
	a = 1+2*3-8/sizeof(int);
	int b;
	b = a+argc;
	int r;
	r = fce(a, b);
	
	int *p;
	p = &r;

	*p = fce(a+b, b-a);

	return 0;
}
