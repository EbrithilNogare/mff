import random

import foraging

class ExampleAgent:

    def __init__(self, w, h, a):
        self.width = w
        self.height = h
        self.agents = a

    # returns list of actions for all the agents
    def action(self, state):
        world_map, agent_locations = state # state is a tuple
        return [random.randrange(0,4) for _ in agent_locations]

    # is called to inform the agents about the reward from previous step
    def reward(self, reward):
        pass


if __name__ == '__main__':

    WIDTH = 5
    HEIGHT = 5
    OBJECTS = 10
    AGENTS = 5

    env = foraging.ForagingEnvironment(WIDTH, HEIGHT, OBJECTS, AGENTS)

    agent = ExampleAgent(WIDTH, HEIGHT, AGENTS)

    for _ in range(5):
        state = env.reset()
        R = 0
        while not env.done():
            reward, *state = env.perform_actions(agent.action(state))
            agent.reward(reward)
            R += reward
        print(f'Finished with reward: {R}')
    
    #env.render_history()