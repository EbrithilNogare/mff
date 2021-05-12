from sys import stdin

mode, nodes, lines = input().split()
nodes = int(nodes)

print("data;")
print("param H:=",nodes,";")

table = [[1 for c in range(nodes)] for r in range(nodes)]

for line in stdin:
	table[int(line.split()[0])][int(line.split()[2])] = 0
	table[int(line.split()[2])][int(line.split()[0])] = 0

for i in range(len(table)):
	table[i][i] = 0

print("set E:=")
for row in range(len(table)):
	for column in range(row+1, len(table)):
		if(table[row][column] == 1):
			print("("+str(row+1)+","+str(column+1)+")")
print(";")
