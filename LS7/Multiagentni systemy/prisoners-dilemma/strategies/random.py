import random

class Random:

    @staticmethod
    def author_name():
        return "Martin Pilat"

    @staticmethod
    def strategy_name():
        return "Random"

    def __init__(self):
        pass

    def reset(self):
        pass

    def last_move(self, my_move, other_move):
        pass

    def play(self):
        return 'C' if random.randint(0,1) == 0 else 'D'

def create_strategy():
    return Random()