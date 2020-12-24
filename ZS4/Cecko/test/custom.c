// sizeof

int main(int argc, char** argv)
{
	printf("_Bool:%d char:%d int:%d int*:%d void*:%d int(*)(void):%d\n", 
		sizeof(_Bool), sizeof(char), sizeof(int), sizeof(int*), sizeof(void*), sizeof(int(*)(void)));

	return 0;
}
