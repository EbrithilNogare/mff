// compound operators

int main(int argc, char** argv)
{
	int ar, br, cr, dr, er, fr, gr, hr, ir;
	{
		int ai, bi, ci, di, ei, fi, gi, hi, ii;

		ai = 7;
		bi = 9;
		ci = 11;
		di = 13;
		ei = 15;
		fi = 17;
		gi = 19;
		hi = 21;
		ii = 23;

		ar = ++ai;
		br = bi++;
		cr = --ci;
		dr = di--;
		er = ei += 5;
		fr = fi -= 4;
		gr = gi *= 3;
		hr = hi /= 2;
		ir = ii %= 6;

		printf("ai=%d bi=%d ci=%d di=%d ei=%d fi=%d gi=%d hi=%d ii=%d\n", ai, bi, ci, di, ei, fi, gi, hi, ii);
		printf("ar=%d br=%d cr=%d dr=%d er=%d fr=%d gr=%d hr=%d ir=%d\n", ar, br, cr, dr, er, fr, gr, hr, ir);
	}
	{
		char ai, bi, ci, di, ei, fi, gi, hi, ii;

		ai = 255;
		bi = 255;
		ci = 0;
		di = 0;
		ei = 255;
		fi = 0;
		gi = 200;
		hi = 200;
		ii = 200;

		ar = ++ai;
		br = bi++;
		cr = --ci;
		dr = di--;
		er = ei += 5;
		fr = fi -= 4;
		gr = gi *= 3;
		hr = hi /= 2;
		ir = ii %= 6;

		printf("ai=%d bi=%d ci=%d di=%d ei=%d fi=%d gi=%d hi=%d ii=%d\n", ai, bi, ci, di, ei, fi, gi, hi, ii);
		printf("ar=%d br=%d cr=%d dr=%d er=%d fr=%d gr=%d hr=%d ir=%d\n", ar, br, cr, dr, er, fr, gr, hr, ir);
	}
	{
		const char* ai, * bi, * ci, * di, * ei, * fi;
		const char* ar, * br, * cr, * dr, * er, * fr;
		const char* d;
		
		d = "ABCDEFGHIJ";

		ai = d + 4;
		bi = d + 4;
		ci = d + 4;
		di = d + 4;
		ei = d + 4;
		fi = d + 4;

		ar = ++ai;
		br = bi++;
		cr = --ci;
		dr = di--;
		er = ei += 5;
		fr = fi -= 4;

		printf("*ai=%c *bi=%c *ci=%c *di=%c *ei=%c *fi=%c\n", *ai, *bi, *ci, *di, *ei, *fi);
		printf("*ar=%c *br=%c *cr=%c *dr=%c *er=%c *fr=%c\n", *ar, *br, *cr, *dr, *er, *fr);
	}
	return 0;
}
