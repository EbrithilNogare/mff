// pointer operations

int main(int argc, char** argv)
{
	int a, b;

	a = 3;
	b = 5;
	{
		char g;
		const char* pa, * pb, * pc, * pd, * pe, * ph;

		const char* p;
		p = "ABCDEFGHIJK";

		pa = p + 1;
		pb = p + 7;

		pc = pa + b;
		pd = a + pb;
		pe = pb - a;
		g = pa[a];
		ph = &pb[-b];

		printf("*pc=%c *pd=%c *pe=%c g=%c *ph=%c\n", *pc, *pd, *pe, g, *ph);
	}
	{
		int g;
		int* pa, * pb, * pc, * pd, * pe, * ph;

		int arr[11];
		arr[2] = 222;
		arr[4] = 444;
		arr[6] = 666;
		arr[8] = 888;
		arr[10] = 1010;

		pa = arr + 1;
		pb = arr + 7;

		pc = pa + b;
		pd = a + pb;
		pe = pb - a;
		g = pa[a];
		pb[-b] = 729;

		printf("*pc=%d *pd=%d *pe=%d g=%d arr[2]=%d\n", *pc, *pd, *pe, g, arr[2]);
	}
	return 0;
}
