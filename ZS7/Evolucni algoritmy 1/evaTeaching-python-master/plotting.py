# use and edit this file to make all the plots you need - it is generally easier
# than plotting directly after the run of the algorithm

import utils

import matplotlib.pyplot as plt 

plt.figure(figsize=(12,8))

utils.plot_experiments('continuous', ['default.f02', 'mine.f02'])

#plt.title('F01')
#plt.rc('lines', linestyle='--', lw=5)
#utils.plot_experiments('continuous', ['default.f01'])
#plt.rc('lines', linestyle='-', lw=1)
#utils.plot_experiments('continuous', ['mutationWithDirection.f01', 'arithmetic_cross.f01', 'simulated_binary_crossover.f01', 'arithmetic_cross+mutationWithDirection.f01'])

#plt.title('F02')
#plt.rc('lines', linestyle='--', lw=5)
#utils.plot_experiments('continuous', ['default.f02'])
#plt.rc('lines', linestyle='-', lw=1)
#utils.plot_experiments('continuous', ['mutationWithDirection.f02', 'arithmetic_cross.f02', 'simulated_binary_crossover.f02', 'arithmetic_cross+mutationWithDirection.f02'])
#
#plt.title('F06')
#plt.rc('lines', linestyle='--', lw=5)
#utils.plot_experiments('continuous', ['default.f06'])
#plt.rc('lines', linestyle='-', lw=1)
#utils.plot_experiments('continuous', ['mutationWithDirection.f06', 'arithmetic_cross.f06', 'simulated_binary_crossover.f06', 'arithmetic_cross+mutationWithDirection.f06'])
#
#plt.title('F08')
#plt.rc('lines', linestyle='--', lw=5)
#utils.plot_experiments('continuous', ['default.f08'])
#plt.rc('lines', linestyle='-', lw=1)
#utils.plot_experiments('continuous', ['mutationWithDirection.f08', 'arithmetic_cross.f08', 'simulated_binary_crossover.f08', 'arithmetic_cross+mutationWithDirection.f08'])
#
#plt.title('F10')
#plt.rc('lines', linestyle='--', lw=5)
#utils.plot_experiments('continuous', ['default.f10'])
#plt.rc('lines', linestyle='-', lw=1)
#utils.plot_experiments('continuous', ['mutationWithDirection.f10', 'arithmetic_cross.f10', 'simulated_binary_crossover.f10', 'arithmetic_cross+mutationWithDirection.f10'])
#

plt.yscale("log")
plt.show()
