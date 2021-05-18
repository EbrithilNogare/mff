# mnoziny
param N;
set I;
set E within (I cross I);
set D within (0..N cross 0..N);

# promenne
var V{I,I} binary;
var P{I} binary;

# podminky
s.t. JustOneColor {i in I}: sum{j in I} V[i,j] = 1;
s.t. Edges1 {(i,j) in E, k in I}: V[i,k] + V[j,k] <= P[k];
s.t. Edges2 {(i,j) in E, k in I}: V[i,j] = 0;
s.t. OrderedEdges {i in I, j in I: i > j }: V[i,j] = 0;


# min/max funkce
minimize Colors: sum{j in I} P[j];

# vyres to!
solve;

# napis vysledek
printf "#OUTPUT: %d\n", Colors.val;
printf {i in I, j in I: V[i,j] == 1} "v_%d : %d\n", i, j;
printf {(d, s) in D, j in I: V[d,j] == 1} "v_%d : %d\n", s, j;
printf "#OUTPUT END\n";

# umri
end;
