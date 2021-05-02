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
	% labeling([ff], V), % doesnt work, todo get only one solution for sudoku, with multiple solutions
	forEach(all_distinct, Rows),
	transpose(Rows, Columns),
	forEach(all_distinct, Columns),
	squares(A, B, C),
	squares(D, E, F),
	squares(G, H, I).

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
 * printSudoku(Sudoku : list)
 * 
 * Print 2D list, line by line.
 *
 * @param Sudoku Sudoku to print.
 */
printSudoku([]).
printSudoku([Row | Rest]) :-
	print(Row),
	write("\n"),
	printSudoku(Rest).


sudokuGenerate :- 
	Sudoku = [
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
	sudoku(Sudoku),
	printSudoku(Sudoku).

% sudokuShuffle(Sudoku, Count) :-
%	length(Sudoku, 9)
% 	random(1, 3, RowBlock),
% 	random(1, 3, Row).
	


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
	printSudoku(Sudoku).

sudoku2 :- 
	Sudoku = [
		[_,2,_,4],
		[_,_,1,_],
		[2,_,4,_],
		[_,3,2,_]],
	sudoku(Sudoku),
	printSudoku(Sudoku).

help :-
	write("""
Sudoku = [
	[_,_,_,_],
	[_,_,_,_],
	[_,_,_,_],
	[_,_,_,_]
], sudoku(Sudoku), printSudoku(Sudoku).		
""").


% problem(1, Rows), sudoku(Rows), printSudoku(Rows).