# Minesweeper4J
![alt tag](https://github.com/kefik/Minesweeper4J/raw/master/Minesweeper4J/Minesweeper-1.png)
![alt tag](https://github.com/kefik/Minesweeper4J/raw/master/Minesweeper4J/Minesweeper-2.png)

Minesweeper4J is an implementation of the classic Minesweeper game in Java. It is playable from the keyboard, but truly intended for writing programs that solve Minesweeper automatically.

Minesweeper4J was originally written by my collegue Jakub Gemrot.  I've made various changes and improvements.

This program is **licensed under** [CC-BY-3.0](https://creativecommons.org/licenses/by/3.0/). If you modify or distribute it, please retain a URL to the original [Minesweeper4J site](https://github.com/kefik/Minesweeper4J) in your work.

## Quick start

To play the game from the keyboard on Linux or macOS, run

```
$ ./minesweeper
```

Or, on Windows:

```
> .\minesweeper
```

When the game begins, all squares are hidden, and one square will appear with a thin white border.  This is a hint indicating that the square does not contain a mine.  Click it to begin the game (or, if you are brave, click somewhere else!)

Click any square with the left mouse button to open it, or with the right mouse button to flag it as a mine.  Press 'S' to ask for a hint.

By default, the board is 9 x 9 and contains 10 mines.  For a larger board, specify -medium for a 16 x 16 board with 40 mines, or -hard for a 30 x 16 board with 99 mines.

Alternatively, the -size option specifies a custom board size.  Use -size N to select an N x N board, or -size W H to choose a board with width W and height H.  By default, any custom board will have a mine density of 0.2, i.e. 1 in 5 squares will contain a mine.  Use the -density option to change this value.

More options are available; type './minesweeper -help' to see them all.

## Writing an agent

You can write an agent using the [Minesweeper API](https://ksvi.mff.cuni.cz/~dingle/2020-1/ai_1/minesweeper/minesweeper_api.html).

You may wish to use src/MyAgent.java as a starting point.  This is a dummy agent that asks for a hint on every move.

To watch your agent attempt to solve a default 9 x 9 board, run

```
$ ./minesweeper MyAgent
```

To run your agent on a series of randomly generated boards without visualization, use the -sim option.  For example, to solve 10 boards of size 20 x 20:

```
$ ./minesweeper MyAgent -size 20 -sim 10
```

For each board, the output will indicate how long your solver took and how many hints it requested.

## CREDITS

Minesweeper4J uses a [tileset](https://opengameart.org/content/minesweeper-tile-set) created by [Eugene Loza](https://opengameart.org/users/eugeneloza).  Thank you!
