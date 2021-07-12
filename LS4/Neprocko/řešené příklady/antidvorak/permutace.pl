?- consult('functional.pl').

%nekdy se hodi i permutace s opakovanim

permutation([], []).
permutation([H|T], S) :- 
    permutation(T, P), 
    append(X, Y, P), 
    append(X, [H|Y], S).

permutation_member(X, Y) :-
    permutation(X, P),
    member(P,Y).



p(C-Xss, Ys) :- map(q(C),Xss, Ys).
q(C, Xs, [C | Xs]).

permutations_repetiton(0, _, [[]]) :- !.
permutations_repetiton(_, [], []) :- !.
permutations_repetiton(N, Xs, Ys) :-
    M is N-1,
    permutations_repetiton(M, Xs, C),
    length(Xs, L),
    replicate(L, C, D),
    zip(Xs, D, E),
    map(p,E, F),
    concat(F,Ys).
    
permutation_repetiton(N, Xs, X) :-
    permutations_repetiton(N, Xs, Ys),
    member(X,Ys).