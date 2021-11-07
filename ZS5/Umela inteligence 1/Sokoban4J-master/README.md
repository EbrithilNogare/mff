# Sokoban4J

![alt tag](https://github.com/kefik/Sokoban4J/raw/master/Sokoban4J/screenshot.png)

## Overview

This is an implementation of the puzzle game [Sokoban](https://en.wikipedia.org/wiki/Sokoban) in Java (using Swing).  It is fully playable from the keyboard, but truly intended for programmers who want to develop Sokoban-playing agents in Java.

Sokoban4J supports loading level files in the .sok format.  The levels folder in this repository contains many .sok packages downloaded from [sokobano.de](http://sokobano.de/en/levels.php). Thank you for such a wonderful collection!

This project includes art created by 1001.com, downloaded from [OpenGameArt](http://opengameart.org/content/sokoban-pack) and used under [CC-BY-SA-4.0](https://creativecommons.org/licenses/by-sa/4.0/legalcode); thank you!

Sokoban4J was originally written by Jakub Gemrot of the Faculty of Mathematics and Physics at Charles University.  [Adam Dingle](https://ksvi.mff.cuni.cz/~dingle/) has continued its development.  It is licensed under [CC-BY-SA-4.0](https://creativecommons.org/licenses/by-sa/4.0/legalcode). Please retain a URL to the original [Sokoban4J repository](https://github.com/kefik/Sokoban4J) in your work.

## Building the game

This version of Sokoban4J works with Java 11 or higher, and possibly older Java versions as well.  It includes a Maven project file, and you should easily be able to open it in any Java IDE such as Eclipse, IntelliJ, or Visual Studio Code.

## Playing the game

To play the game from the keyboard on Linux or macOS, run

```
$ ./sokoban
```

Or, on Windows:

```
> .\sokoban
```
By default, the game plays a set of 10 relatively easy levels, found in `levels/easy.sok`. 

Use the arrow keys or the W/S/A/D keys to move.  Press Z to undo your last move.

Use the -levelset option to choose a different level set from the levels/ directory, e.g.

```
$ ./sokoban -levelset Aymeric_Medium
```
 
Specify the -level option to play only a particular level within a level set:

```
$ ./sokoban -levelset Aymeric_Medium -level 4
```

## Writing an agent

You can write an agent using the [Sokoban API](https://ksvi.mff.cuni.cz/~dingle/2020-1/ai_1/sokoban/sokoban_api.html).

Use src/MyAgent.java as a starting point.  This class contains a simple depth-first search implementation that you can delete and replace with your own solver.

To run your agent on the default level set easy.sok, type

```
$ ./sokoban MyAgent
```

You can also run it on any other level set from the levels/ directory using the -levelset option mentioned above.

Add the -v (verbose) option to get more detailed output.

Some of the level sets in levels/ include an optimal move count for each level.  If you include the -optimal argument, Sokoban4J will only accept solutions that are optimal, i.e. whose length matches these known values.  This is a good way to test that your solver generates move-optimal solutions (if it is in fact designed to do that).

Run './sokoban -help' to find out about other available options.

There are several simple agents in the src/agents directory.  You may wish to look at them, though they are all quite similar to the sample MyAgent.

## Notes

Here are some more detailed notes:

1. The code base includes four different game state representations.  The Board class is an object-oriented representation used by the simulator.  For state space searching, use BoardCompact, BoardSlim or BoardCompressed. Use StateCompressed or StateMinimal for representing of no-good states.

1. A level may include up to 6 different kinds of boxes (yellow, blue, gray, purple, red and black) and targets for specific box types. A brown target is a generic spot for any kind of box.

1. Large levels automatically scale down to fit the screen in order to be playable by humans.

1. Level play time may be limited (in milliseconds).

1. You may find introductory tips for creating a Sokoban artificial player in this [report](http://pavel.klavik.cz/projekty/solver/solver.pdf) (courtesy of Pavel Klavík).

------------------------------------------------------------

![alt tag](https://github.com/kefik/Sokoban4J/raw/master/Sokoban4J/screenshot2.png)

![alt tag](https://github.com/kefik/Sokoban4J/raw/master/Sokoban4J/screenshot3.png)

------------------------------------------------------------
