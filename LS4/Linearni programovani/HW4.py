import re
from sys import stdin

mode, nodes, lines = input().split()
nodes = int(nodes)

table = [[0 for c in range(nodes)] for r in range(nodes)]
for i in range(len(table)):
	table[i][i] = 1

for line in stdin:
	table[int(line.split()[0])][int(line.split()[2])] = 1


print("""
# mnoziny
set I := 0..""",nodes-1,""";
set J := 0..""",nodes-1,""";

# promenne
var V{I,J}, >= 0, <= 1, integer;
var P{J}, >= 0, <= 1, integer;

# podminky
s.t. a{i in I}: sum{j in J} V[i,j] = 1;
""")

counter = 0
for row in range(len(table)):
	for column in range(0, len(table)):
		if(table[row][column] == 0):
			if(column >= row):
				print("s.t. d_"+str(counter)+"{j in J}: V[",row,",j] + V[",column,",j] <= P[j];")
			print("s.t. c_"+str(counter)+": V[",row,",",column,"] = 0;")
			counter+=1


print("""
# min/max funkce
minimize obj: sum{j in J} P[j];

# vyres to!
solve;
""")

if(0==1):
	print("""
# napis debugovaci info
display obj;
display V;
printf {i in I, j in J} "%s%s%s%s", (if (V[i,j] = 1) then i else ""), (if (V[i,j] = 1) then ": " else ""), (if (V[i,j] = 1) then j else ""), (if (V[i,j] = 1) then ", " else "");
printf "\\n";
display P;
	""")

print("""
printf "#OUTPUT: %d\\n", obj.val;
printf {i in I, j in J: V[i,j] != 0} "v_%d : %d\\n", i, j;
printf "#OUTPUT END\\n";
""")

print("end;")
