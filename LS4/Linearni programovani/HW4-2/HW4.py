from sys import stdin

#
# author : David Napravnik
#
# references:
# https://arxiv.org/pdf/1706.10191.pdf?fbclid=IwAR0z8dDI9hWEr3WMu-Xq7uIIuCLG7lHQHm4LuciI-v5fYHPW192sdwlspk4
#

mode, nodes, lines = input().split()
nodes = int(nodes)

print("data;")
print("param N:=",nodes-1,";")

graph = [[1 for c in range(nodes)] for r in range(nodes)]

for line in stdin:
	graph[int(line.split()[0])][int(line.split()[2])] = 0
	graph[int(line.split()[2])][int(line.split()[0])] = 0

for i in range(len(graph)):
	graph[i][i] = 0


dominators = []
dominating = []

# A vertex u is dominated by a vertex v, v != u, if the neighborhood of u is a subset of the
# neighborhood of v. In this case, the vertex u can be deleted from G, the remaining graph
# can be colored, and at the end, u can get the same color as v.
def isDominator(u, v):
	if(u==v): return False
	if(u in dominators): return False
	for i in range(len(graph)):
		if(graph[v][i] - graph[u][i] < 0):
			return False
	return True

#for i in range(len(graph)):
#	print(graph[i])

for i in range(len(graph)):
	for j in range(len(graph)):
		if(isDominator(i,j)):
			dominators.append(i)
			dominating.append([j,i])



print("set D:=", end=" ")
for i in range(len(dominating)):
	[a,b] = dominating[i]
	print("(",a,",",b,")", end=" ")
print(";")

print("set I:=", end=" ")
for i in range(len(graph)):
	if not i in dominators:
		print(i, end=" ")
print(";")

print("set E:=", end=" ")
for row in range(len(graph)):
	for column in range(row+1, len(graph)):
		if(graph[row][column] == 1 and (row not in dominators) and (column not in dominators)):
			print("("+str(row)+","+str(column)+")", end=" ")
print(";")
