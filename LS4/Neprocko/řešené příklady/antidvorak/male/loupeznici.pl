?- consult('../functional.pl').

%klasika, loupeznici

loupeznici(Xs, A, B) :- loupeznici(Xs, [], [], A, B).

loupeznici([], P, Q, P, Q) :-
    sum(P, S),
    sum(Q, S).

loupeznici([X | Xs], A, B, P, Q) :-
    AA = [X | A],
    loupeznici(Xs, AA,B,P,Q).

loupeznici([X | Xs], A, B, P, Q) :-
    BB = [X | B],
    loupeznici(Xs, A,BB,P,Q).