vstup(T) :- T = t(t(t(nil,5,nil),10,t(nil,15,nil)),20,t(nil,50,t(nil,100,nil))).

%http://forum.matfyz.info/viewtopic.php?f=169&t=10036&p=39223&hilit=orez#p39223

orez(_, _, nil, nil) :- !.
orez(L, U, t(LS, H, PS), t(NL, H, NP)) :-
    H >= L,
    H =< U,
    orez(L, U, LS, NL),
    orez(L, U, PS, NP), !.

orez(L, U, t(_, H, PS), NP) :-
    H < L,
    orez(L,U,PS, NP), !.

orez(L, U, t(LS, H, _), NL) :-
    H > U,
    orez(L,U,LS, NL), !.
