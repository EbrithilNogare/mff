?- consult('../functional.pl').

?- op(400, yfx, and).
?- op(400, yfx, or).
?- op(300, fy, not).
?- op(450, xfy, =>).
?- op(500, yfx, <=>).

vstup1(V) :- V = (a => b) and (b => a) <=> (a <=> b). %yes
vstupE(V) :- V = (a => b) and (b => a) <=> (a <=> c). %no

%overi, zda je dana vyrokova formule tautologie
%http://forum.matfyz.info/viewtopic.php?f=169&t=9645&p=38384&hilit=matice#p38384

eval(A, Vals, V) :-
    atom(A),
    member(A-V, Vals).

eval(A and B, Vals, true) :- eval(A, Vals, true), eval(B, Vals, true), !.
eval(_ and _, _, false).

eval(A or B, Vals, true) :- eval(A, Vals, O1), eval(B, Vals, O2), (O1 = true ; O2 = true), !.
eval(_ or _, _, false).

eval(A => B, Vals, true) :- eval(A, Vals, O1), eval(B, Vals, O2), (O1 = false ; (O1 = true, O2 = true)), !.
eval(_ => _, _, false).

eval(A <=> B, Vals, true) :- eval(A, Vals, O1), eval(B, Vals, O2), ( (O1 = true, O2 = true) ; (O1 = false, O2 = false) ), !.
eval(_ <=> _, _, false).

eval(not A, Vals, true) :- eval(A, Vals, false), !.
eval(not _, _, false).

eval(false, _, false).
eval(true, _, true).

promenne(Formule, Ps) :-
    promenne1(Formule, P),
    unique(P, Ps).

promenne1(A, [A]) :- atom(A).
promenne1(A and B, Ps) :-
    promenne1(A, P1),
    promenne1(B, P2),
    append(P1, P2, Ps).

promenne1(A or B, Ps) :-
    promenne1(A, P1),
    promenne1(B, P2),
    append(P1, P2, Ps).

promenne1(A => B, Ps) :-
    promenne1(A, P1),
    promenne1(B, P2),
    append(P1, P2, Ps).

promenne1(A <=> B, Ps) :-
    promenne1(A, P1),
    promenne1(B, P2),
    append(P1, P2, Ps).

promenne1(not A, Ps) :- promenne1(A, Ps).

ohodnoceni(N, O) :- 
    N > 0,
    M is N-1,
    ohodnoceni(M, P),
    map(pushFront(false), P, R),
    map(pushFront(true), P, S),
    append(R,S,O).

ohodnoceni(0,[[]]).

p(Formule, Promenne, Ohod, Acc, [true | Acc]) :-
    zip(Promenne, Ohod, PO),
    eval(Formule, PO, true), !.

p(_, _, _, Acc, [false | Acc]).

tautologie(Formule) :-
    promenne(Formule, Promenne),
    length(Promenne, L),
    ohodnoceni(L, Ohod),
    foldl(p(Formule, Promenne),Ohod, [], V),
    \+ member(false, V).