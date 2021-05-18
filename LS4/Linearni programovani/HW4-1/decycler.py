from sys import stdin

#
# author : David Napravnik
#

nodes = int(input().split()[2])

print("data;")
print("param N:=",nodes-1,";")

edges = [[0 for c in range(nodes)] for r in range(nodes)]
weights = [[0 for c in range(nodes)] for r in range(nodes)]

for line in stdin:
	edges[int(line.split()[0])][int(line.split()[2])] = 1
	weights[int(line.split()[0])][int(line.split()[2])] = int(line.split()[4][:-1])

#for i in range(len(edges)):
#	print(edges[i])
#	
#for i in range(len(weights)):
#	print(weights[i])

# odebrat vrcholy, ktere jsou budou jen ven, nebo jen dovnitr
for row in range(len(edges)):
	highestVertex = True
	for column in range(len(edges)):
		if(edges[row][column] == 1):
			highestVertex = False
	if(highestVertex):
		for column in range(len(edges)):
			edges[column][row] = 0
		

print("set NotEdges :=", end=" ")
for row in range(len(edges)):
	for column in range(len(edges)):
		if(edges[row][column] == 0):
			print("("+str(row)+","+str(column)+")", end=" ")
print(";")

print("param W :"," ".join(str(x) for x in range(nodes)),":=")
for row in range(len(edges)):
	print(row, end=" ")
	for column in range(len(edges)):
		print(weights[row][column], end=" ")
	print("")
print(";")
