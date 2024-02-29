import random

class AlwaysDefect:

    @staticmethod
    def author_name():
        return "Martin Pilat"

    @staticmethod
    def strategy_name():
        return "Always Defect"

    def __init__(self):
        pass

    def reset(self):
        pass

    def last_move(self, my_move, other_move):
        pass

    def play(self):
        return 'D'

def create_strategy():
    return AlwaysDefect()