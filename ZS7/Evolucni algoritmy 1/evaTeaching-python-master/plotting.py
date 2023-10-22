# use and edit this file to make all the plots you need - it is generally easier
# than plotting directly after the run of the algorithm

import utils

import matplotlib.pyplot as plt 

plt.figure(figsize=(12,8))
utils.plot_experiments('partition', ['defaultMutation', 'myMutation2', 'myMutation3', 'myMutation4'])
plt.yscale("log")
plt.show()
