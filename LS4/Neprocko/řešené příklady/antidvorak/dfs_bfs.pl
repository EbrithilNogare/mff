graf([a,b,c,d], [h(a,b), h(b,c), h(c,a), h(c,d)]).

%DFS a BFS v prologu

vrcholy(V) :- graf(V,_).
hrany(E) :- graf(_,E).

hrana(X,Y) :- 
    E =.. [h, X, Y],
    hrany(Edges),
    member(E,Edges).

dfs(X, Y, Path) :- 
    dfs(X, Y, [X], PathR),
    reverse(PathR, Path).

dfs(X, X, Path, Path).

dfs(X, Y, Visited, Path) :-
    hrana(X, Z),
    \+ member(Z, Visited),
    dfs(Z, Y, [Z | Visited], Path).




bfs(Start,Dst,Path):-
    bfs1([[Start]],Dst,PathRev),            
    reverse(PathRev,Path).

bfs1([Xs|_], Dst, Xs):- 
    Xs=[Dst|_].

bfs1([[X|Xs]|Xss], Dst, PathR):-
    findall([Y,X|Xs],(hrana(X,Y),\+ member(Y,[X|Xs])), NewPaths),
    append(Xss,NewPaths,NewQueue),
    !,
    bfs1(NewQueue,Dst,PathR).