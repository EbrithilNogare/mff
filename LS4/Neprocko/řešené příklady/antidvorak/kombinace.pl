?- consult('functional.pl').

%kombinace se mi hodili furt

combinations(0,_,[[]]) :- !.
combinations(_, [], []) :- !.
combinations(N, [X | Xs], Ks) :-
    N > 0,
    combinations(N, Xs, K1s),
    M is N - 1,
    combinations(M, Xs, K2s),
    map(pushFront(X), K2s, K3s),
    append(K1s, K3s, Ks).

combination(N, Xs, Ys) :-
    combinations(N, Xs, Z),
    member(Ys, Z).