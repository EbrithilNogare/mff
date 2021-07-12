?- consult('../functional.pl').

%klasika - problem batohu

cmpare(B, Acc) :-
    map(snd, B, C),
    sum(C, S),
    map(snd, Acc, A),
    sum(A, S2),
    S >= S2.

batoh(W, Xs, Ys) :- 
    vsechnyBatohy(W, Xs, [], Yss),
    maximumBy(cmpare, Yss, Ys).

batoh(_, [], Acc, Acc).

batoh(W, [V-C | Xs], Acc, Out) :-
    T is W - V,
    T >= 0, !,
    ( batoh(T, Xs, [V-C|Acc], Out) ; batoh(W, Xs, Acc, Out) ).

batoh(W, [_ | Xs], Acc, Out) :- batoh(W, Xs, Acc, Out).

nanelezenyBatoh(W, Xs, Acc, Ys) :-
    batoh(W, Xs, [], Ys),
    \+ member(Ys, Acc).

vsechnyBatohy(W, Xs, Acc, Out) :-
    nanelezenyBatoh(W, Xs, Acc, Ys), !,
    vsechnyBatohy(W, Xs, [Ys|Acc], Out).

vsechnyBatohy(_, _, Acc, Acc).
