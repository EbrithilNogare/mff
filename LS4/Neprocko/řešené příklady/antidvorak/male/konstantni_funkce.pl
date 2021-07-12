?- consult('../functional.pl').

%http://forum.matfyz.info/viewtopic.php?f=169&t=10961&p=40530&hilit=funkce#p40530

addIntervals([], _, Acc, Out) :- reverse(Acc, Out).

addIntervals([U-Value | Xs], LBound, Acc, FunctionWithIntervals) :-
    UBound is LBound + U,
    addIntervals(Xs, UBound, [LBound-UBound-Value | Acc], FunctionWithIntervals).


addIntervals(Function, FunctionWithIntervals) :-
    addIntervals(Function, 0, [], FunctionWithIntervals).

najdi(Min-Max, F, Acc, H) :-
    member(Mi-Ma-H,F),
    Min >= Mi,
    Max =< Ma,
    H > Acc,
    !.
najdi(_, _, Acc, Acc).

najdiMax(Min-Max, Funkce, M) :-
    foldl(najdi(Min-Max), Funkce, 0, M).

jeObsazen(Min-Max, Funkce) :-
    member(F, Funkce),
    member(Mi-Ma-_, F),
    Min >= Mi,
    Max =< Ma.

vyrobFunkci(Min-Max, Funkce, FAcc, Out) :-
    jeObsazen(Min-Max, Funkce), !,
    najdiMax(Min-Max, Funkce, M),
    NovyMax is Min + 2,
    vyrobFunkci(Max-NovyMax, Funkce, [Min-Max-M | FAcc], Out).

vyrobFunkci(_, _, FAcc, Out) :- reverse(FAcc, Out).

stejnaHodnota(_-_-C, _-_-F) :- C == F.

delka(Z-K-_, Acc, NewAcc) :- D is K-Z, NewAcc is Acc + D.

seskup(Usek, Delka-Hodnota) :-
    Usek = [_-_-Hodnota | _],
    foldl(delka, Usek, 0, Delka).

funkce(Fs, Hledana) :-
    map(addIntervals, Fs, Funkce),
    vyrobFunkci(0-1, Funkce, [], F),
    groupBy(stejnaHodnota, F, Grouped),
    map(seskup, Grouped, Hledana).