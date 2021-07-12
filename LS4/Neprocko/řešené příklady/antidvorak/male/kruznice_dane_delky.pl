?- consult('../functional.pl').
?- consult('../permutace.pl').

vstup(G) :- G = [a-[b,c,d], b-[a,c], c-[a,b,d], d-[a,c]].

%mejme graf, najdete vsechny kruznice delky N

vrcholy(G, Vrcholy) :-
    vstup(G),
    map(fst, G, Vrcholy).

vrchol(G, V) :-
    vrcholy(G, Vrcholy),
    member(V, Vrcholy).

vyrobHrany2(V, Nasledovnik, Acc, [V-Nasledovnik | Acc]).

vyrobHrany(V-Nasledovnici, NalezeneHrany,  Hrany) :-
    foldl(vyrobHrany2(V),Nasledovnici,[], NoveHrany),
    append(NalezeneHrany, NoveHrany, Hrany).

hrany(G, Hrany) :-
    foldl(vyrobHrany,G,[],Hrany).

hrana(G, E) :- 
    hrany(G, Hrany),
    member(E, Hrany).

%vyrobTrojuhelnik([X,Y,Z], Acc, Out) :- 
%    T =.. [t, X, Y, Z],
%    Out = [T | Acc].

permutationMember(X, Xs) :-
    permutation(X, XP),
    member(XP, Xs).

existujeNenalezenaKruznice(N,G,V, Acc, Path) :-
    kruznice(N, G, V, Path),
    \+ permutationMember(Path,Acc).

vsechnyKruznice(N,G,V,Acc,Acc) :- \+ existujeNenalezenaKruznice(N,G,V,Acc, _), !.

vsechnyKruznice(N, G, V, Acc, Cesty) :-
    existujeNenalezenaKruznice(N,G,V,Acc, Path),
    !,
    vsechnyKruznice(N,G,V, [Path | Acc], Cesty).

%najde vsechny kruznice delky N z vrcholu V
vsechnyKruznice(N, G, V, T) :-
    N > 2,
    vsechnyKruznice(N,G,V,[],T).
    %foldl(vyrobTrojuhelnik, Cesty, [], T).

%najde kruznici delky N
kruznice(0, _, X, X, PathR, PathR2) :- PathR = [_ | PathR2].

kruznice(N, G, X, X, [X], PathR) :-
    N > 0,
    hrana(G, X-Z),
    M is N - 1,
    kruznice(M, G, X, Z, [Z,X], PathR).

kruznice(N, G, X, Y, Visited, PathR) :-
    N > 0,
    X \= Y,
    hrana(G, Y-Z),
    ( \+ member(Z, Visited) ; Z = X ),
    M is N - 1,
    kruznice(M, G, X, Z, [Z | Visited], PathR).

kruznice(N, G,V, PathR) :-
    vrchol(G, V),
    kruznice(N, G, V, V, [V], PathR).