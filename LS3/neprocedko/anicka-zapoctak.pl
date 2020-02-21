/**
*Zapoctovy program - NEPROCEDURALNI PROGRAMOVANI
*TOKY V SITI
*implementace Dinicova algoritmu (zatim pro bipartitni grafy)
*vystup - matice toku
*/

getMaxFlow(N, MaxFlows) :- %vybere dle cisla N graf a vrati jeho maximalni tok - TUHLE FUNKCI VOLAM
    bipartiteGraph(N, A, B, H),
    buildNetwork([A, B], H, Vertices, Edges),
    initializeFlow(Vertices, Flows),
    maxFlow(MaxFlows, Edges, Vertices, Flows), !.

bipartiteGraph(1,[1,2,3],[4,5,6],[[1,4],[2,5],[3,6]]).
bipartiteGraph(2,[1,2,3,4,5],[6,7,8,9],[[1,6],[2,6],[2,7],[3,8],[3,9],[4,7],[5,9],[5,6]]).
bipartiteGraph(3,[1,2,3],[4,5,6],[[1,4],[1,5],[1,6],[2,4],[2,5],[2,6],[3,4],[3,5]]).
bipartiteGraph(4,[1,3,5],[2,4,7,6],[[1,2],[3,2],[3,4],[3,7],[5,4],[5,6],[1,6]]).

%orientate(+FirstPartition, +SecondPartition, +Edges, +InitOutput, -Output) %z neorientovanych hran udela orientovane
orientate(_, _, [], Output, Output).
orientate(FirstPartition, SecondPartition, [[X,Y]|Edges], InitOutput, Output) :-
    ((member(Y,FirstPartition), member(X,SecondPartition)) ->
        orientate(FirstPartition, SecondPartition, Edges, [1-[Y,X]|InitOutput], Output)
    ;
        orientate(FirstPartition, SecondPartition, Edges, [1-[X,Y]|InitOutput], Output)
    ).

%isBipartite+[FirstPartition, SecondPartition], +Edges) %overeni bipartity
isBipartite([FirstPartition, _], Edges) :-
    member(X, FirstPartition),
    member(Y, FirstPartition),
    not(member(_-[X,Y], Edges)).
isBipartite([_, SecondPartition], Edges) :-
    member(X, SecondPartition),
    member(Y, SecondPartition),
    not(member(_-[X,Y], Edges)).

%reverseEdges(+Edges, -ReverseEdges) %ke kazde hrane prida zpetnou hranu s hodnotou 0
reverseEdges([], []).
reverseEdges([1-[X,Y]|Edges], [0-[Y,X]|OutputEdges]) :- reverseEdges(Edges,OutputEdges).

%initializeFlow(+Vertices, -FlowMatrix) %inicializace toku
initializeFlow(Vertices, FlowMatrix) :-
    length(Vertices, N),
    buildMatrix(N, N, FlowMatrix, []).

%buildMatrix(+N, +M, -FlowMatrix, ?Matrix) %postaveni matice
buildMatrix(0, _, FlowMatrix, FlowMatrix).
buildMatrix(X, Y, FlowMatrix, Matrix) :-
    buildRow(0, Y, Row),
    X1 is X - 1,
    buildMatrix(X1, Y, FlowMatrix, [Row|Matrix]).

%buildRow(+X, +N, -List) %postaveni radku
buildRow(X, N, List) :-
    length(List, N),
    maplist(=(X), List).

%maxFlow(-Flows, +,Edges, +Vertices, +InitFlow) %najde zlepsujici tok a zlepsi ho
maxFlow(Flows, Edges, Vertices, InitFlow) :-
    once(buildResidualNetwork(UrovNewEdges, InitFlow, Edges, Vertices)), %postavi urovnovou sit
    (UrovNewEdges \= []) -> %kdyz je neco v urovnove siti
    (
        once(findAndAugment(UrovNewEdges, Vertices, InitFlow, NewFlows)), %najde na siti vsechny zlepsujici cesty a zlepsi je
        maxFlow(Flows, Edges, Vertices, NewFlows)
    );(
       Flows = InitFlow %kdyz uz v urovnove siti nic neni, je to maximalni tok
    ).

%buildResidualNetwork(-Sorted, +Flows, +Edges, +Vertices) %postavi urovnovou sit
buildResidualNetwork(Sorted, Flows, Edges, Vertices) :-
    findall(Path, bfs(Path, Flows, Edges, Vertices), Paths), %najde vsechny vhodne cesty
    findMin(Paths, Min, 100), %z nich vybere tu nejkratsi
    lenMin(Paths, Min, MinPaths, []), %nahradi dvojice vrcholu hranami, ktere je spojuji
    toEdges(MinPaths, NoveMinPaths, Edges),
    sort(NoveMinPaths, Sorted). %setridi hrany a odstrani duplicitu


%bfs(-Path, +Flows, +Edges, +Vertices) %najde vsechny vhodne cesty pomoci BFS
bfs(Path, Flows, Edges, Vertices) :-
    bfs1([[s]|Z]-Z, PathR, Flows, Edges, Vertices),
    reverse(PathR, Path).

%bfs1(+Fronta, -Path, +Flows, +Edges, +Vertices) %pomocna fce pro bfs
bfs1([Xs|_]-_, Xs, _, _, _) :- Xs=[t|_].
bfs1([[X|Xs]|Xss]-Z, Path, Flows, Edges, Vertices) :-
    findall([Y,X|Xs],
        (
            member(K-[X,Y], Edges),
            nth0(C1, Vertices, X),
            nth0(C2, Vertices, Y),
            nth0(C1, Flows, List),
            nth0(C2, List, Tok),
            Tok < K, %pripustne hrany maji tok v matici toku ostre mensi nez je jejich kapacita
            \+ member(Y,[X|Xs])
        ),
            NewPaths
    ),
    append(NewPaths, ZZ, Z), !,
    Xss \== ZZ,
    bfs1(Xss-ZZ, Path, Flows, Edges, Vertices).

%findMin(+Paths, -Min, ?InitMin) %najde nejkratsi seznam v seznamu seznamu
findMin([], Min, Min).
findMin([X|Xs], Min, Z) :-
    length(X, Lx),
    NZ is min(Z,Lx),
    findMin(Xs, Min, NZ).

%lenMin(+Paths, +Min, -MinPaths, ?InitMinPaths) %ve vystupnim seznamu budou jen listy se zadanou delkou
lenMin([], _, Y, Y).
lenMin([X|Xs], Min, Y, Z) :- %kdyz je seznam X delky Min, pridam ho do seznamu
    (
        length(X, Min),
        append([X], Z, NZ),
        lenMin(Xs, Min, Y, NZ)
    );(
        length(X, NMin),
        NMin \= Min,
        lenMin(Xs, Min, Y, Z)
    ).

%toEdges(+Paths, -PathsWithEdges, +Edges) %nahradi dvojice vrcholů prislusnymi hranami v seznamu cest
toEdges([], [], _).
toEdges([X|Xs], Output, Edges) :-
    exchangePath(X, PathH, [], Edges),
    toEdges(Xs, PartOutputu, Edges),
    append(PathH, PartOutputu, Output).

%exchangePath(+Path, -PathsEdgemi, ?PocPathsEdgemi, +Edges) %nahradi dvojice vrcholů prislusnymi hranami v jedne ceste
exchangePath(X, PathH, InitPath, Edges) :- %cesta je tvorena uz jen poslednimi vrcholy
    length(X, 2),
    head(X, Y),
    last(X, Z),
    member(K-[Y,Z],Edges),
    append([K-[Y,Z]], InitPath, PathH).
exchangePath([X|Xs], PathH, InitPath, Edges) :- %nahradi prvni dva vrcholy prislusnou hranou a rekurzivne nahradi cestu bez prvniho vrcholu
    head(Xs, Y), member(K-[X,Y], Edges),
    append([K-[X,Y]],InitPath, NovaPath),
    exchangePath(Xs, PathH, NovaPath, Edges).

%head(+List, -Head) %hlava listu
head([Y], Y) :- !.
head([X|_], X).

%findAndAugment(+Edges, +Vertices, +Flows, -NewFlows) %najde vsechny zlepsujici cesty pomoci DFS a zlepsi je
findAndAugment(Edges, Vertices, Flows, NewFlows) :-
    (dfs(Path,Edges,Vertices,Flows,RFlow) -> (
        exchangePath(Path,NovaPath,[],Edges), !,
        augmentPath(NovaPath,Flows,ZlepseneFlows,RFlow,Vertices), !,
        findAndAugment(Edges,Vertices,ZlepseneFlows,NewFlows), !
    );
        Flows = NewFlows
    ).

%dfs(-Path, +Edges, +Vertices, +Flows, -RFlow) %dfs najde cestu ze zdroje do stoku a jeji rezidualny tok pomoci dfs
dfs(Path,Edges,Vertices,Flows,RFlow):-
    dfs1(s,t,[s],C, Edges,Vertices,Flows,1,RFlow),
    reverse(C,Path).

%dfs1(+X, +Y, +Navstivene, -Path, +Edges, +Vertices, +Flows, +InitRFlow, -RFlow) %pomocna fce pro dfs
dfs1(X,X,C,C, _,_,_,RFlow,RFlow).
dfs1(X,Z,Nav,C,Edges,Vertices,Flows,InitRFlow,RFlow):-
    member(K-[X,Y], Edges),
    nth0(C1, Vertices, X),
    nth0(C2, Vertices, Y),
    nth0(C1, Flows, List),
    nth0(C2, List, Tok),
    CiastRFlow is K - Tok,
    CiastRFlow \= 0,
    NInitRFlow is min(InitRFlow, CiastRFlow),
    \+ member(Y,Nav),
    dfs1(Y,Z,[Y|Nav],C, Edges, Vertices, Flows, NInitRFlow, RFlow).

%augmentPath(+Path, +Flows, -ZlepseneFlows, +ResidualFlow, +Vertices) %zlepseni toku po vsech hranach na ceste
augmentPath([], NewFlows, NewFlows, _, _).
augmentPath([_-[X,Y]|Xs], Flows, NewFlows, RFlow, Vertices) :- nth0(C1, Vertices, X),
    nth0(C2, Vertices, Y),
    Min is min(C1,C2),
    Max is max(C1,C2),
    Dif is Max - Min - 1,
    firstN(Min, Flows, FirstN, [], [A|As]),
    firstN(Max, A, Edge, [], [B|Bs]),
    firstN(Dif, As, NextN, [], [C|Cs]),
    firstN(Min, C, OpEdge, [], [D|Ds]), %chceme radky a sloupce, takze rozlozime matici
    (Min = C1 -> (
        NewFlow is B + RFlow,
        NewOpFlow is D - RFlow,
        append(Edge, [NewFlow|Bs], NewEdges),
        append(FirstN, [NewEdges|NextN], NewPart),
        append(OpEdge, [NewOpFlow| Ds], NewOpEdges),
        append(NewPart, [NewOpEdges|Cs], Flows2) %slozime matici zpatky, bude mit novou hodnotu toku na hrane
    );(
        NewFlow is D + RFlow,
        NewOpFlow is B - RFlow,
        append(Edge, [NewOpFlow|Bs], NewEdges),
        append(FirstN, [NewEdges|NextN], NewPart),
        append(OpEdge, [NewFlow| Ds], NewOpEdges),
        append(NewPart, [NewOpEdges|Cs], Flows2) %slozime matici zpatky, bude mit novou hodnotu toku na hrane
    )),
    augmentPath(Xs, Flows2, NewFlows, RFlow, Vertices). %rekurzivne zlepsujeme dalsi hrany na ceste


%firstN(+N, +List, -FirstN, +Start, -Remainder) %rozdeleni seznamu na prvni n polozek a zbytek
firstN(_, [], FirstN, FirstN, []).
firstN(0, Remainder, FirstN, FirstN, Remainder).
firstN(N, [X|Xs], FirstN, Part, Remainder) :-
    N \= 0,
    append(Part, [X], NewPart),
    N1 is N - 1,
    firstN(N1, Xs, FirstN, NewPart, Remainder).


%buildNetwork(+[FirstPartition, SecondPartition], +Edges, -Vertices, -OutputEdges) %udela z grafu sit
buildNetwork([FirstPartition, SecondPartition], Edges, Vertices, OutputEdges) :-
      orientate(FirstPartition, SecondPartition, Edges, [], Output), %zorientuje hrany
      isBipartite([FirstPartition, SecondPartition], Output), %kontrola bipartity
      findall(1-[s,X], member(X, FirstPartition), EdgesS),
      findall(1-[Y,t], member(Y, SecondPartition), EdgesT),
      append(EdgesS, EdgesT, NewEdges),
      append(Output, NewEdges, OutputEdges1),
      reverseEdges(OutputEdges1, ReverseEdges),
      append(OutputEdges1, ReverseEdges, OutputEdges),
      append([s|FirstPartition], [t|SecondPartition], Vertices). %Vertices = list se vsemi vrcholy v siti
