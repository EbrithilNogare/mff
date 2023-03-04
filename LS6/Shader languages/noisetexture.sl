#define snoise(x) (2 * noise(x) - 1);
#define vsnoise(x) (2 * (vector noise(x)) - 1);

float pulse( float edge0, edge1; float x)
{
return step( edge0, x ) - step( edge1, x );
} 

surface
noisetexture (
    float Ka = 1;
    float Kd = .5;
    float Ks = .5;
    float roughness = .1;
    color specularcolor = 1;
) {
    normal Nf = faceforward (normalize(N),I);
    Ci = diffuse(Nf);
    
    
    float freq = 20.0;
    float size = 2;
    float magnitude = .6;
    
    float ss = size * u + magnitude * snoise(P);
    float tt = size * v + magnitude * snoise(P); 


    ss = 1 - pulse( 0.45, 0.55, mod(ss * freq,1) );
    tt = 1 - pulse( 0.45, 0.55, mod(tt * freq,1) );


    color Ci2 = ss * tt * 0.8 + snoise(P * 10);
    Ci *= Ci2 / 5 + .8;
    Oi = Os;
    Ci *= Os * ( Cs * (Ka*ambient() + Kd*diffuse(Nf)) + specularcolor * Ks*specular(Nf,-normalize(I),roughness));
}