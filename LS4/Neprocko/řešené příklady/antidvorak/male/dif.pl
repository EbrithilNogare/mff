%http://forum.matfyz.info/viewtopic.php?f=169&t=11466&p=41155&hilit=dif#p41155

diff(X, X).
diff(X, D) :- select(_,X,D).
diff(X, D) :- select(_,X,Z), select(_,Z,D).

availablediff(X, Acc, A) :-
    diff(X,A),
    \+ member(A, Acc).

dif(X, D, D) :- \+ availablediff(X, D, _), !.
dif(X, Acc, D) :-
    availablediff(X, Acc, A),
    !,
    dif(X, [A | Acc], D).

dif(X, D) :- dif(X, [], D).