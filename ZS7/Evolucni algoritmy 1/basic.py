import random
import numpy as np

MAX_GEN = 100
POP_SIZE = 50
IND_SIZE = 10
MUT_PROB = 1/IND_SIZE/POP_SIZE
CROSS_PROB = .8

evals = []
best_fit = []


def fitness(individual):
	# return sum(individual)/IND_SIZE # OneMAX
	return sum([1-g if i % 2 == 0 else g for i, g in enumerate(individual)])/IND_SIZE

def getFitnessOfPopulation(population):
    total = 0
    for individual in population:
            total += fitness(individual)
    return total/POP_SIZE

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
		evals.append(getFitnessOfPopulation(population));
		best_fit.append(fitness(best));
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


def main():
	population = generate_population()
	print("before:", getFitnessOfPopulation(population))
	population = evolution(population)
	print("after: ", getFitnessOfPopulation(population))
	print("best: ", population[0])

main()


evals = np.array(evals)
best_fit = np.array(best_fit)

# plot the converegence graph and quartiles
import matplotlib.pyplot as plt
plt.plot(evals[0,:], np.median(best_fit, axis=0))
plt.fill_between(evals[0,:], np.percentile(best_fit, q=25, axis=0),
							np.percentile(best_fit, q=75, axis=0), alpha = 0.2)
plt.show()