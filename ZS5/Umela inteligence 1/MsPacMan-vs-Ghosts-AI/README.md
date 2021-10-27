# Ms. Pac-Man vs. Ghosts Version 2.3

![screenshot](mspac.png)

## Overview

This is a Java implementation of the classic video game Ms. Pac-Man.  You can play it live from the keyboard, but it is mostly intended for experimenting with algorithms that play the game automatically. You can write your own agent that plays the game using the API described below.

## History

Midway released the arcade game Ms. Pac-Man in 1981.  Like its predecessor Pac-Man, it quickly became popular around the world.

In recent years, the [University of Essex](https://www.essex.ac.uk/) ran a series of competitions for software agents that attempt to play Ms. Pac-Man automatically, including the [Ms. Pac-Man versus Ghosts Competition](https://ieeexplore.ieee.org/document/5949599) from 2011-12.  Simon Lucas wrote an implementation of Ms. Pac-Man for that competition, and it was later modified by Philipp Rohlfshagen and other authors.  The implementation here is derived from that code, with further changes by Jakub Gemrot and [Adam Dingle](https://ksvi.mff.cuni.cz/~dingle/) from the Faculty of Mathematics and Physics at [Charles University](https://cuni.cz/UKEN-1.html).

## Building the game

This version of Ms. Pac-Man vs. Ghosts works with Java 11 or newer, and possibly older Java versions as well.

This project includes Maven build files.  You should easily be able to load it into Eclipse, Intelli/J, or Visual Studio Code, all of which understand Maven builds.

## Playing the game

To play the game from the keyboard on Linux or macOS, run

```
$ ./mspac
```

Or, on Windows:

```
> .\mspac
```

By default, the game is controlled from the keyboard.  Various options are available; type './mspac -help' to see them.

Use the arrow keys to move.  You can press 'P' to pause the game, or 'N' to advance the game by a single frame.

## Writing an agent

Here is the [documentation](https://ksvi.mff.cuni.cz/~dingle/2020-1/ai_1/ms_pacman/api.html) for the API you can use to build agents to play the game.

The package controllers.pacman.examples contains several sample agents, which you may wish to study as a starting point.

You can enhance the MyAgent class to build a custom agent.  On each tick, the game will call your implementation of the tick() method, where you can decide which action Ms. Pac-Man should take.

The Game interface has everything you need to find about the game state. Note that

- The maze is represented as a graph of __nodes__.  Each node is a distinct position in the maze and has a unique integer index.  There are about 1,000 nodes in each maze (the exact number varies from level to level) and they are evenly spaced throughout the maze.
- Each pill in the maze has a unique index.
- Ghosts are numbered from 0 to 3.
- Directions are listed at the top of interface Game (UP=0, RIGHT=1, etc.)
- The game normally runs at 25 ticks per second.

For more details, see the [API documentation](https://ksvi.mff.cuni.cz/~dingle/2020-1/ai_1/ms_pacman/api.html).

To see your agent play the game, run

```
$ ./mspac MyAgent
```

## Evaluating your agent

The -sim option will run a series of random games without visualization, and will report statistics about an agent's average performance over these games.  For example, to run 20 games of MyAgent:

```
$ ./mspac MyAgent -sim 20
```

If you want to see the outcome of each individual simulated game, add the -v (verbose) option:

```
$ ./mspac MyAgent -sim 20 -v
```

In this output you will see the random seed that was used for each game.  If you'd like to repeat any individual game, you can rerun that game with its particular seed.  For example, suppose that you see that your agent did poorly in the game with seed 12.  You can rerun that game like this:

```
$ ./mspac MyAgent -seed 12
```


## Other notes

The controllers.pacman.examples package contains a set of sample Ms. Pac-Man agents.  (The controllers.ghosts.examples package contains additional agents that control ghosts.)

When running an agent, press the 'H' key to "hijack" control and manually navigate Ms. Pac-Man.
