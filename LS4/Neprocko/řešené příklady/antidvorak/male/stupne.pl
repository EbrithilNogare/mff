?- consult('../functional.pl').

graf([a-b,b-c,a-c,d-c]).

%spocita stupne vrcholu v grafu
%http://forum.matfyz.info/viewtopic.php?f=169&t=8836&p=35894&hilit=stupne#p35894

addV(E-F, Acc, [E, F | Acc]).

vrcholy(G, V) :-
    foldr(addV,G,[], Vs),
    unique(Vs,V).

hranaObsahujici(V, V-_).
hranaObsahujici(V, _-V).

calcDeg(G, V, Acc, [V-L | Acc]) :-
    filter(hranaObsahujici(V), G, Spln),
    length(Spln, L).

stupne(G, Vs) :-
    vrcholy(G, V),
    foldr(calcDeg(G),V, [], Vs).