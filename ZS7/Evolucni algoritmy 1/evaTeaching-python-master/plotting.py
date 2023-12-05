# use and edit this file to make all the plots you need - it is generally easier
# than plotting directly after the run of the algorithm

import utils

import matplotlib.pyplot as plt 

plt.figure(figsize=(12,8))

plt.title('F')
utils.plot_experiments('differential', ['f3cr1.f02', 'd8cr9.f02'])

#plt.title('F01')
#plt.rc('lines', linestyle='--', lw=5)
#utils.plot_experiments('differential', ['cont.f01'])
#plt.rc('lines', linestyle='-', lw=1)
#utils.plot_experiments('differential', ['select-3.f01', 'select-5.f01', 'select-7.f01'])


#plt.title('F02')
#plt.rc('lines', linestyle='--', lw=5)
#utils.plot_experiments('differential', ['cont.f02'])
#plt.rc('lines', linestyle='-', lw=1)
#utils.plot_experiments('differential', ['select-3.f02', 'select-5.f02', 'select-7.f02'])
#
#plt.title('F06')
#plt.rc('lines', linestyle='--', lw=5)
#utils.plot_experiments('differential', ['cont.f06'])
#plt.rc('lines', linestyle='-', lw=1)
#utils.plot_experiments('differential', ['select-3.f06', 'select-5.f06', 'select-7.f06'])
#
#plt.title('F08')
#plt.rc('lines', linestyle='--', lw=5)
#utils.plot_experiments('differential', ['cont.f08'])
#plt.rc('lines', linestyle='-', lw=1)
#utils.plot_experiments('differential', ['select-3.f08', 'select-5.f08', 'select-7.f08'])
#
#plt.title('F10')
#plt.rc('lines', linestyle='--', lw=5)
#utils.plot_experiments('differential', ['cont.f10'])
#plt.rc('lines', linestyle='-', lw=1)
#utils.plot_experiments('differential', ['select-3.f10', 'select-5.f10', 'select-7.f10'])
#

plt.yscale("log")
plt.show()
