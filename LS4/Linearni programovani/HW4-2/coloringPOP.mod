# mnoziny
param H;
set I := 1..H;
set V := 1..H;
set E within (I cross I);
param q := H;

# promenne
var y{I,V} binary;
var z{V,I} binary;

# podminky
s.t. p1 {v in V}: z[v,1] = 0;
s.t. p2 {v in V}: y[H,v] = 0;
s.t. p3 {v in V, i in 1..H-1}: y[i,v] - y[i+1,v] >= 0;
s.t. p4 {v in V, i in 1..H-1}: y[i,v] + z[v,i+1] = 1;
s.t. p5 {(u,v) in E, i in 1..H}: y[i,u] + z[u,i] + y[i,v] + z[v,i] >= 1;
s.t. p6 {v in V, i in 1..H-1}: y[i,q] - y[i,v] >= 0;


# min/max funkce
minimize Colors: sum{i in 1..H} y[i,q];

# vyres to!
solve;

# napis vysledek
printf "#OUTPUT: %d\n", Colors.val + 1;
printf {w in V, v in V : y[v,w] = 1 and y[v+1,w] = 0} "v_%d : %d\n", w,v;
printf "#OUTPUT END\n";

# umri
end;