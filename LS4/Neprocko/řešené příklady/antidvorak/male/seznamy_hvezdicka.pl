%http://forum.matfyz.info/viewtopic.php?f=169&t=8337&p=34114&hilit=seznamy+s#p34114

asterisk_split([], [], []) :- !.
asterisk_split([* | Xs], [], Xs) :- !.
asterisk_split([X | Xs], [X | Ys], Zs) :- 
    X \= *,
    asterisk_split(Xs, Ys, Zs).

match([], _) :- !.
match(_, []) :- !.
match([X | Xs], [X | Ys]) :- match(Xs, Ys).

substitution(Xs, Ys) :-
    asterisk_split(Xs, X1s, X2s),
    asterisk_split(Ys, Y1s, Y2s),
    reverse(X2s, X2Rs),
    reverse(Y2s, Y2Rs),
    match(X1s,Y1s),
    match(X2Rs, Y2Rs).



