?- consult('../functional.pl').
?- consult('../mnozinove_operace.pl').

vstup(G) :- G = graf([a,b,c,d,e], [a-b,b-c,b-d,c-d]).

%http://forum.matfyz.info/viewtopic.php?f=169&t=11457&p=41144&hilit=nez#p41144

vrcholy(G, Vrcholy) :- arg(1, G, Vrcholy).
vrchol(G, V) :- vrcholy(G, Vrcholy), member(V, Vrcholy).

hrany(G, Hrany) :- arg(2, G, Hrany).
hrana(G, E, F) :-
    vrchol(G,E),
    vrchol(G,F),
    hrany(G, Hrany),
    member(E-F, Hrany).

jeIncidentni(V, V-_).
jeIncidentni(V, _-V).

konfigurace(A-A, A-A).
konfigurace(A-B, A-B) :- \+ A == B.
konfigurace(A-B, B-A) :- \+ A == B.

zbylyGraf(G, PokryteVrcholy, G2) :-
    vrcholy(G, Vrcholy),
    hrany(G, Hrany),
    difference(Vrcholy, PokryteVrcholy, NoveVrcholy),
    G2 =.. [graf, NoveVrcholy, Hrany].

po(V, Hrana, Acc, Acc) :- \+ jeIncidentni(V, Hrana), !.
po(V, Hrana, Acc, [W | Acc]) :- 
    konfigurace(Hrana, H),
    H = V-W.

pokryje(G, NezMn, NezMn) :- \+ vrchol(G, _).
pokryje(G, Acc, NezMn) :-
    hrany(G, Hrany),
    vrchol(G, V),
    foldl(po(V), Hrany, [], PokryteVrcholy),
    zbylyGraf(G, [V | PokryteVrcholy], G2),
    pokryje(G2, [V | Acc], NezMn).

pokryjePrvni(G, V, PokryteVrcholy) :-
    hrany(G, Hrany),
    foldl(po(V), Hrany, [], PokryteVrcholy).

nez(G, V, NezMn) :-
    vrchol(G, V),
    pokryjePrvni(G, V, PokryteVrcholy),
    zbylyGraf(G, [V | PokryteVrcholy], G2),
    pokryje(G2, [V], NezMn).