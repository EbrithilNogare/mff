import sys


class Person:
    def __init__(self, name):
        self.name = name
    
    def talk(self):
        print(self.name)


def newmethod44():
    persona = Person("me")
    return persona

persona = newmethod44()
persona.talk()
