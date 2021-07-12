?- consult('../diff_seznamy.pl').
?- consult('../functional.pl').

%vypis BVS po hladinach
%nefunguje oboustranne :(
%krylovina

vstup( t(t(t(nil,10,nil),25,t(t(nil,30,nil),35,t(nil,37,nil))),50,t(nil,75,t(t(nil,80,nil),100,nil))) ).

vypis(BVS, Xss) :- vypis1([ [BVS] ], [], Xss).

hodnoty(nil, Acc, Acc).
hodnoty(t(_,V,_), Acc, [V|Acc]).

synove(t(L,_,R), Acc, [L,R|Acc]) :- L \= nil,  R \= nil, !.
synove(t(L,_,_), Acc, [L|Acc]) :- L \= nil, !.
synove(t(_,_,R), Acc, [R|Acc]) :- R \= nil, !.
synove(t(nil,_,nil), Acc, Acc).

vypis1([], Acc, Acc).
vypis1([Xs | Xss], Acc, Out) :-
    foldr(hodnoty, Xs, [], NoveHodnoty),
    foldr(synove, Xs, [], Synove),
    Synove = [_ | _], !,
    append(Acc, [NoveHodnoty], Bcc),
    append(Xss, [Synove], Yss),
    vypis1(Yss, Bcc, Out).

vypis1([Xs | Xss], Acc, Out) :-
    foldr(hodnoty, Xs, [], NoveHodnoty),
    append(Acc, [NoveHodnoty], Bcc),
    vypis1(Xss, Bcc, Out).
