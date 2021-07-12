vstup(G) :- G = graf([a,b,c,d,e,f], [h(a,d), h(a,f), h(b,e), h(b,d), h(c,d), h(c,f)]).

%mejme graf, napiste predikat, ktery rozhodne, jestli je bipartitni
%tvrzeni dokazte tim, ze vratite prislusne vrcholy rozdelene do dvou partit

vrcholy(G, Vertices) :- arg(1, G, Vertices).
vrchol(G, V) :- vrcholy(G, Vertices), member(V, Vertices).

hrana(G, E, F) :-
    arg(2, G, Edges),
    E1 =.. [h,E,F],
    E2 =.. [h,F,E],
    ( member(E1, Edges) ; member(E2, Edges) ).

splitList([],[],[]).
splitList([X | Xs], [X | Ys], Zs) :- splitList(Xs, Ys, Zs).
splitList([X | Xs], Ys, [X | Zs]) :- splitList(Xs, Ys, Zs).

existujePorusujiciHrana(G, P1, P2) :-
    ( 
        member(U, P1),
        member(V, P1),
        hrana(G, U, V)
    ) ;
    (
        member(U, P2),
        member(V, P2),
        hrana(G, U, V)
    ).

%testuje jestli graf G, jde rozlozit na dve partity X a Y
je_bipartitni(G, X, Y) :-
    vrcholy(G, Vertices),
    splitList(Vertices, X, Y),
    \+ existujePorusujiciHrana(G, X, Y).

