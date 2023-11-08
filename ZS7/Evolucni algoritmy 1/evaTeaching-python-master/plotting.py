# use and edit this file to make all the plots you need - it is generally easier
# than plotting directly after the run of the algorithm

import utils

import matplotlib.pyplot as plt 

plt.figure(figsize=(12,8))
#utils.plot_experiments('partition', ['defaultMutation', 'myMutation2', 'myMutation3', 'myMutation4'])
utils.plot_experiments('continuous', ['default.f01','default.f02','default.f06','default.f08','default.f10'])
plt.yscale("log")
plt.show()