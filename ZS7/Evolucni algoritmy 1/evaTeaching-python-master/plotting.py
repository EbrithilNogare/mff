# use and edit this file to make all the plots you need - it is generally easier
# than plotting directly after the run of the algorithm

import utils

import matplotlib.pyplot as plt 

plt.figure(figsize=(12,8))

#utils.plot_experiments('multi', ['default.ZDT1', 'default.ZDT2', 'default.ZDT3', 'default.ZDT4', 'default.ZDT6'])
utils.plot_experiments('multi', ['default.ZDT1', 'arithmetic_cross.ZDT1'])

#plt.title('ZDT1')
#plt.rc('lines', linestyle='--', lw=5)
#utils.plot_experiments('multi', ['default.ZDT1'])
#plt.rc('lines', linestyle='-', lw=1)
#utils.plot_experiments('multi', ['default.ZDT1'])


plt.yscale("log")
plt.show()
