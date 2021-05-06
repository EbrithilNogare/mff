# mnoziny
set I := 1..5;
set J := 1..5;

# promenne
var V{I,J}, >= 0, <= 1, integer;
var P{J}, >= 0, <= 1, integer;

# podminky
s.t. a{i in I}: sum{j in J} V[i,j] = 1;
s.t. b{i in I, j in J}: P[j] >= V[i,j];
s.t. c_1{j in J}: V[1,j] + V[4,j] <= 1;
s.t. c_2{j in J}: V[1,j] + V[5,j] <= 1;
s.t. c_3{j in J}: V[2,j] + V[5,j] <= 1;
s.t. c_4{j in J}: V[3,j] + V[4,j] <= 1;

# min/max funkce
minimize obj: sum{j in J} P[j];

# vyres to!
solve;

# napis debugovaci info
display obj;
display V;
printf {i in I, j in J} "%s%s%s%s", (if (V[i,j] = 1) then i else ""), (if (V[i,j] = 1) then ": " else ""), (if (V[i,j] = 1) then j else ""), (if (V[i,j] = 1) then ", " else "");
printf "\n";
display P;

# napis vysledek
printf "result: %d\n", obj.val;

# umri
end;