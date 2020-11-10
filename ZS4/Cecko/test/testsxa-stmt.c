// statements

int main(int argc, char** argv)
{

	if(argc<2)
	{
		printf("No args");
		return 4;
	}

	int mx;
	sscanf(argv[1], "%d", &mx);

	int i;
	for(i=0;i<mx;++i)
	{
		int q;
		q = i;
		while(q--)
			if(i>q)
				do {
					// nothing to do
				} while(q++!=i);
			else if(i<q)
				do {
					// nothing to do
				} while(q--!=i);			
		  else
				;	// this is empty expr
		
	}

	return 0;
}
