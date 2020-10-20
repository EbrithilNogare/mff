
int main(int argc, char** argv)
{
	// here we will test bad strings
	char *a = "EOL in string
	char *b = "Too long hex \xabcescape";
	char *c = "";	// empty string should be fine
	char *d = "Bad escape \u";

	// the next one is bad
	char *q = "\

}

char *z = "EOF in string