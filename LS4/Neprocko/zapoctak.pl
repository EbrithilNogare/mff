:- use_module(library(clpfd)).
 
/**
 * sudoku(Sudoku : list)
 * 
 * Find solution for sudoku 9x9 or 4x4.
 *
 * @param Sudoku Sudoku to solve.
 */
sudoku(Rows) :-
	length2D(Rows, 9, 9),
	Rows = [A,B,C,D,E,F,G,H,I],
	append(Rows, V), V ins 1..9,
	forEach(all_distinct, Rows),
	transpose(Rows, Columns),
	forEach(all_distinct, Columns),
	squares(A, B, C),
	squares(D, E, F),
	squares(G, H, I),
	labeling([ff], V).

sudoku(Rows) :-
	length2D(Rows, 4, 4),
	Rows = [A,B,C,D],
	append(Rows, V), V ins 1..4,
	forEach(all_distinct, Rows),
	transpose(Rows, Columns),
	forEach(all_distinct, Columns),
	squares(A, B),
	squares(C, D),
	labeling([ff], V).

/**
 * squares(ListA : list, ListB : list, ?ListC : list)
 * 
 * Check if squares are correct.
 *
 * ListA = [A1 ... AN | ARest]
 * ListB = [B1 ... BN | BRest]
 * ListC = [C1 ... CN | CRest]
 * 
 * @param ListA 1st Row.
 * @param ListB 2nd Row.
 * @param ListC 3rd Row.
 */
squares([], []).
squares([A1, A2 | ARest], [B1, B2 | BRest]) :-
	all_distinct([A1, A2 ,B1, B2]),
	squares(ARest, BRest).

squares([], [], []).
squares([A1, A2, A3 | ARest], [B1, B2, B3 | BRest], [C1, C2, C3 | CRest]) :-
	all_distinct([A1, A2 ,A3 ,B1, B2, B3, C1, C2, C3]),
	squares(ARest, BRest, CRest).

/**
 * length2D(2DList : list, Width : int, Height : int)
 * 
 * Get width and height of 2D List.
 * False if width isnt same for all rows.
 *
 * @param 2DList List to be measured.
 * @param Width Width of 2D List. 
 * @param Height Height of 2D List.
 */
length2D(Rows, Width, Height) :-
	length(Rows, Height),
	forEach(lengthOf(Width), Rows).

/**
 * lengthOf(Length : int, List : list)
 * 
 * Enable using length with forEach function.
 *
 * @param Length Length of list. 
 * @param List List to be measured.
 */
lengthOf(Length, List) :-
	length(List, Length).

/**
 * forEach(Function : callable, List : list)
 * 
 * Runs function to all items of list.
 *
 * @param Function Function to be called with params. 
 * @param List List of items to be used.
 */
forEach(_, []).
forEach(G, [X | Rest]) :-
	call(G, X),
	forEach(G, Rest).

/**
 * prettyPrintSudoku(Sudoku : list)
 * 
 * Print 2D list, line by line.
 *
 * @param Sudoku Sudoku to print.
 */
prettyPrintSudoku([]) :-
	write("+---+---+---+---+---+---+---+---+---+\n").
prettyPrintSudoku([Row | Rest]) :-
	write("+---+---+---+---+---+---+---+---+---+\n"),
	prettyPrintRow(Row),
	write("|\n"),
	prettyPrintSudoku(Rest).

/**
 * prettyPrintRow(List : list)
 * 
 * Print list.
 *
 * @param List List to be printed.
 */
prettyPrintRow([]).
prettyPrintRow([Column | Rest]) :-
	write("| "),
	prettyPrintNotNull(Column),
	write(" "),
	prettyPrintRow(Rest),
	!.

/**
 * prettyPrintNotNull(_ : _)
 * 
 * Print value, if it is anonymou variable, print space.
 *
 * @param _ Variable to print.
 */
prettyPrintNotNull("Null") :-
	write(" ").

prettyPrintNotNull(String) :-
	print(String).

/**
 * generateSudoku9x9(Difficulty : int)
 * 
 * Generate sudoku with one solution and pretty print it..
 *
 * @param Difficulty Difficulty from 1 to 20.
 */
generateSudoku9x9(Difficulty) :- 
	Difficulty < 1; Difficulty > 20,
	write("difficulty must be between 1 and 20").	
generateSudoku9x9(Difficulty) :- 
	Difficulty > 0,
	Difficulty < 21,
	SudokuIn = [
		[1,2,3,4,5,6,7,8,9],
		[7,8,9,1,2,3,4,5,6],
		[4,5,6,7,8,9,1,2,3],
		[3,1,2,8,4,5,9,6,7],
		[9,6,7,3,1,2,8,4,5],
		[8,4,5,9,6,7,3,1,2],
		[2,3,1,5,9,4,6,7,8],
		[6,7,8,2,3,1,5,9,4],
		[5,9,4,6,7,8,2,3,1]
	],
	sudokuShuffle(SudokuIn, 30, SudokuShuffled),
	sudokuHardener(SudokuShuffled, Difficulty, SudokuOut),
	prettyPrintSudoku(SudokuOut),
	!.

/**
 * generateSudoku4x4(Difficulty : int)
 * 
 * Generate sudoku with one solution and pretty print it..
 *
 * @param Difficulty Difficulty from 1 to 5.
 */
generateSudoku4x4(Difficulty) :- 
	Difficulty < 1; Difficulty > 5,
	write("difficulty must be between 1 and 5").	
generateSudoku4x4(Difficulty) :- 
	Difficulty > 0,
	Difficulty < 6,
	SudokuIn = [
		[1,2,3,4],
		[3,4,1,2],
		[2,1,4,3],
		[4,3,2,1]
	],
	sudokuShuffle(SudokuIn, 10, SudokuShuffled),
	sudokuHardener(SudokuShuffled, Difficulty, SudokuOut),
	prettyPrintSudoku(SudokuOut),
	!.

/**
 * sudokuHardener(SudokuIn : list, Count : int, SudokuOut: list)
 * 
 * Remove random field from sudoku until it have one solution and Counter is on 0.
 *
 * @param SudokuIn Sudoku to alternate.
 * @param Count Number of tries to remove another numbers.
 * @param SudokuOut Resulted sudoku.
 */
sudokuHardener(Sudoku, 0, Sudoku).
sudokuHardener(SudokuIn, Count, SudokuOut) :-
	length(SudokuIn, Length),
	random_between(1, Length, Row),
	random_between(1, Length, Column),
	conditionalyRemoveItemFromArray(Row, Column, SudokuIn, SudokuAlternated),
	aggregate_all(count, sudoku(SudokuAlternated), Solutions),
	( Solutions = 1 ->
		sudokuHardener(SudokuAlternated, Count, SudokuOut)
	;
		NewCount is Count - 1,
		sudokuHardener(SudokuIn, NewCount, SudokuOut)
	).

/**
 * conditionalyRemoveItem(Column : int, Item : var, NewItem : var, Index : int)
 * 
 * If Column equals Index, remove Item from List
 *
 * @param Column Column index to compare
 * @param Item Old Item.
 * @param NewItem NewItem.
 * @param Index Actual Index of List.
 */
conditionalyRemoveItem(Column, Item, NewItem, Index) :-
	( Index = Column ->
		NewItem = _
	;
		NewItem = Item
	).

/**
 * conditionalyRemoveItem(Row : int, Column : int, List : list, NewList : list, Index : list)
 * 
 * If Row equals Index, remove List in List on Column.
 *
 * @param Row Row index to compare.
 * @param Column Column index to compare.
 * @param List Old List to alternate.
 * @param NewList Alternated List.
 * @param Index Actual Index of List.
 */
conditionalyRemoveItemFromList(Row, Column, List, NewList, Index) :-
	( length(List, 9) ->
		IndicesList = [1,2,3,4,5,6,7,8,9]
	;
		IndicesList = [1,2,3,4]
	),
	( Index = Row ->
		maplist(conditionalyRemoveItem(Column), List, NewList, IndicesList)
	;
		NewList = List
	).

/**
 * conditionalyRemoveItem(Row : int, Column : int, Array : list, NewArray : list)
 * 
 * If Row equals Index, remove Array in Array on Column.
 *
 * @param Row Row index to compare.
 * @param Column Column index to compare.
 * @param Array Array to alternate.
 * @param NewArray Alternated Array.
 */
conditionalyRemoveItemFromArray( Row, Column, Array, NewArray) :-
	( length(Array, 9) ->
		IndicesList = [1,2,3,4,5,6,7,8,9]
	;
		IndicesList = [1,2,3,4]
	),
    maplist(conditionalyRemoveItemFromList(Row, Column), Array, NewArray, IndicesList).


/**
 * sudokuShuffle(Sudoku : List, Count : int, Output : List)
 * 
 * Swap two rows then swap two columns and repeat until Count is not zero.
 *
 * @param Sudoku Sudoku to shuffle.
 * @param Count Desired number of swaps .
 * @param Output Alternated sudoku.
 */
sudokuShuffle(Sudoku, 0, Sudoku).
sudokuShuffle(Sudoku, Count, Output) :-
	Count > 0,
	length(Sudoku, 9),
	random_between(1, 9, Row),
	moveRow(Sudoku, Row, 3, RowMixed),
	transpose(RowMixed, RowMixedT),
	random_between(1, 9, Column),
	moveRow(RowMixedT, Column, 3, ColumnMixed),
	NewCount is Count-1,
	sudokuShuffle(ColumnMixed, NewCount, Output).

sudokuShuffle(Sudoku, Count, Output) :-
	Count > 0,
	length(Sudoku, 4),
	random_between(1, 4, Row),
	moveRow(Sudoku, Row, 2, RowMixed),
	transpose(RowMixed, RowMixedT),
	random_between(1, 4, Column),
	moveRow(RowMixedT, Column, 2, ColumnMixed),
	NewCount is Count-1,
	sudokuShuffle(ColumnMixed, NewCount, Output).

/**
 * moveRow(SudokuIn : list, From : int, BlockSize: int, SudokuOut : list)
 * 
 * Swap two lines in sudoku
 *
 * @param SudokuIn Sudoku to alternate.
 * @param From Row number.
 * @param BlockSize Size of square.
 * @param SudokuOut Alternated sudoku.
 */	
moveRow([A, B | Rest], 1, 3, [B, A | Rest]).
moveRow([A, B, C | Rest], 2, 3, [A, C, B | Rest]).
moveRow([A, B, C | Rest], 3, 3, [C, B, A | Rest]).
moveRow([A, B, C, D, E | Rest], 4, 3, [A, B, C, E, D | Rest]).
moveRow([A, B, C, D, E, F | Rest], 5, 3, [A, B, C, D, F, E | Rest]).
moveRow([A, B, C, D, E, F | Rest], 6, 3, [A, B, C, F, E, D | Rest]).
moveRow([A, B, C, D, E, F, G, H | Rest], 7, 3, [A, B, C, D, E, F, H, G | Rest]).
moveRow([A, B, C, D, E, F, G, H, I | Rest], 8, 3, [A, B, C, D, E, F, G, I, H | Rest]).
moveRow([A, B, C, D, E, F, G, H, I | Rest], 9, 3, [A, B, C, D, E, F, I, H, G | Rest]).
moveRow([A, B | Rest], 1, 2, [B, A | Rest]).
moveRow(In, 2, 2, Out) :- moveRow(In, 1, 2, Out).
moveRow([A, B, C, D | Rest], 3, 2, [A, B, D, C | Rest]).
moveRow(In, 4, 2, Out) :- moveRow(In, 3, 2, Out).

/**
 * help()
 * 
 * Just print help to user.
 */	
help :-
	write("""
This program can solve any 9x9 or 4x4 sudoku.

By this sequence, you can input your own sudoku and program will print solution(s).
Sudoku = [
	[_,_,_,_],
	[_,_,_,_],
	[_,_,_,_],
	[_,_,_,_]
], sudoku(Sudoku), prettyPrintSudoku(Sudoku).	

There are some examples to fast test program. 
sudoku0.
sudoku1.
sudoku2.
sudoku3.
sudoku4.

You can also generate sudoku for you to solve, with custom difficulty setting.
1 is easy and more higher number is, more difficult it genererate.
generateSudoku9x9(10).
generateSudoku4x4(3).
""").

% empty 9x9 sudoku
sudoku0 :- 
	Sudoku = [
		[_,_,_,_,_,_,_,_,_],
		[_,_,_,_,_,_,_,_,_],
		[_,_,_,_,_,_,_,_,_],
		[_,_,_,_,_,_,_,_,_],
		[_,_,_,_,_,_,_,_,_],
		[_,_,_,_,_,_,_,_,_],
		[_,_,_,_,_,_,_,_,_],
		[_,_,_,_,_,_,_,_,_],
		[_,_,_,_,_,_,_,_,_]],
	sudoku(Sudoku),
	prettyPrintSudoku(Sudoku).

% random 9x9 sudoku
sudoku1 :- 
	Sudoku = [
	[_,6,_,7,_,_,_,_,_],
	[_,3,_,_,_,_,_,8,_],
	[_,_,_,_,3,2,_,_,4],
	[6,7,_,_,_,_,5,_,_],
	[4,5,8,_,_,6,_,1,_],
	[_,_,_,_,_,_,_,_,9],
	[_,_,2,5,_,_,_,_,_],
	[_,_,_,_,1,_,_,9,_],
	[9,_,5,_,8,_,1,3,_]],
	sudoku(Sudoku),
	prettyPrintSudoku(Sudoku).

% random 4x4 sudoku
sudoku2 :- 
	Sudoku = [
		[_,2,_,4],
		[_,_,1,_],
		[2,_,4,_],
		[_,3,2,_]],
	sudoku(Sudoku),
	prettyPrintSudoku(Sudoku).

% anti bruteforce sudoku
sudoku3 :- 
	Sudoku = [
		[_,_,_,_,_,_,_,_,_],
		[_,_,_,_,_,3,_,8,5],
		[_,_,1,_,2,_,_,_,_],
		[_,_,_,5,_,7,_,_,_],
		[_,_,4,_,_,_,1,_,_],
		[_,9,_,_,_,_,_,_,_],
		[5,_,_,_,_,_,_,7,3],
		[_,_,2,_,1,_,_,_,_],
		[_,_,_,_,4,_,_,_,9]],
	sudoku(Sudoku),
	prettyPrintSudoku(Sudoku).

% sudoku with 2 solutions
sudoku4 :- 
	Sudoku = [
		[8,7,6,1,9,2,5,4,3],
		[2,9,5,7,4,3,8,6,1],
		[4,3,1,8,6,5,9,_,_],
		[1,5,4,9,3,8,6,_,_],
		[9,2,8,6,7,1,3,5,4],
		[7,6,3,5,2,4,1,8,9],
		[6,1,2,3,8,7,4,9,5],
		[5,4,9,2,1,6,7,3,8],
		[3,8,7,4,5,9,2,1,6]],
	sudoku(Sudoku),
	prettyPrintSudoku(Sudoku).
