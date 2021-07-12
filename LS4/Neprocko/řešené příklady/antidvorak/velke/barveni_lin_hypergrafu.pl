?- consult('powerset.pl').
?- consult('functional.pl').
?- consult('mnozinove_operace.pl').

%jenom uplne pitomej bruteforce
%http://forum.matfyz.info/viewtopic.php?f=169&t=10479&p=39917&hilit=hypergraf#p39917

je_linearni_hypergraf(E) :- \+ neni_linearni_hypergraf(E).
neni_linearni_hypergraf(E) :-
    member(S1, E),
    member(S2, E),
    S1 \= S2, 
    intersection(S1, S2, S),
    length(S, L),
    L > 1.

neni_linearni_hypergraf(E) :-
    member([], E),
    member(S, E),
    S \= [].

neni_linearni_hypergraf(E) :- member([_], E).
neni_linearni_hypergraf([]).

linearni_hypergraf(V, E) :-
    powerset(V, A),
    powerset(A, Es),
    filter(je_linearni_hypergraf, Es, EsF),
    member(E, EsF).

obarvi(Bs, E, Acc, [E-B|Acc]) :- member(B,Bs).

spatne_obarven(Es) :-
    member(E-B, Es),
    member(F-B, Es),
    E \= F,
    intersection(E,F,G),
    length(G, L),
    L > 0.

dobre_obarven(Es) :- \+ spatne_obarven(Es).

proved_spravne_obarveni(Bs, E, OE) :-
    foldr(obarvi(Bs), E, [], OE),
    dobre_obarven(OE), !.

obarveni(V, OE) :-
    linearni_hypergraf(V, E),
    length(V, L),
    sequence(1,1,L,Bs),
    proved_spravne_obarveni(Bs,E, OE),
    dobre_obarven(OE).
