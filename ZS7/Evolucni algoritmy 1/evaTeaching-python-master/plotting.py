# use and edit this file to make all the plots you need - it is generally easier
# than plotting directly after the run of the algorithm

import utils

import matplotlib.pyplot as plt 

plt.figure(figsize=(12,8))

#utils.plot_experiments('tsp', ['default', 'create_short_ind', 'edge_recombination_cross', 'create_short_ind_+_edge_recombination_cross', 'tmp'])

plt.title('TSP')
plt.rc('lines', linestyle='--', lw=5)
utils.plot_experiments('tsp', ['default'])
plt.rc('lines', linestyle='-', lw=1)
utils.plot_experiments('tsp', ['create_short_ind', 'edge_recombination_cross', 'create_short_ind_+_edge_recombination_cross', 'opt2_mutate', 'create_short_ind_+_edge_recombination_cross_+_opt2_mutate', 'edge_recombination_cross_+_opt2_mutate'])
#utils.plot_experiments('tsp', ['tmp'])
#
#plt.title('ZDT2')
#plt.rc('lines', linestyle='--', lw=5)
#utils.plot_experiments('tsp', ['default.ZDT2'])
#plt.rc('lines', linestyle='-', lw=1)
#utils.plot_experiments('tsp', ['arithmetic_cross.ZDT2', 'differential_mutation.ZDT2', 'assign_hypervolume_contribution.ZDT2'])
#
#plt.title('ZDT3')
#plt.rc('lines', linestyle='--', lw=5)
#utils.plot_experiments('tsp', ['default.ZDT3'])
#plt.rc('lines', linestyle='-', lw=1)
#utils.plot_experiments('tsp', ['arithmetic_cross.ZDT3', 'differential_mutation.ZDT3', 'assign_hypervolume_contribution.ZDT3'])
#
#plt.title('ZDT4')
#plt.rc('lines', linestyle='--', lw=5)
#utils.plot_experiments('tsp', ['default.ZDT4'])
#plt.rc('lines', linestyle='-', lw=1)
#utils.plot_experiments('tsp', ['arithmetic_cross.ZDT4', 'differential_mutation.ZDT4', 'assign_hypervolume_contribution.ZDT4'])
#
#plt.title('ZDT6')
#plt.rc('lines', linestyle='--', lw=5)
#utils.plot_experiments('tsp', ['default.ZDT6'])
#plt.rc('lines', linestyle='-', lw=1)
#utils.plot_experiments('tsp', ['arithmetic_cross.ZDT6', 'differential_mutation.ZDT6', 'assign_hypervolume_contribution.ZDT6'])


#plt.yscale("log")
plt.show()
