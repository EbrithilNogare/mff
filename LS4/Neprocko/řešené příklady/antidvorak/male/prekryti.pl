?- consult('../functional.pl').

%http://forum.matfyz.info/viewtopic.php?f=169&t=11954

konfigurace(X,X).
konfigurace(X, Y) :- 
    reverse(X, Y), 
    Y \= X.

prekryti(Xs, Ys, Out) :-
    konfigurace(Xs, X),
    konfigurace(Ys, Y),
    Y = [H | _],
    length(Y,L),
    findIndex(equal(H), X, Idx),
    drop(Idx, X, XTail),
    take(L, XTail, XTrimmed),
    length(XTrimmed, L2),
    take(L2, Y, YTrimmed),
    XTrimmed = YTrimmed,
    take(Idx, X, Out1),
    append(Out1, Y, Out).

vsechna_prekryti(Xs, Ys, Acc, Out) :-
    prekryti(Xs,Ys,A),
    \+ member(A, Acc),
    vsechna_prekryti(Xs, Ys, [A | Acc], Out),
    !.

vsechna_prekryti(_, _, Out, Out) :- !.
