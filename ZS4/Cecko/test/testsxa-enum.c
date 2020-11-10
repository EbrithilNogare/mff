// enum

enum days { MON, TUE, WED, THU, WRI, SAT=15, SUN };

int main(int argc, char** argv)
{
	enum days day;
	day = SAT;

	enum rgb { RED, GREEN, BLUE } color;
	color = GREEN;
	return color;
}
