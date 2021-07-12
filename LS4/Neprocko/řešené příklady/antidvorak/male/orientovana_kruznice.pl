vstup(I) :- I = [a->[c], b->[a], c->[b,d,e], d->[], e->[b]].

%mejme orientovany graf, naleznete vsechny orientovane cykly

vrchol(V) :- 
    vstup(I),
    member(V->_,I).

hrana(E,F) :-
    vstup(I),
    vrchol(F),
    member(E->N, I),
    member(F, N).

%nalezne vsechny cykly v orientovanem grafu
cyklus(X, Path) :-
    cyklus(X, X, [X], PathR),
    reverse(PathR, Path).

cyklus(X, X, [X], Path) :-
    hrana(X,Z),
    Z \= X,
    cyklus(X, Z, [Z,X], Path).

cyklus(X, Y, Visited, [X | Visited]) :-
    Y \= X,
    hrana(Y,X).

cyklus(X, Y, Visited, Path) :-
    Y \= X,
    hrana(Y,Z),
    \+ member(Z, Visited),
    cyklus(X,Z, [Z | Visited], Path).

