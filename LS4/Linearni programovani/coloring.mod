# mnoziny
param N;
set V := 1..N;
set E within (V cross V);

var w{1..N} binary;
var x{V,1..N} binary;

minimize Colors: sum{i in 1..N} w[i];

subject to Assigned {i in V}:
sum{j in 1..N}x[i,j]=1;

subject to Edges {(i,j) in E, k in 1..N}:
x[i,k] + x[j,k] <= w[k];

solve;

# napis vysledek
printf "%d\n", Colors.val;

# umri
end;
