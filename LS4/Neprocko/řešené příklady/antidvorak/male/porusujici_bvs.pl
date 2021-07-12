%vrcholy, ktere porusuji podminku BVS

vstup(T) :- T = t(t(t(nil,40,nil),60,t(nil,70,nil)),50,t(t(nil,3,nil),80,t(nil,1,nil))).

hodnota(t(_,H,_), H).

porusuje(t(_,H,R), RH) :-
    hodnota(R, RH),
    H > RH.

porusuje(t(L,H,_), LH) :-
    hodnota(L, LH),
    LH > H.

porusuje(t(L,_,_), X) :-
    porusuje(L, X).

porusuje(t(_,_,R), X) :-
    porusuje(R, X).