// struct

struct mighty_str {
	char c;
	int x,y;
};

int main(int argc, char** argv)
{
	struct mighty_str ms;

	ms.c = 'x';

	struct mighty_str *ps;
	ps = &ms;
	ps->x = ps->y = 1;

	struct god_str {
		struct mighty_str ims_;
	} gs;
	gs.ims_.c = 'G';

	return 0;
}
