?- consult('../permutace.pl').

vstup(G) :- G = graf([a,b,c,d,e,f], [h(a,d), h(a,f), h(b,e), h(b,d), h(c,d), h(c,f)]).

%http://forum.matfyz.info/viewtopic.php?f=169&t=10993&p=40570&hilit=parovani#p40570

vrcholy(G, Vertices) :- arg(1, G, Vertices).
vrchol(G, V) :- vrcholy(G, Vertices), member(V, Vertices).

hrana(G, E, F) :-
    arg(2, G, Edges),
    E1 =.. [h,E,F],
    member(E1, Edges).

existujeHranaNaParovani(G, H) :-
    hrana(G, E, F),
    vrchol(G, E),
    vrchol(G, F),
    H =.. [h,E,F].

parovani(G, MaxParovani, MaxParovani) :- \+ existujeHranaNaParovani(G, _).
parovani(G, Parovani, MaxParovani) :-
    existujeHranaNaParovani(G, H),
    H =.. [_, E, F],
    G =.. [_, Vrcholy, Hrany],
    once(select(E, Vrcholy, V1)),
    once(select(F, V1, V2)),
    G2 =.. [graf, V2, Hrany],
    parovani(G2, [H | Parovani], MaxParovani).


            %do inkluze
parovani(G, MaxParovani) :-
    parovani(G, [], MaxParovani).

existuje_nenalezene_parovani(G, Acc, P) :-
    parovani(G, P),
    \+ permutation_member(P, Acc).

%vsechnaParovani(G, VParovani) :- vsechnaParovani(G, [], VParovani).
vsechnaParovani(G, VParovani) :- vsechnaParovani(G, [], P), member(VParovani, P).

vsechnaParovani(G, Acc, VParovani) :-
    existuje_nenalezene_parovani(G, Acc, P), !,
    vsechnaParovani(G, [P | Acc], VParovani).

vsechnaParovani(_,Acc,Acc).