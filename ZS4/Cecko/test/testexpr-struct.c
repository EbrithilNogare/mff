// struct

struct mighty_str {
	char c;
	int x,y;
};

struct mighty_str F(void)
{
	struct mighty_str locvar;
	locvar.c = 'C';
	locvar.x = 10;
	locvar.y = 42;
	return locvar;
}

int main(int argc, char** argv)
{
	struct mighty_str ms;

	ms = F();

	printf("c=%c x=%d y=%d\n", ms.c, ms.x, ms.y);

	char c;
	int x, y;
	c = F().c;
	x = F().x;
	y = F().y;

	printf("c=%c x=%d y=%d\n", c, x, y);

	return 0;
}
