?- consult('../functional.pl').
?- consult('../kombinace.pl').

vstup(G) :- G = [a-[b,c,d], b-[a,c], c-[a,b,d], d-[a,c]].

%najde vsechny trojuhelniky v grafu
%http://forum.matfyz.info/viewtopic.php?f=169&t=10036&p=39223&hilit=trojuhelniky#p39223

vrcholy(G, Vrcholy) :-
    vstup(G),
    map(fst, G, Vrcholy).

vrchol(G, V) :-
    vrcholy(G, Vrcholy),
    member(V, Vrcholy).

vyrobHrany2(V, Nasledovnik, Acc, [V-Nasledovnik | Acc]).

vyrobHrany(V-Nasledovnici, NalezeneHrany, Hrany) :-
    foldl(vyrobHrany2(V),Nasledovnici,[], NoveHrany),
    append(NalezeneHrany, NoveHrany, Hrany).

hrany(G, Hrany) :- foldl(vyrobHrany,G,[],Hrany).

hrana(G, E) :- 
    hrany(G, Hrany),
    member(E, Hrany).

jeTrojuhelnik([X,Y,Z], G) :-
    hrana(G,X-Y),
    hrana(G, Y-Z),
    hrana(G, Z-X).

vyrobTrojuhelnik(G,[X,Y,Z], Acc, [N | Acc]) :-
    jeTrojuhelnik([X,Y,Z],G), !,
    N =.. [t,X,Y,Z].

vyrobTrojuhelnik(G,[X,Y,Z], Acc, Acc) :- 
    \+ jeTrojuhelnik([X,Y,Z], G).


trojuhelniky(G, Ts) :- 
    vrcholy(G, Vs),
    combinations(3, Vs, Cs),
    foldl(vyrobTrojuhelnik(G),Cs,[], Ts).


% trojce(Vrcholy, X, Y, Z) :-
%     member(X, Vrcholy),
%     member(Y, Vrcholy),
%     member(Z, Vrcholy),
%     X \= Y,
%     X \= Z,
%     Y \= Z.

% permutace([], []).
% permutace([X | Xs], Ys) :-
%     permutace(Xs, Zs),
%     select(X, Ys, Zs).

% permutationMember(X, Xs) :-
%     permutace(X, P),
%     member(P, Xs).

% existujePridatelnyTrojuhelnik(G, Acc, T) :-
%     trojuhelnik(G, T),
%     \+ permutationMember(T, Acc).

% vsechnyTrojuhelniky(G, Acc, Acc) :- \+ existujePridatelnyTrojuhelnik(G, Acc, _), !.

% vsechnyTrojuhelniky(G, Acc, Ts) :-
%     existujePridatelnyTrojuhelnik(G, Acc, T),
%     !,
%     vsechnyTrojuhelniky(G, [T | Acc], Ts).

% vsechnyTrojuhelniky(G, Ts) :- vsechnyTrojuhelniky(G, [], Rs), foldl(vyrobTrojuhelnik, Rs, [], Ts).

% vyrobTrojuhelnik([X,Y,Z], Acc, Out) :- T =..[t,X,Y,Z], Out = [T | Acc].

% trojuhelnik(G, [X,Y,Z]) :-
%     vrcholy(G, Vrcholy),
%     trojce(Vrcholy, X, Y, Z),
%     hrana(G,X-Y),  
%     hrana(G,X-Z),
%     hrana(G,Y-Z).