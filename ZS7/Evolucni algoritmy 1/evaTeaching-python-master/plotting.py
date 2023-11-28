# use and edit this file to make all the plots you need - it is generally easier
# than plotting directly after the run of the algorithm

import utils

import matplotlib.pyplot as plt 

#plt.figure(figsize=(12,8))

utils.plot_experiments('differential', ['default.f02'])

#plt.title('F01')
#plt.rc('lines', linestyle='--', lw=5)
#utils.plot_experiments('continuous', ['default.f01'])
#plt.rc('lines', linestyle='-', lw=1)
#utils.plot_experiments('continuous', ['mutationWithDirection.f01', 'arithmetic_cross.f01', 'simulated_binary_crossover.f01', 'arithmetic_cross+mutationWithDirection.f01'])

plt.yscale("log")
plt.show()
