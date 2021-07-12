?- consult('functional.pl').

%mnozinoviny

elementToAdd(Xs, Y, Acc, [Y | Acc]) :-
    member(Y, Xs),
    \+ member(Y, Acc), !.

elementToAdd(_, _, Acc, Acc).
intersection(Xs, Ys, Zs) :- foldr(elementToAdd(Xs),Ys,[], Zs).

union(Xs, Ys, Zs) :- setAppend(Xs,Ys,Zs).


isIn(Ys, X, Acc, [X | Acc]) :-
    \+ member(X, Ys),
    \+ member(X, Acc), !.

isIn(_, _, Acc, Acc).

difference(Xs, Ys, Zs) :- foldr(isIn(Ys), Xs, [], Zs).

% Ys^C vuci Xs
complement(Xs, Ys, C) :- difference(Xs, Ys, C).

setPushFront(X, Xs, Xs) :- member(X, Xs), !.
setPushFront(X, Xs, [X | Xs]).

setPushBack(X, Xs, Xs) :- member(X, Xs), !.
setPushBack(X, Xs, Ys) :- append(Xs, [X], Ys).

setAppend(Xs, Ys, Zs) :-
    append(Xs, Ys, Z),
    unique(Z, Zs).

symmetricDifference(Xs, Ys, Zs) :-
    difference(Xs, Ys, A),
    difference(Ys, Xs, B),
    union(A, B, Zs).

k2(X, Y, Acc, [X-Y | Acc]).
k1(Ys, X, Acc, Ret) :- foldr(k2(X), Ys, [], Out), append(Acc, Out,Ret).
cartesian_product(Xs, Ys, Zs) :- foldl(k1(Ys), Xs, [], Zs).