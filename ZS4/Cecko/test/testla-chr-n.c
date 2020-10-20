
int main(int argc, char** argv)
{
	// here we will test bad chars

	int a = 'abcd';	// this should be fine!

	char b = '';		// error
	char c = ';			// error
	char d = '\q';		// error
	char e = '\x1234';	// error
	char f = '\x';		// error
	// the next one is bad as well
	char g = '\
	char h = '\xF';		// this should be fine

}

char z = 'a