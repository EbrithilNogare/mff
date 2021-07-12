?- consult('../functional.pl').

%http://forum.matfyz.info/viewtopic.php?f=169&t=8308&p=34050&hilit=paretoMax#p34050

ma_mensi_slozku(Y, X) :-
    member(S, Y),
    member(T, X),
    S < T.

dominuje(Y, X) :- \+ ma_mensi_slozku(Y, X).

%je dominovan X lib. prvkem z Ys
je_dominovan(X, Ys) :-
    member(Y, Ys),
    dominuje(Y, X).

p(Xs, X, Acc, [X | Acc]) :-
    once(select(X,Xs,Z)),
    \+ je_dominovan(X, Z), !.

p(_, _, Acc, Acc).

paretoMax(Xs, Ys) :- foldl(p(Xs), Xs, [], Ys).