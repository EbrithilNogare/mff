class ExampleStrategy6:

    def __init__(self, num_strategies):
        self.num_strategies = num_strategies
        self.remaining_money = 0
        self.num_auctions = 0

    # name of the strategy - make sure it is unique
    def name(self):
        return "Example Strategy 6"

    # name of the author of the strategy
    def author(self):
        return "Martin Pilát"

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
        self.value = value

    # shows interest in the object for the current price, called in each iteration of each aution
    def interested(self, price, active_strats):
        return price <= self.value and price <= self.remaining_money

def strategy_ascending(num_strategies):
    return ExampleStrategy6(num_strategies)

def strategy_descending(num_strategies):
    return ExampleStrategy6(num_strategies)