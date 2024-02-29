import random

class TitForTat:

    @staticmethod
    def author_name():
        return "David Napravnik"

    @staticmethod
    def strategy_name():
        return "Forgiving resilient tit for tat"

    def __init__(self):
        self.reset()

    def reset(self):
        self.last_op = 'C'
        self.CooperateCount = 0
        self.DefectCount = 0

    def last_move(self, my_move, other_move):
        self.last_op = other_move
        if other_move == "C":
            self.CooperateCount += 1
        else:
            self.DefectCount += 1

    def play(self):
        # forgive
        if random.random() <= .01:
            self.DefectCount = 0
            return 'C'
        # resilient
        if self.DefectCount > 10:
            return 'D'
        # tit for tat
        return self.last_op

def create_strategy():
    return TitForTat()