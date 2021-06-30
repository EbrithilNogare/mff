% a) reprezentace stromu:
% [(vrchol, [jehoDeti])]
% [(1, [(2, []), (3, [])])]

% b)
iso(Tree1, Tree2) :-
    isoTree(Tree1, Tree2, Tree2),!.

isoTree([], [], _).

isoTree([(Node1, Tree1)], [(Node1, Tree2)], _) :-
    isoTree(Tree1, Tree2, Tree2).

isoTree([(Node1, Tree1) | Rest1], [(Node1, Tree2) | _], WholeTree2) :-
    isoTree(Tree1, Tree2, Tree2), % vstup do podstromu
    isoTree(Rest1, WholeTree2, WholeTree2). % pokracuj na dalsi vrchol

isoTree([(Node1, Tree1) | Rest1], [_ | Rest2], WholeTree2) :-
    isoTree([(Node1, Tree1) | Rest1], Rest2, WholeTree2). % zkus porovnat s dalsim vrcholem z 2
        
test :-
    Tree1 = [(1, [(2, []), (3, [])])],
    Tree2 = [(1, [(2, []), (3, [])])],
    iso(Tree1, Tree2),
    !.

% c) zadny z nich neni nedeterministicky