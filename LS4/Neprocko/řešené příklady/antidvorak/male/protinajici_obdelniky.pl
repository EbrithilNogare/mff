?- consult('../kombinace.pl').

vstup(V) :- V = [o(0, 0, 1, 1), o(1, 1, 2, 3), o(2, 2, 3, 4)].

%http://forum.matfyz.info/viewtopic.php?f=169&t=8851

protinaji_se(O1,O2) :- \+ neprotinaji_se(O1,O2).

neprotinaji_se(o(X1,Y1,W1,H1), o(X2,Y2,W2,H2)) :-
    PX1 is X1 + W1,
    PX2 is X2 + W2,
    HY1 is Y1 + H1,
    HY2 is Y2 + H2,
    (
    PX1 < X2 ; %nalevo 
    PX2 < X1 ; %napravo
    HY1 < Y2 ; %dole
    Y1 > HY2   %nahore
    ).

protinajiSe(Os, O1-O2) :-
    combination(2, Os, [O1,O2]),
    protinaji_se(O1, O2).
