// array

int main(int argc, char** argv)
{
	int arr[42];

	int i, j;
	for(i=0;i<42;++i)
		arr[i] = i;

	int darr[4][2];

	for(i=0;i<4;++i)
		for(j=0;j<2;++j)
		darr[i][j] = i+j;

	return 0;
}
