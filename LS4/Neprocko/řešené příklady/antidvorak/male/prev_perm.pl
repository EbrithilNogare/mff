%spocita predchozi permutaci

splitPerm([X, Y | Xs], [X | Zacatek], Xi, Zbytek) :- 
    X > Y,
    splitPerm([Y | Xs], Zacatek, Xi, Zbytek).

splitPerm([X, Y | Xs], [X], Y, Xs) :- X < Y.

copyZbytek([], _, _, []).
copyZbytek([X | Xs], Zacatek, Y, [X | Zbytek]) :- copyZbytek(Xs, Zacatek, Y, Zbytek).

prohodMensi(Xi, [X | Xs], X, [Xi | NovyZacatek]) :-
    X < Xi,
    copyZbytek(Xs, Xi, X, NovyZacatek).

prohodMensi(Xi, [X | Xs], Yi, [X | NovyZacatek]) :-
    X > Xi,
    prohodMensi(Xi, Xs, Yi, NovyZacatek).

prev_perm(Perm, PrevPerm) :-
    reverse(Perm, PermR),
    splitPerm(PermR, Zacatek, Xi, Zbytek),
    prohodMensi(Xi,Zacatek, Yi, NovyZacatek),
    reverse(Zbytek,ZbytekR),
    append(ZbytekR, [Yi | NovyZacatek], PrevPerm).