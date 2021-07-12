?- consult('../functional.pl').
?- consult('../mnozinove_operace.pl').
?- consult('../permutace.pl').

graf(G) :- G = graf([a,b,c,d,e],[h(a,b), h(b,c), h(c,d), h(d,b), h(d,e), h(e,b)]).

%http://forum.matfyz.info/viewtopic.php?f=169&t=11466&p=41155&hilit=pokryti#p41155

vrchol(V, G) :- 
    arg(1, G, Vertices),
    member(V, Vertices).

hrana(E, F, G) :- 
    arg(2, G, Edges),
    H =.. [h, E, F],
    member(H, Edges).

po(V, Edge, Acc, NewAcc) :-
    Edge =.. [h, U, W],
    (U = V ; W = V),
    setPushFront(U, Acc, NAcc),
    setPushFront(W, NAcc, NewAcc).

po(V, Edge, Acc, Acc) :-
    Edge =.. [h, U, W],
    U \= V, 
    W \= V.

pokryje(V, G, NewV) :-
    arg(2, G, Edges),
    foldl(po(V), Edges,[], NewV).

nepokryje(V, G, CoNepokryje) :-
    pokryje(V, G, CoPokryje),
    arg(1, G, Vertices),
    difference(Vertices, CoPokryje, CoNepokryje).

pokryti(G, VPokryti) :- pokryti(G, [], VPokryti).

pokryti(G, Acc, Acc) :-
    G =.. [graf, Vertices, _],
    length(Vertices, L),
    L == 0.

pokryti(G, Acc, VPokryti) :-
    G =.. [graf, Vertices, Edges],
    length(Vertices, L),
    L =\= 0,
    vrchol(V, G),
    nepokryje(V, G, NewV),
    G2 =.. [graf, NewV, Edges],
    pokryti(G2, [V | Acc], VPokryti).

existuje_nenalezene_pokryti(G, Acc, P) :-
    pokryti(G, P),
    \+ permutation_member(P, Acc).

%vsechnaPokryti(G, VPokryti) :- vsechnaPokryti(G, [], VPokryti).
vsechnaPokryti(G, VPokryti) :- vsechnaPokryti(G, [], Pokryti), member(VPokryti, Pokryti).

vsechnaPokryti(G, Acc, VPokryti) :-
    existuje_nenalezene_pokryti(G, Acc, P), !,
    vsechnaPokryti(G, [P|Acc], VPokryti).

vsechnaPokryti(_, Acc, Acc).