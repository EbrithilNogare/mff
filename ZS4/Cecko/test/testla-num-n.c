
int main(int argc, char** argv)
{
	int a = 12345678901234567890;	// too long
	int b = 0x0123456789abcdef;		// too long

	int c = 123abc;					// malformed
	int d = 0xabcdefghijkl;			// malformed

	int e = 12345678901234567890abc;	// malformed and long
	int f = 0x0123456789abcdefghijkl;	// malformed and long
}
