?- consult('../functional.pl').

vstup(V) :-
    V = [
        [e, a, b], %1
        [a, b, e], %3
        [b, e, a]  %3 
        ].

vstup2(V) :-
    V = [
       [e, a, b, c], %1
       [a, b, e, c], %4
       [b, c, a, e], %3
       [c, e, c, b]  %3
       ].

vstup3(V) :-
   V = [
      [e, a, b, c], %1
      [a, b, e, c], %4
      [b, c, a, b], %3
      [c, e, c, e]  %2 
      ].

%http://forum.matfyz.info/viewtopic.php?f=169&t=11043&p=40642&hilit=grupa#p40642

transform(V, H-Z) :-
    V = [H | Y],
    transpose(Y,YT),
    YT = [_ | TT],
    transpose(TT, Z).

op(_, A, e, A).
op(_, e, A, A).
op(H-Z, A, B, C) :-
    findIndex(unify(A), H, I),
    findIndex(unify(B), H, J),
    nth1(I, Z, R),
    nth1(J, R, C).

rad(_, e, 1) :- !.
rad(V, A, R) :- transform(V, H-Z), rad(H-Z, 1, A, A, R).

rad(_, N, e, _, N).
rad(H-Z, N, A, B, R) :-
    op(H-Z, A, B, C),
    M is N+1,
    rad(H-Z, M, C, B, R), !.