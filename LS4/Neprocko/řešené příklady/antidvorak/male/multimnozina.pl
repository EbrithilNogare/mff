?- consult('../functional.pl').
?- consult('../mnozinove_operace.pl').

%http://forum.matfyz.info/viewtopic.php?f=169&t=11357

jeVetsi(false, _, _, false) :- !.
jeVetsi(_, A, X, true) :- 
    atom_string(A, AA),
    atom_string(X, XX),
    AA > XX.

mensi(MM1, MM2) :-
    map(fst, MM1, M1),
    map(fst, MM2, M2),
    difference(M2, M1, M), % prvky jenom v 2.
    difference(M1, M2, N), %prvky jenom v 1.
    member(A, M),
    foldl(jeVetsi(A),N,true,true).