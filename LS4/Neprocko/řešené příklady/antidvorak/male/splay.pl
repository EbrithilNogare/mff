testStrom(X) :-
    X = t(t(t(nil,1,nil),5,t(nil,7,nil)),10,t(nil,40,t(nil,50,nil))).

%klasika - splay, na foru asi tisickrat
%http://forum.matfyz.info/viewtopic.php?f=169&t=10961&p=40530&hilit=splay#p40530

splay(H, t(L, H, R), t(L,H,R)) :- !.

splay(H, t(L, V, R), Vysl) :-
    H < V,
    splay(H, L, LP),
    rotaceDoprava(t(LP, V, R), Vysl).

splay(H, t(L, V, R), Vysl) :-
    H > V,
    splay(H, R, PP),
    rotaceDoleva(t(L,V,PP), Vysl).

rotaceDoprava(t(t(A,LV, C), V, B), t(A, LV, t(C,V,B))).
rotaceDoleva( t(A,V,t(C,RV,B)), t(t(A,V,C),RV,B)).


