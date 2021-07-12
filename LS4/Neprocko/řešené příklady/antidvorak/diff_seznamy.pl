%rozdilove seznamy

diff2list(X-[], X).

list2diff([], X-X).
list2diff([H | T], [H | S]-X) :- list2diff(T,S-X). 

join(A-B, B-C, A-C).

push_back(X, A-[X | C], A-C).

rotate([],X-X).
rotate([X | Xs], Ys-B) :- rotate(Xs, Ys-[X | B]).

rotate2([]) --> [].
rotate2([H|T]) --> rotate2(T), [H].

rotuj(Xs, Ys) :- phrase(rotate2(Ys), Xs).


diff_rotace(X-X,Y-Y) :- X = [], !.
diff_rotace([X | Xs]-A, Y-B) :- diff_rotace(Xs-A, Y-[X|B]).
