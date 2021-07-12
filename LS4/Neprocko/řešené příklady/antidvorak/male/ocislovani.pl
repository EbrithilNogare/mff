%dostaneme binarni/n-arni strom
%pridejte kazdemu vrcholu dve cisla 
%jedno znaci poradi preorder pruchodem, druhe postorder pruchodem


%binarni stromy
vstup(t(t(t(nil,10,nil),25,t(nil,35,nil)),50,t(t(nil,60,nil),75,nil))).
vstup2(t(t(nil,20,nil),50,nil)).

ocisluj(T, OT) :- ocisluj1(T, 1, 1, _, _, OT).

ocisluj1(nil, P, Q, P, Q, nil).
ocisluj1(t(LS,H,PS), P, Q, C, T, t(OLS, H-P-S, OPS)) :-
    A is P + 1, % P je na preorder do korene
    ocisluj1(LS, A, Q, B, R, OLS),
    ocisluj1(PS, B, R, C, S, OPS),
    T is S + 1. % S je na postorder do korene




%n-arni stromy
vstup3(t(10,[t(5,[t(3,[]), t(8,[])]), t(12,[]), t(40,[t(35,[]), t(37,[]), t(42,[]), t(43,[])])])).

ocislujN(T, OT) :- ocislujPrvek(T, 1, 1, _, _, OT).

ocislujPrvek(T, A, Q, C, S, NT) :-
    T =.. [t, H, Xs],
    B is A + 1,
    ocislujPole(Xs, B, Q, C, R, OT),
    S is R+1,
    NT =.. [t, H-A-R, OT].

ocislujPole([], A, Q, A, Q, []).
ocislujPole([X | Xs], A, Q, C, S, [OX |OXs]) :-
    ocislujPrvek(X, A, Q, B, R, OX),
    ocislujPole(Xs, B, R, C, S, OXs).