displacement bumps( float mult = 5, Kd = 0.4)
{
	normal Nn = normalize(N);
	float a,b,d;
	point Po = transform("object",P)*mult;

	a = noise(Po);
	a = -a*a;

	b = noise(Po * 10);
	b = -b*b;

	d = a + 0.4*b;

	P += Nn * Kd * d;
	N = calculatenormal(P);
}
