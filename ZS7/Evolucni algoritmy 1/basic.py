import random

MAX_GEN = 100
POP_SIZE = 50
IND_SIZE = 10
MUT_PROB = 1/IND_SIZE
CROSS_PROB = .8

def fitness(individual):
	return sum(individual)

def evolution(population):
	for _ in range(MAX_GEN):
		fitnesses = [fitness(x) for x in population]
		best=max(population, key=fitness)
		mating_pool = selection(population, fitnesses, len(population))
		offspring = []
		for p1, p2 in zip(mating_pool[::2], mating_pool[1::2]):
			o1, o2 = crossover(p1, p2)
			o1 = mutation(o1)
			o2 = mutation(o2)
			offspring.append(o1)
			offspring.append(o2)
		population = offspring[:]
		population[0] = best
	return population

def generate_population():
    return [[random.randint(0, 1) for _ in range(POP_SIZE)] for _ in range(IND_SIZE)]

def crossover(parent1, parent2):
	if random.random() < CROSS_PROB:
		crossover_point = random.randrange(0, IND_SIZE)
		child1 = parent1[:crossover_point] + parent2[crossover_point:]
		child2 = parent2[:crossover_point] + parent1[crossover_point:]
		return child1, child2
	else:
		return parent1, parent2


def mutation(individual):
	return [1-g if random.random() < MUT_PROB else g for g in individual]

def selection(population, fitnesses, size):
	return random.choices(population, fitnesses, k=size)

def sum_2d_array(arr_2d):
    total = 0
    for row in arr_2d:
        for element in row:
            total += element
    return total

def main():
	population = generate_population()
	print(sum_2d_array(population))
	population = evolution(population)
	print(sum_2d_array(population))

main()