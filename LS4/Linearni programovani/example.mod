# mnoziny
set I := 0..4;
set J := 0..4;

# promenne
var V{I,J}, >= 0, <= 1, integer;
var P{J}, >= 0, <= 1, integer;

# podminky
s.t. a{i in I}: sum{j in J} V[i,j] = 1;
s.t. b{i in I, j in J}: P[j] >= V[i,j];
s.t. c_1{j in J}: V[0,j] + V[2,j] <= 1;
s.t. c_2{j in J}: V[0,j] + V[4,j] <= 1;
s.t. c_3{j in J}: V[1,j] + V[4,j] <= 1;
s.t. c_4{j in J}: V[2,j] + V[3,j] <= 1;

# min/max funkce
minimize obj: sum{j in J} P[j];

# vyres to!
solve;

# napis vysledek
printf "#OUTPUT: %d\n", obj.val;
printf {i in I, j in J: V[i,j] != 0} "v_%d : %d\n", i, j;
printf "#OUTPUT END\n";

# umri
end;
