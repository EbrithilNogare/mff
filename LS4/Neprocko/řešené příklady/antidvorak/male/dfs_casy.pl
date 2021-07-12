?- consult('../functional.pl').

vstup(graf([a,b,c,d,e,s], [h(a,s), h(a,b), h(a,c), h(b,d), h(b,e),h(c,e),h(d,e)] )).

%tohle je hroznej humus
%http://forum.matfyz.info/viewtopic.php?f=169&t=11756&p=41531&hilit=dfs#p41531

obsahuje_souseda(U,h(U,B),Acc,[B|Acc]) :- !.
obsahuje_souseda(U,h(A,U),Acc,[A|Acc]) :- !.
obsahuje_souseda(_,_,Acc,Acc).

sousedi(G, U, Vs) :-   
    arg(2,G, E),
    foldl(obsahuje_souseda(U),E,[], Vs).

dfs_casy(G, V, Z) :- dfs_casy1(G, [V], 0, _-_-Z).


nenavstiven(Visited, V) :- \+ member(V, Visited).
                               %navstivene-dalsi_pouzitelne_cislo-ocislovane_vrcholy
dfs_casy1(G, [V | Visited], C, [V|Visited]-E-[V-C-D]) :-
    D is C+1,
    E is C+2,
    sousedi(G, V, Sousedi),
    filter(nenavstiven(Visited), Sousedi, PripustniS),
    PripustniS = [], !.

dfs_casy1(G, [V | Visited], C, NV-F-[V-C-NC|Vs]) :-
    D is C+1,
    sousedi(G, V, Sousedi),
    filter(nenavstiven(Visited), Sousedi, PripustniS),
    zavolej(PripustniS, [V|Visited], G, D, [], NV-NC-Vs),
    F is NC+1.    

zavolej([], Visited, _, D, Vd, Visited-D-Vd).
zavolej([P|Ps], Visited, G, D, Vd, Z) :-
    member(P,Visited), !,
    zavolej(Ps,Visited,G,D,Vd, Z).

zavolej([P|Ps], Visited, G, D, Vd, Z) :-
    dfs_casy1(G, [P|Visited], D, NV-E-Vs),
    append(Vd, Vs, V),
    zavolej(Ps, NV, G, E, V, Z).