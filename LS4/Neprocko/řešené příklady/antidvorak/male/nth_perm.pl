%najdete n-tou permutaci (bez iterativniho hledani)

%tohle je principielne uplne jednoduche,
%ale nenaprogramujete to pokud neznate algoritmus
%alg: https://medium.com/@aiswaryamathur/find-the-n-th-permutation-of-an-ordered-string-using-factorial-number-system-9c81e34ab0c8

factorial(X, Y) :- factorial(X, 0, 1, Y).

factorial(X, X, Acc, Acc) :- !.
factorial(X, Y, Acc, Out) :-
    Z is Y+1,
    W is Acc * Z,
    factorial(X, Z, W, Out).

to_fact_system(V, Xs) :- to_fact_system(V, 2, [0], Xs).

to_fact_system(0, _, Acc, Acc) :- !.
to_fact_system(V, N, Acc, Out) :-
    Z is V mod N,
    M is N+1,
    U is V div N,
    to_fact_system(U, M, [Z|Acc], Out).

construct_perm([], _, Acc, Out) :- !, reverse(Acc, Out).
construct_perm(FPerm, [D|N], Acc, Out) :-
    nth0(D, FPerm, V), %index od 0
    once(select(V, FPerm, NPerm)),
    construct_perm(NPerm, N, [V|Acc], Out).

fill_to_match(X, L, Out) :-
    length(X,K),
    K < L, !,
    fill_to_match([0|X], L, Out).

fill_to_match(X, _, X).

nth_perm(FPerm, N, OutPerm) :-
    N >= 0,
    to_fact_system(N, FN),
    length(FPerm, L),
    factorial(L, M),
    N < M,
    fill_to_match(FN, L, FNM),
    construct_perm(FPerm, FNM, [], OutPerm).
