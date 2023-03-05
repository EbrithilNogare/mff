point alterCenter(point P) 
{
    point thiscell = point (floor(xcomp(P)), floor(ycomp(P)), floor(zcomp(P)) ) + 0.5;
    float dist_to_nearest = 1000;
    uniform float i, j, k;
    point nearestfeature;
    
    for (i = -1; i <= 1; i += 1) {
        for (j = -1; j <= 1; j += 1) {
            for (k = -1; k <= 1; k += 1) {
                point testcell = thiscell + vector(i,j,k);
                point pos = testcell + vector cellnoise(testcell) - 0.5;
                vector offset = pos - P;
                float dist = distance( pos, P );
                if (dist < dist_to_nearest) {
                    dist_to_nearest = dist;
                    nearestfeature = pos;
                }
            }
        }
    }
    return nearestfeature;
}


surface voronoi()
{
    float f1 = .1;
    float spos1, tpos1;
    float freq = 4;

    point pp = P * freq;
    
    point posP = alterCenter(pp*(1, 1, 1));

    Ci = color cellnoise( posP );
    
    float cen = smoothstep(0.04,.05,f1);
    Ci = mix(color(1,1,1),Ci,cen);


    normal Nf = faceforward (normalize(N),I);
    Ci *= Os * ( Cs * (ambient() + 1*diffuse(Nf)));
}
