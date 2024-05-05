import importlib
import os
import random
import config

from collections import defaultdict

# simulates a single english auction with a set of strategies
def simulate_english(strategies):
    base_value = random.randint(100, 200)
    str_values = {strat.name() : base_value + random.randint(-50, 50) 
                    for strat in strategies}
    
    for s in strategies:
        s.set_value(str_values[s.name()])

    active_strategies = set(strategies)
    for current_price in range(10, 300):
        act_count = len(active_strategies)
        active_strategies = {s for s in strategies 
                               if s.interested(current_price, act_count)}
        if len(active_strategies) == 1:
            winner = next(iter(active_strategies)) # python can't take an element from set directly
            return winner, str_values[winner.name()] - current_price, current_price
        elif len(active_strategies) == 0:
            return None, 0, 0

    return None, 0, 0

# simulates a single dutch auction with a set of strategies
def simulate_dutch(strategies):

    base_value = random.randint(100, 200)
    str_values = {strat.name() : base_value + random.randint(-50, 50) 
                    for strat in strategies}
    
    for s in strategies:
        s.set_value(str_values[s.name()])

    active_strategies = set(strategies)
    for current_price in range(300, 10, -1):
        active_strategies = {s for s in strategies 
                               if s.interested(current_price, 0)}
        if active_strategies:
            winner = random.sample(active_strategies, 1)[0]
            return winner, str_values[winner.name()] - current_price, current_price

    return None, 0, 0

# simulates multiple auctions
def simulate_multiple(strategies, auction_type='english', count=100):

    if auction_type == 'english':
        auc_fn = simulate_english
    elif auction_type == 'dutch':
        auc_fn = simulate_dutch

    str_money = {strat.name() : 1000 for strat in strategies}
    for s in strategies:
        s.set_num_auctions(count)
        s.set_money(str_money[s.name()])

    str_profits = defaultdict(int)

    for _ in range(count):
        winner, profit, price = auc_fn(strategies)
        if not winner:
            continue
        # uncomment the line below to see all the sales
        # print(f'{winner.name()} won for profit {profit} paying {price}')
        winner.won(price)
        str_money[winner.name()] -= price
        str_profits[winner.name()] += profit + price
        if (str_money[winner.name()] < 0):
            str_profits[winner.name()] = -1000000  # winner did not have money to pay
            strategies.remove(winner)

    for s in strategies:
        str_profits[s.name()] += str_money[s.name()]

    return str_profits

if __name__ == '__main__':
    #for strategyParam in range(10, 1, -1):
    #    config.strategyParamGlobal = strategyParam / 10.0
    results = {}
    for _ in range(100):
        strat_modules = []

        for f in os.listdir('strategies'):
            module_name = '.'.join(f.split('.')[:-1])
            if (module_name): # skips directories and files without '.'
                strat_modules.append(importlib.import_module(f'strategies.{module_name}'))

        num_strategies = len(strat_modules)

        strategies_english = [m.strategy_ascending(num_strategies) for m in strat_modules]
        strategies_dutch = [m.strategy_descending(num_strategies) for m in strat_modules]

        profits_english = simulate_multiple(strategies_english, 'english')
        profits_dutch = simulate_multiple(strategies_dutch, 'dutch')

        score_board_english = sorted([(k, v) for k, v in profits_english.items()], key=lambda x: x[1], reverse=True)
        score_board_dutch = sorted([(k, v) for k, v in profits_dutch.items()], key=lambda x: x[1], reverse=True)

        #print('*'*30, ' English Aution ', '*'*30)

        for name, score in score_board_english:
            if name in results:
                results[name] += score
            else:
                results[name] = score
            #print(f'\t\t{name:40} {score}')

        #print('*'*31, ' Dutch Aution ', '*'*31)

        for name, score in score_board_dutch:
            if name in results:
                results[name] += score
            else:
                results[name] = score
            #print(f'\t\t{name:40} {score}')

    for name, score in results.items():
        #if name == 'Static Strategy':
        print(f'{name:25} {score:10} {config.strategyParamGlobal:4} {len(results):4}')
