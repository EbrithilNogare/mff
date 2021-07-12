?- consult('functional.pl').

%mnozina vsech mnozin

powerset([], [[]]).
powerset([X | Xs], Ps) :-
    powerset(Xs, Ps1), 
    map(pushFront(X), Ps1, Ps2),
    append(Ps1, Ps2, Ps).

subset(S, P) :- powerset(P, PS), member(S, PS).
