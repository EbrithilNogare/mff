from sys import stdin

#
# author : David Napravnik
#

nodes = int(input().split()[2])

print("data;")
print("param N :=",nodes-1,";")

graph = [[0 for c in range(nodes)] for r in range(nodes)]

for line in stdin:
	graph[int(line.split()[0])][int(line.split()[2])] = int(line.split()[4][:-1])


def isPartOfCycle(vertex1, vertex2):
	if(graph[vertex1][vertex2] > 0):
		for vertex3 in range(len(graph)):
			if(graph[vertex2][vertex3] > 0):
				if(graph[vertex3][vertex1] > 0):
					return True
				for vertex4 in range(len(graph)):
					if(graph[vertex3][vertex4] > 0 and graph[vertex4][vertex1] > 0):
						return True
	return False

for row in range(len(graph)):
	for column in range(len(graph)):
		if(not isPartOfCycle(row,column)):
			graph[row][column] = 0



#for i in range(len(graph)):
#	print(graph[i])

print("set NotEdges :=", end=" ")
for row in range(len(graph)):
	for column in range(len(graph)):
		if(graph[row][column] == 0):
			print("("+str(row)+","+str(column)+")", end=" ")
print(";")

print("param W :"," ".join(str(x) for x in range(nodes)),":=")
for row in range(len(graph)):
	print(row, end=" ")
	for column in range(len(graph)):
		print(graph[row][column], end=" ")
	print("")
print(";")
