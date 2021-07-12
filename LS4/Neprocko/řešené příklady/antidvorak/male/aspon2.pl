?- consult('../functional.pl').
?- consult('../mnozinove_operace.pl').

%http://forum.matfyz.info/viewtopic.php?f=169&t=11380

lisi_alespon2(O1, O2) :-
    length(O1, L),
    intersection(O1,O2, O),
    length(O, K),
    L - K >= 2.

aspon2(V, X) :-
    map(fst, V, Promenne),
    length(Promenne, L),
    ohodnoceni(L, VsechnaOhodnoceni),
    foldl(p(Promenne, V),VsechnaOhodnoceni, [], X).

  %  mnohem lepsi reseni, ktere je bohuzel zakazane
  %  findall(PO, ( member(O, VsechnaOhodnoceni), zip(Promenne, O, PO), lisi_alespon2(PO, V) ), X).

ohodnoceni(N, O) :- 
    N > 0,
    M is N-1,
    ohodnoceni(M, P),
    map(pushFront(false), P, R),
    map(pushFront(true), P, S),
    append(R,S,O).

ohodnoceni(0,[[]]).

p(Promenne, V, Ohodnoceni, Acc, [O | Acc]) :-
    zip(Promenne, Ohodnoceni, O),
    lisi_alespon2(O, V), !.

p(_,_,_,Acc,Acc).
