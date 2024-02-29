import random

class TitForTat:

    @staticmethod
    def author_name():
        return "Martin Pilat"

    @staticmethod
    def strategy_name():
        return "Tit for Tat"

    def __init__(self):
        self.reset()

    def reset(self):
        self.last_op = None

    def last_move(self, my_move, other_move):
        self.last_op = other_move

    def play(self):
        return self.last_op if self.last_op else 'C'

def create_strategy():
    return TitForTat()