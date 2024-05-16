import random
import copy

NORTH = 0
SOUTH = 1
WEST = 2
EAST = 3

# the environment
class ForagingEnvironment:

    # h, w are the dimensiouns, objects and agents are the numbers of objects and agents respectively
    def __init__(self, h, w, objects, agents):
        self.h = h
        self.w = w
        self.objects = objects
        self.agents = agents
        
        self.reset()

    # reset the environment (generate a new one) and return as a state
    def reset(self):
        self.agent_locations = []
        self.object_locations = []
        self.world = [[0 for _ in range(self.w)] for _ in range(self.h)]
        self.steps = 0

        current_objects = 0
        while current_objects < self.objects:
            i,j = random.randrange(0, self.h), random.randrange(0, self.w)
            if self.world[i][j] == 0:
                current_objects += 1
                ol = random.randrange(1, self.agents)
                self.world[i][j] = ol
                self.object_locations.append((i, j, ol))

        current_agents = 0
        while current_agents < self.agents:
            i,j = random.randrange(0, self.h), random.randrange(0, self.w)
            if self.world[i][j] == 0:
                current_agents += 1
                self.world[i][j] = -1
                self.agent_locations.append([i, j])

        self.history = [(copy.deepcopy(self.world), 0)]

        return copy.deepcopy(self.world), copy.deepcopy(self.agent_locations)

    # update the world after moving the agents
    def _update_world(self):
        self.steps += 1
        self.world = [[0 for _ in range(self.w)] for _ in range(self.h)]

        # count agents on each location and forage objects
        for ag_x, ag_y in self.agent_locations:
            self.world[ag_x][ag_y] += 1
        
        reward = 0
        for ob_x, ob_y, ob_l in self.object_locations:
            if self.world[ob_x][ob_y] >= ob_l: # enough agents to forage object
                reward += ob_l
                self.object_locations.remove((ob_x, ob_y, ob_l))

        # create world
        self.world = [[0 for _ in range(self.w)] for _ in range(self.h)]
        
        for ag_x, ag_y in self.agent_locations:
            self.world[ag_x][ag_y] -= 1

        for ob_x, ob_y, ob_l in self.object_locations:
            self.world[ob_x][ob_y] = ob_l

        return reward - 0.1 # 0.1 is penalty for each step

    # apply the agents' actions in the environment (move agents and collect objects)
    def perform_actions(self, actions):
        nag_loc = []
        for (ag_x, ag_y), act in zip(self.agent_locations, actions):
            nag_x, nag_y = ag_x, ag_y
            if act == NORTH:
                nag_x = max(ag_x - 1, 0)
            if act == SOUTH:
                nag_x = min(ag_x + 1, self.h - 1)
            if act == WEST:
                nag_y = max(ag_y - 1, 0)
            if act == EAST:
                nag_y = min(ag_y + 1, self.w - 1)

            nag_loc.append([nag_x, nag_y])

        self.agent_locations = nag_loc

        r = self._update_world()

        self.history.append((copy.deepcopy(self.world), r))

        return r, copy.deepcopy(self.world), copy.deepcopy(self.agent_locations)

    # clear history (also done by reset())
    def clear_history(self):
        self.history = []

    # is the simulation finished?
    def done(self):
        return not self.object_locations or self.steps >= 500

    # show the world
    def show(self):
        import matplotlib.pyplot as plt
        plt.imshow(self.world, 'PuOr', interpolation=None)
        plt.clim(-len(self.agent_locations), len(self.agent_locations))
        plt.show()

    # render the whole saved history (in this run)
    def render_history(self):    
        import matplotlib.pyplot as plt
        from matplotlib.animation import FuncAnimation

        fig, ax = plt.subplots()
        ln = plt.imshow(self.history[0][0], 'PuOr', interpolation=None)
        plt.title("Score: 0 Step: 0")
        plt.clim(-len(self.agent_locations), len(self.agent_locations))
        score = 0
        step = 0

        def init():
            return ln,

        def update(frame):
            nonlocal score
            nonlocal step
            w, r = frame
            ln.set_data(w)
            score += r
            step += 1
            plt.title(f'Score: {score:.1f} Step: {step}')
            return ln,

        ani = FuncAnimation(fig, update, frames=self.history,
                            init_func=init, blit=False)
        plt.show()