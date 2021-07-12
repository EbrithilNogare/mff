?- consult('../functional.pl').

vstup(V) :- V = [
    [ jmeno-"xy", vek-30, vaha-90],
    [ jmeno-"xyz", vek-35, vaha-80],
    [ jmeno-"ab",vaha-80]
   ].

%http://forum.matfyz.info/viewtopic.php?f=169&t=11969&p=41802&hilit=objekt#p41802

objekt2nazvy(O, N) :- map(fst, O, N).

pridejNazvy(O, Acc, NewAcc) :-
    objekt2nazvy(O, N),
    append(Acc, N, NewAcc).

objekty2nazvy(Os, Ns) :-
    foldl(pridejNazvy, Os, [], Nd),
    unique(Nd, Ns).

vyndej(Name, A-_, Acc, Acc) :- Name \= A, !.
vyndej(Name, Name-B, Acc, [B | Acc]).

%fails if there are more than one value for one attribute name
getAttributeValue(O, Name, Value) :-
    foldl(vyndej(Name),O,[], Values),
    length(Values, L),
    (L == 1 ->
        Values = [Value] ;
    (L == 0 ->
        Value = undefined ;

    %else
        fail
    )
    ).


p(Name, O, Acc, [Value|Acc]) :- getAttributeValue(O, Name, Value).

getAttributeValues(Os, Name, Values) :- foldr(p(Name),Os, [], Values).

dvojce(Objekty, Nazev, Nazev-Hodnoty) :- 
    getAttributeValues(Objekty, Nazev, Vals),
    unique(Vals, Hodnoty).

%%%%%%%%%%%%
hodnoty(V, Y) :-
    objekty2nazvy(V, Ns),
    map(dvojce(V),Ns,Y).