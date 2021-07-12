?- consult('../functional.pl').
?- consult('../kombinace.pl').

vstup([a->b, a->c, b->d, e->f]).

%http://forum.matfyz.info/viewtopic.php?f=169&t=11954

relace_zadano(V, X,Y) :- member(X->Y, V).

relace(V, X,X) :- promenna(V,X).
relace(V, X,Y) :- X =\= Y, relace_zadano(V, X,Y).
relace(V, X,Y) :- 
    \+ X == Y, 
    relace_zadano(V,X,Z),
    relace(V,Z,Y).

porov(V, X,Y) :- ( relace(V, X,Y) ; relace(V, Y,X) ).

p(A->B, Acc, [A, B | Acc]).

promenne(V, X) :-
    foldl(p,V,[], Y),
    unique(Y, X).

promenna(V, X) :-
    promenne(V,Y),
    member(X,Y).

c(V, [X, Y], Acc, [X-Y | Acc]) :- \+ porov(V, X, Y), !.
c(_, _, Acc, Acc).

nepor(V, N) :-
    promenne(V, Vs),
    combinations(2, Vs, Cs),
    foldl(c(V),Cs, [], N).
