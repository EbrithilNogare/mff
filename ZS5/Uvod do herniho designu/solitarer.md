# Multiplayer Solitare

## Game dev

### First ideas
Classic solitare, but instead one player threre will be two players, competing agains each other.

### Implementation details
Each player has two decks and they are collectiong them.
Who collect both deck is winner.

### After game testing no. 1
Player need two folowed moves insted just one.
There must be some automatic moves as reveal hidden card on top of column or on top of deck if there is none revealed.

### After implementing previous and game testing no. 2
Younger player starts.
Players will collect black (or red) decks based on color of first card.

### After implementing previous and game testing no. 3
Game starts with one revealed card from deck.

### Second ideas
Every card can be put on every other bigger card (5 can go to 7).
Colors doesnt matter (black can go on black).
Not a younger player, but player with longest mustache begins.

### After implementing previous and game testing no. 4
Player cannot move card from any players deck.
Set of cards can be moved together as long as all of them are direct successors (only 4,5,6,7 can move, but 4,6,7 not).
Only one pass deck.

### After school testing
Empty position is infinity, so every card can be there.
Player must do two moves (cant do just one or none).
Player can drag card from revealed pile to his deck if symbol matches and is direct successor.
Ace is meant as one and king cant be used as dirrect successor.
To maintain number of free positions, columns are
diversificated, so it is easier to tell where and how many empty row here there.

### After second school testing
The game is finally balanced, but involving strategy isnt as intuitivve as should be, which is not that bad.
Luck is big part of game but good strategy can move winning chances to one side completely.
The game is very slow at beginning but very very, fast at the end.
In the game there is not easy way to get into the state from which game couldnt be played and win by any player.

## Final rules
Rules are same as for solitare Klondike and below are exceptions.

Two players compete to collect two sorted piles of same color before enemy.	

### Before game
- turn the first card from a deck
- if the card is red, first player will be collecting red cards (similarly for black card)
- the player with the longest mustache starts

### Turn
Every player has **2 moves** in his turn.

The move is one of:
- move one card from revealed deck to bigger (unlimitedly bigger) card on field or to the collectible pile
- flip new card from deck to revealed deck (with only **one pass through deck**)
- move one top card to his collectible pile (must be direct successor), with respect to color
- move one or more (only if they are direct successors) cards to the card which is bigger (**unlimitedly bigger**) than biggest card from moved cards, with **no respect to colors**

### Winner
The first one who has whole two piles is winner

### Game details
Length: 10-15 minutes  
Players: 2  
Age: 8+
