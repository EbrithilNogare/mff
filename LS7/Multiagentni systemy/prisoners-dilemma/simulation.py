import collections
import copy
import importlib
import json
import pprint
import random
import os
import sys

import matplotlib.pyplot as plt
import numpy as np

import tournament

def create_default_config(strat_modules, config_file):
    """
        Creates a configuration file with all strategies found in the modules, 
        each of them 10 times.

        Arguments:

        strat_modules - list with the names of modules with strategies

        config_file - name of the configuration file to create

    """
    config = {sm:10 for sm in strat_modules}
    with open(config_file, 'w') as f:
        json.dump(config, f, indent=1)


def simulate(pop, strategies, max_gen, encounters):
    """
        Simulates the "life" of individuals in `pop` each of them playing the 
        prisoner's dilemma and using strategies from `strategies`.

        Arguments:

        pop - the initial population; each individual is the name of a 
            module with a strategy

        strategies - dictionary of strategies; keys are module names values
            are strategies from these modules

        max_gen - the number of generations to simulate

        encounters - the number of different individuals each individual 
            meets in every generation
        
        Returns:

        A list of dictionaries containing the number of individuals for 
        each strategy in each generation (keys are module names, values are 
        number of instances of strategy in the given generation)
    """
    pop_size = len(pop)

    str_counts = []
    str_scores = []

    for g in range(max_gen):
        scores = pop_size*[0]
        for s1_idx, s in enumerate(pop):
            s1 = strategies[s]
            
            # run one more generation to get the scores for the last generation
            for _ in range(encounters + 1):  
                # select a random strategy and play a game against it
                s2_idx = random.randint(0, pop_size - 1)
                s2 = copy.deepcopy(strategies[pop[s2_idx]])

                sc1, sc2 = tournament.play_game(s1, s2, 200, verbose=False)
                scores[s1_idx] += sc1
                scores[s2_idx] += sc2
        
            
        # save stats about strategies
        counter = collections.Counter(pop)
        counts = {k:counter[k] for k in strategies.keys()}
        print(f'****************** Generation {g} *********************')
        pprint.pprint(counts)
        str_counts.append(counts)
        str_scores.append({k:np.mean([sc for sc, st in zip(scores, pop) if st == k])
                      for k in strategies.keys()})

        pop = np.random.choice(pop, size=pop_size, p=np.array(scores)/np.sum(scores))
        

    return str_counts, str_scores

if __name__ == '__main__':
    strat_modules = []

    # read the strategies
    for f in os.listdir('strategies'):
        if not f.endswith('.py'):
            continue

        module_name = '.'.join(f.split('.')[:-1])
        if module_name: # skips directories and files without '.'
            strat_modules.append(f'strategies.{module_name}')

    # default config file
    config_file = 'config.json'

    if len(sys.argv) > 1:
        config_file = sys.argv[1]

    # config file does not exist - create default
    if not os.path.isfile(config_file):
        create_default_config(strat_modules, config_file)

    # load config file
    with open(config_file) as f:
        config = json.load(f)

    # create population
    pop = []
    strategies = {}
    for k, v in config.items():
        pop += v*[k]
        strategies[k] = importlib.import_module(k).create_strategy()

    # run simulation
    str_counts, str_scores = simulate(pop, strategies, max_gen=10, encounters=10) 

    # create plot
    plt.figure(figsize=(12, 12))
    plt.subplot(2,1,1)
    for s in strategies.keys():
        plt.plot([sc[s] for sc in str_counts], label=strategies[s].strategy_name())

    plt.legend()

    plt.subplot(2,1,2)
    for s in strategies.keys():
        plt.plot([sc[s] for sc in str_scores], label=strategies[s].strategy_name())

    plt.legend()
    plt.savefig(f'{config_file}.png')
    plt.show()

    
    