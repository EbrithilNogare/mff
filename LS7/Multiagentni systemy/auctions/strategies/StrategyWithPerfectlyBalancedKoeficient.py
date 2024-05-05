class StrategyWithPerfectlyBalancedKoeficient:

    def __init__(self, num_strategies):
        self.num_strategies = num_strategies
        self.remaining_money = 0
        self.num_auctions = 0
        self.current_auction_number = 0
        self.value = 0
        self.perfectly_balanced_koeficient = min(0.0009 * (num_strategies ** 2) + .0136 * num_strategies + .0339, 1)

    # name of the strategy - make sure it is unique
    def name(self):
        return "Strategy with balanced koeficient"

    # name of the author of the strategy
    def author(self):
        return "David Napravnik"

    # number of auctions that will be simulated - called before the first auction
    def set_num_auctions(self, num_auctions):
        self.num_auctions = num_auctions

    # amount of money available for all auctions - called before the first aution
    def set_money(self, money):
        self.remaining_money = money

    # called after winning an aution with the price that was paid for the object
    def won(self, price):
        self.remaining_money -= price

    # value of the object for this agent - called before every auction
    def set_value(self, value): 
        self.current_auction_number = self.current_auction_number + 1
        self.value = value

    # shows interest in the object for the current price, called in each iteration of each aution
    def interested(self, price, active_strats):
        return price < self.value * self.perfectly_balanced_koeficient and price <= self.remaining_money

def strategy_ascending(num_strategies):
    return StrategyWithPerfectlyBalancedKoeficient(num_strategies)

def strategy_descending(num_strategies):
    return StrategyWithPerfectlyBalancedKoeficient(num_strategies)