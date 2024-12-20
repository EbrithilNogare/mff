import random
import numpy as np
import functools
import math

import co_functions as cf
import utils

# pylint: disable=missing-function-docstring, invalid-name

DIMENSION = 10  # dimension of the problems
POP_SIZE = 100  # population size
MAX_GEN = 500  # maximum number of generations
CX_PROB = 0.8  # crossover probability
MUT_PROB = 0.2  # mutation probability
MUT_STEP = 0.5  # size of the mutation steps
REPEATS = 10  # number of runs of the algorithm (should be at least 10)
OUT_DIR = 'differential'  # output directory for logs
EXP_ID = 'mine'  # the ID of this experiment (used to create log names)
TOURNAMENT_COMPETITOR_COUNT = 2

def create_ind(ind_len):
    return np.random.uniform(-5, 5, size=(ind_len,))

def create_pop(pop_size, create_individual):
    return [create_individual() for _ in range(pop_size)]

# Selection
def tournament_selection(pop, fits, _k):
     selected = []
     for _ in range(len(pop)):
        parents = [random.randrange(0, len(pop)) for _ in range(TOURNAMENT_COMPETITOR_COUNT)]
        selected.append(pop[max(parents, key=lambda ind:fits[ind])])
     return selected

# Crossover
def crossover(pop, cross, cx_prob, generation):
    off = []
    for p1, p2 in zip(pop[0::2], pop[1::2]):
        if random.random() < cx_prob:
            o1, o2 = cross(p1, p2, generation=generation)
        else:
            o1, o2 = p1[:], p2[:]
        off.append(o1)
        off.append(o2)
    return off

def one_pt_cross(p1, p2, generation):
    point = random.randrange(1, len(p1))
    o1 = np.append(p1[:point], p2[point:])
    o2 = np.append(p2[:point], p1[point:])
    return o1, o2

def arithmetic_cross(p1, p2, generation):
    alpha = random.random()
    o1 = alpha * p1 + (1 - alpha) * p2
    o2 = alpha * p2 + (1 - alpha) * p1
    return o1, o2

def simulated_binary_crossover(p1, p2, generation):
    eta = 20 + generation
    o1 = p1[::]
    o2 = p2[::]
    for i, (x1, x2) in enumerate(zip(p1, p2)):
        rand = random.random()
        if rand <= 0.5:
            beta = 2. * rand
        else:
            beta = 1. / (2. * (1. - rand))
        beta **= 1. / (eta + 1.)
        o1[i] = 0.5 * (((1 + beta) * x1) + ((1 - beta) * x2))
        o2[i] = 0.5 * (((1 - beta) * x1) + ((1 + beta) * x2))
    return o1, o2

# Mutation
def mutation(pop, mutate, mut_prob, generation):
    return [mutate(p, pop, generation) if random.random() < mut_prob else p[:] for p in pop]

class Mutation:

    def __init__(self, step_size):
        self.step_size = step_size

    def __call__(self, ind, pop, generation):
        return ind + self.step_size*np.random.normal(size=ind.shape)
    
    def __call__2(self, ind, pop, generation):
        random_ind = pop[np.random.randint(0, len(pop))]
        direction_vector = random_ind - ind
        if np.all(direction_vector == 0):
            direction_vector = np.random.normal(size=ind.shape)
        direction_vector /= np.linalg.norm(direction_vector)
        return ind + self.step_size * direction_vector


def mate(pop, operators, generation):
    for o in operators:
        pop = o(pop, generation=generation)
    return pop




def differential_evolution(pop, max_gen, fitness, operators, mate_sel, mutate_ind, *, map_fn=map, log=None):
    NP = POP_SIZE
    CR = 0.9
    F = 0.8
    evals = 0

    for G in range(max_gen):
        fits_objs = list(map_fn(fitness, pop))
        evals += len(pop)
        if log:
            log.add_gen(fits_objs, evals)
        new_pop = []
        for x_index in range(POP_SIZE):
            x = pop[x_index]
            while True:
                companions = [random.randrange(0, POP_SIZE) for _ in range(7)]
                [a, b, c, d, e, f, g] = companions
                companions.append(x_index)
                unique = len(set(companions)) == 8
                if unique:
                    break
            randomIndex = random.randrange(0, DIMENSION)
            y = [0 for i in range(DIMENSION)]
            [a, b, c, d, e, f, g] = [pop[a], pop[b], pop[c], pop[d], pop[e], pop[f], pop[g]]
            for i in range(DIMENSION):
                if i == randomIndex or random.random() < CR:
                    y[i] = a[i] + F * (b[i] - c[i]) # + F * (d[i] - e[i]) + F * (f[i] - g[i])
                else:
                    y[i] = x[i]
            if fitness(y).fitness > fitness(x).fitness:
                new_pop.append(y)
            else:
                new_pop.append(x)
        pop = new_pop[::]
    return pop





if __name__ == '__main__':

    # use `functool.partial` to create fix some arguments of the functions 
    # and create functions with required signatures
    cr_ind = functools.partial(create_ind, ind_len=DIMENSION)
    # we will run the experiment on a number of different functions
    fit_generators = [cf.make_f01_sphere,
                      cf.make_f02_ellipsoidal,
                      cf.make_f06_attractive_sector,
                      cf.make_f08_rosenbrock,
                      cf.make_f10_rotated_ellipsoidal]
    fit_names = ['f02']
    #fit_names = ['f01', 'f02', 'f06', 'f08', 'f10']

    for fit_gen, fit_name in zip(fit_generators, fit_names):
        fit = fit_gen(DIMENSION)
        mutate_ind = Mutation(step_size=MUT_STEP)
        xover = functools.partial(crossover, cross=arithmetic_cross, cx_prob=CX_PROB)
        mut = functools.partial(mutation, mut_prob=MUT_PROB, mutate=mutate_ind)

        # run the algorithm `REPEATS` times and remember the best solutions from 
        # last generations
    
        best_inds = []
        for run in range(REPEATS):
            # initialize the log structure
            log = utils.Log(OUT_DIR, EXP_ID + '.' + fit_name , run, 
                            write_immediately=True, print_frequency=5)
            # create population
            pop = create_pop(POP_SIZE, cr_ind)
            # run evolution - notice we use the pool.map as the map_fn
            pop = differential_evolution(pop, MAX_GEN, fit, [xover, mut], tournament_selection, mutate_ind, map_fn=map, log=log)
            # remember the best individual from last generation, save it to file
            bi = max(pop, key=fit)
            best_inds.append(bi)
            
        # print an overview of the best individuals from each run
        for i, bi in enumerate(best_inds):
            print(f'Run {i}: objective = {fit(bi).objective}')

        # write summary logs for the whole experiment
        utils.summarize_experiment(OUT_DIR, EXP_ID + '.' + fit_name)
