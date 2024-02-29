import random

class AlwaysCooperate:

    @staticmethod
    def author_name():
        return "Martin Pilat"

    @staticmethod
    def strategy_name():
        return "Always Cooperate"

    def __init__(self):
        pass

    def reset(self):
        pass

    def last_move(self, my_move, other_move):
        pass

    def play(self):
        return 'C'

def create_strategy():
    return AlwaysCooperate()