% a) struktura grafu
% graf je list hran

vrstvy([], []).
	
vrstvy(List, [Head | Tail]) :-
	removeIfZeroDegree(List, Z, Head, Childs),
	ReduceDegree(Z, Childs, Z2),
	vrstvy(Z2, Tail).
	
vrstvy_go(V ,H ,Tail):-
	numberOfChilds(V ,H ,Tail1),
	vrstvy(Tail1, Tail).

% c) protoze mu jeho pristusnost k vrstve postupne snizujeme a je zaruceno, ze pak uz nizsi byt nemuze 


numberOfChilds([], _, []).

numberOfChilds([X | []], Edges, v(X,N,Z)) :-
	degree(X, Edges, N),
	childs(X, Edges, Z).

numberOfChilds([X|Xs], Edges, [v(X,N,Z)|Vr]):-
	degree(X, Edges, N),
	childs(X, Edges, Z),
	numberOfChilds(Xs, Edges, Vr).



sortByNumber([], Z, Z). 

sortByNumber([X|Xs], Z, Tail) :-
	insertByNumber(X, Z, Tail1),
	sortByNumber(Xs, Tail1, Tail).


insertByNumber(v(X,Xn), [], [v(X,Xn)]).

insertByNumber(v(X,Xn), [v(Y,Yn)|Ys], [v(X,Xn),v(Y,Yn)|Ys]) :-
	Xn =< Yn.

insertByNumber(v(X,Xn), [v(Y,Yn)|Ys], [v(Y,Yn)|Tail]) :-
	insertByNumber(v(X,Xn), Ys, Tail).




ReduceDegree([], _, []).
		
ReduceDegree(v(X, N, Z), List, v(X, N1, Z)) :-
	ReduceByNumber(X, List, Poc),
	N1 is N - Poc.

ReduceDegree([v(X, N, Z)|Ys], List, [v(X, N1, Z)|Tail]) :-
	ReduceByNumber(X, List, Poc),
	N1 is N - Poc,
	ReduceDegree(Ys, List, Tail).


ReduceByNumber(_, [], 0).

ReduceByNumber(X, [X|Ys], Poc) :-
	ReduceByNumber(X,Ys,Poc1),
	Poc is Poc1 + 1.

ReduceByNumber(X, [_|Ys], Poc) :-
	ReduceByNumber(X,Ys,Poc).




removeIfZeroDegree([], [], [], []).

removeIfZeroDegree(v(X,0,Childs), [], X, Childs).

removeIfZeroDegree(v(X,Y,Z), v(X,Y,Z), [], []).
	
removeIfZeroDegree([v(X,0,Childs)|Xs], Z, [X|Tail], Tail2) :-
	removeIfZeroDegree(Xs, Z, Tail, ChildsTail),
	append(Childs, ChildsTail, Tail2).
	
removeIfZeroDegree([v(X,N,Childs)|Xs], [v(X,N,Childs)|Z], Tail, ChildsTail) :-
	removeIfZeroDegree(Xs, Z, Tail, ChildsTail).


test(Rs) :-
    vrstvy([[a,b],[b,c],[b,e],[d,b],[d,e]],Rs).
