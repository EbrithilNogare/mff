import os
import importlib
import random

from collections import defaultdict


def reward(m1, m2):

    if m1 not in ['C', 'D']:
        return -1

    if m2 not in ['C', 'D']:
        return 5
    
    if m1 == 'C':
        return 0 if m2 == 'D' else 3
    if m1 == 'D':
        return 1 if m2 == 'D' else 5

def play_game(s1, s2, iters, verbose=False):
    s1.reset()
    s2.reset()
    if verbose:
        print(f'{s1.strategy_name()} vs. {s2.strategy_name()}')
    
    moves1 = ''
    moves2 = ''
    
    R1 = 0
    R2 = 0
    for _ in range(iters):
        m1 = s1.play()
        m2 = s2.play()
        
        moves1 += m1
        moves2 += m2

        r1 = reward(m1, m2)
        r2 = reward(m2, m1)

        # ignore games where errors occured
        if r1 < 0:
            R1 = 0
            break
            
        if r2 < 0:
            R2 = 0
            break

        R1 += r1
        R2 += r2

        s1.last_move(m1, m2)
        s2.last_move(m2, m1)

    if verbose:
        print(f'\t {R1}:{R2}')
        print(f'\t {moves1}')
        print(f'\t {moves2}')
    
    return R1, R2

def tournament(strategies, iters_mean, iters_range, repeats, verbose=False):

    scores = defaultdict(int)

    for s1 in strategies:
        for s2 in strategies:

            if s1 == s2: 
                continue 
            
            for _ in range(repeats):
                iters = iters_mean + random.randint(-iters_range//2, iters_range//2)
                sc1, sc2 = play_game(s1, s2, iters, verbose=verbose)

                scores[s1.strategy_name()] += sc1
                scores[s2.strategy_name()] += sc2

                if sc1 == 0 and sc2 == 0:
                    break

    return scores

if __name__ == '__main__':
    strat_modules = []

    for f in os.listdir('strategies'):
        module_name = '.'.join(f.split('.')[:-1])
        if (module_name): # skips directories and files without '.'
            strat_modules.append(importlib.import_module(f'strategies.{module_name}'))

    strategies = [m.create_strategy() for m in strat_modules]

    scores = tournament(strategies, 200, 50, 10, verbose=True)

    score_board = sorted([(k, v) for k, v in scores.items()], key=lambda x: x[1], reverse=True)

    for name, score in score_board:
        print(f'\t\t{name:40} {score}')
    
