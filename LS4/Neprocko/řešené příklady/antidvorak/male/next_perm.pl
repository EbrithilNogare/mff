%nasledujici permutace

rozdel([A,B | PermR], [A | RostZac], X, Zbytek) :- 
    A < B, 
    rozdel([B | PermR], RostZac, X, Zbytek).

rozdel([A,B | PermR], [A], B, PermR) :- A > B.

prohod(X, [Z | RostZac], Y, [Z | NovyRostZac]) :- 
    Z < X,
    prohod(X, RostZac, Y, NovyRostZac).

prohod(X, [Z | RostZac], Z, [X | NovyRostZac]) :- 
    Z > X,
    zkopirujZbytek(X, RostZac, Z, NovyRostZac).

zkopirujZbytek(_, [], _, []).
zkopirujZbytek(A,[X | RostZac], B, [X | NovyRostZac]) :- zkopirujZbytek(A, RostZac, B, NovyRostZac).


next_perm(Perm, Ys) :- 
    reverse(Perm,PermR),
    rozdel(PermR, RostZac, X, Zbytek),
    prohod(X,RostZac, Y, NovyRostZac),
    reverse(Zbytek, ZbytekN),
    append(ZbytekN,[Y | NovyRostZac],Ys).