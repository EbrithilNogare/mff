?- consult('../functional.pl').

vstup( t(t(t(nil,10,nil),25,t(t(nil,30,nil),35,t(nil,37,nil))),50,t(nil,75,t(t(nil,80,nil),100,nil))) ).

%http://forum.matfyz.info/viewtopic.php?f=169&t=10963&p=40532&hilit=doprava#p40532

compare(X-_,W-_) :- X < W.

rozdel(T, XX) :- 
    rozdel(T, 0, Y),
    qsortBy(compare, Y, X),
    groupBy1(fst,snd,X,XX).

rozdel(nil, _, []).
rozdel(t(L,V,R), N, Out) :-
    M is N+1,
    rozdel(R, M, O1), %prohozenim M a N dostaneme rozdeleni podle poctu kroku doleva
    rozdel(L, N, O2),
    append(O1, [N-V|O2], Out).