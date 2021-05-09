import re
from sys import stdin

mode, nodes, lines = input().split()
nodes = int(nodes)

table = [[0 for c in range(nodes)] for r in range(nodes)]
for line in stdin:
	table[int(line.split()[0])][int(line.split()[2])] = 1

print("data;")
print("param N:=",nodes-1,";")
print("set E :=")

for row in range(len(table),-1,-1):
	for column in range(row+1, len(table)):
		if(table[row][column] == 0):
			print("("+str(row)+","+str(column)+")")
print(";")
