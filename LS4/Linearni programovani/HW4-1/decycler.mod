# mnoziny
param N;
set I := 0..N;
set NotEdges within (I cross I);
param W{I,I};

# promenne
var E{I,I} binary;

# podminky
s.t. Edges {(i,j) in NotEdges}: E[i,j] = 0;
s.t. Not3Cycle {i in I, j in I, k in I}: E[i,j] + E[j,k] + E[k,i] <= 2;
s.t. Not4Cycle {i in I, j in I, k in I, l in I}: E[i,j] + E[j,k] + E[k,l] + E[l,i] <= 3;

# min/max funkce
maximize obj: sum{i in I, j in I} (W[i,j] * E[i,j]);

# vyres to!
solve;

# napis vysledek
printf "#OUTPUT: %d\n", sum{i in I, j in I} (W[i,j]) - obj.val;
printf {i in I, j in I: E[i,j] == 0 and W[i,j] != 0} "%d --> %d\n", i, j;
printf "#OUTPUT END\n";

# umri
end;
