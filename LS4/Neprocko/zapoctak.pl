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
	squares(C, D).

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

prettyPrintRow([]).
prettyPrintRow([Column | Rest]) :-
	write("| "),
	prettyPrintNotNull(Column),
	write(" "),
	prettyPrintRow(Rest),
	!.

prettyPrintNotNull("Null") :-
	write(" ").

prettyPrintNotNull(String) :-
	print(String).

generateSudoku9x9(Difficulty) :- 
	Difficulty > 0,
	Difficulty < 51,
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

generateSudoku4x4(Difficulty) :- 
	Difficulty > 0,
	Difficulty < 11,
	SudokuIn = [
		[1,2,3,4],
		[3,4,1,2],
		[2,1,4,3],
		[4,3,2,1]
	],
	sudokuShuffle(SudokuIn, 3, SudokuShuffled),
	sudokuHardener(SudokuShuffled, Difficulty, SudokuOut),
	prettyPrintSudoku(SudokuOut),
	!.

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
		sudokuHardener(SudokuAlternated, NewCount, SudokuOut)
	).

conditionalyRemoveItem(Column, Item, New_item, Index) :-
	( Index = Column ->
		New_item = _
	;
		New_item = Item
	).

conditionalyRemoveItemFromList(Row, Column, List, New_list, Index) :-
	( length(List, 9) ->
		IndicesList = [1,2,3,4,5,6,7,8,9]
	;
		IndicesList = [1,2,3,4]
	),
	( Index = Row ->
		maplist(conditionalyRemoveItem(Column), List, New_list, IndicesList)
	;
		New_list = List
	).

conditionalyRemoveItemFromArray( Row, Column, Array, New_array) :-
	( length(Array, 9) ->
		IndicesList = [1,2,3,4,5,6,7,8,9]
	;
		IndicesList = [1,2,3,4]
	),
    maplist(conditionalyRemoveItemFromList(Row, Column), Array, New_array, IndicesList).


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


help :-
	write("""
Sudoku = [
	[_,_,_,_],
	[_,_,_,_],
	[_,_,_,_],
	[_,_,_,_]
], sudoku(Sudoku), prettyPrintSudoku(Sudoku).		
""").


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

sudoku1 :- 
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

sudoku2 :- 
	Sudoku = [
		[_,2,_,4],
		[_,_,1,_],
		[2,_,4,_],
		[_,3,2,_]],
	sudoku(Sudoku),
	prettyPrintSudoku(Sudoku).

sudoku4x4_0 :- 
	Sudoku = [
		[_,_,_,_],
		[_,_,_,_],
		[_,_,_,_],
		[_,_,_,_]],
	sudoku(Sudoku),
	prettyPrintSudoku(Sudoku).

sudoku3 :- 
	Sudoku = [
		[_,_,_,_,_,_,_,_,_],
		[_,_,_,_,_,3,_,_,5],
		[_,_,1,_,2,_,_,_,_],
		[_,_,_,5,_,7,_,_,_],
		[_,_,4,_,_,_,1,_,_],
		[_,9,_,_,_,_,_,_,_],
		[5,_,_,_,_,_,_,7,3],
		[_,_,2,_,1,_,_,_,_],
		[_,_,_,_,4,_,_,_,9]],
	sudoku(Sudoku),
	prettyPrintSudoku(Sudoku).


% problem(1, Rows), sudoku(Rows), prettyPrintSudoku(Rows).