% a) reprezentace stromu:
% [(vrchol, [jehoDeti])]
% [(1, [(2, []), (3, [])])]

% b)
iso([], []).
iso([(Node1, Tree1)], [(Node1, Tree2)]) :-
    iso(Tree1, Tree2).
iso([(Node1, Tree1) | _], [(Node1, Tree2) | _]) :-
    iso(Tree1, Tree2).
iso([(Node1, Tree1) | Rest1], [(Node2, _) | Rest2]) :-
    Node1 \= Node2,
    iso([(Node1, Tree1) | Rest1], Rest2).
        
test :-
    Tree1 = [(1, [(2, [(4, [])]), (3, [(5, []), (6, [])])])],
    Tree2 = [(1, [(3, [(6, []), (5, [])]), (2, [(4, [])])])],
    iso(Tree1, Tree2),
    !.

% Tree1:
% 1
% |\
% 2 3
% | |\
% 4 5 6

% Tree2:
% 1
% |\
% 3 2
% |\ \
% 5 6 4

% c) zadny z nich neni nedeterministicky