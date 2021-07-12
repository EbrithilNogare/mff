?- consult('../functional.pl').
?- consult('../mnozinove_operace.pl').

%nasel jsem na foru 3 variace

%Sestavte predikát termy/1, který postupně vrací termy složené z funktorů bin/2, un/1 a const/0. 
%Výstupem bude tedy korektně sestavený term. Predikát by měl postupně vrátit všechna řešení, sice v libovolném pořadí, ovšem každé právě jednou.

obalUnarne(Term, UnarneObaleny) :- UnarneObaleny =.. [un, Term].
obalBinarne(T1-T2, BinarneObaleny) :- BinarneObaleny =..[bin, T1, T2].

termy_hloubka(0, [const]) :- !.
termy_hloubka(N, TermyN) :-
    N > 0,
    M is N - 1,
    termy_hloubka(M, TermyM),
    map(obalUnarne, TermyM, UnarneObalene),
    cartesian_product(TermyM, TermyM, Termy),
    map(obalBinarne, Termy, BinarneObalene),
    append(UnarneObalene, BinarneObalene, TermyN).


termy(V) :- termy(0, V).

termy(N, V) :-
    termy_hloubka(N, Termy),
    member(V, Termy).

termy(N, V) :-
    M is N + 1,
    termy(M, V).









%Napište predikát termy(+N, -V), který postupně skrze V vydá všechny možnosti, 
%jak lze z právě N funktorů poskládat smysluplný výraz. Každý funktor je buď 
%binární plus/2 nebo unární minus/1 anebo nulární p/0. Na pořadí možností ve výstupu nezáleží. 

termy2(1, p) :- !.
termy2(2, minus(p)) :- !.
termy2(N, plus(F,G)) :-
    N > 2,
    B is N-2,
    C is N-1,
    between(1,B, X),
    D is C - X,
    termy2(X, F),
    termy2(D, G).

termy2(N, minus(E)) :-
    N > 2,
    A is N-1,
    termy2(A, E).














%Prolog: Generování výrokových formulí (5 bodů)
%Formule výrokového počtu jsou sestavené z (výrokových) proměnných ve funktoru var/1 a logických spojek negace, konjunkce a disjunkce (bez konstant). 
%Dále máte dány v argumentech predikátu gen/3 číslo k pro velikost formule a seznam jmen proměnných. Generujte backtrackingem všechny logické formule 
%(každou jednou), které obsahují proměnné ze seznamu a ve kterých je počet spojek a výskytů proměnných dohromady právě k.

termy3(1, Jmena, var(J)) :- !, member(J, Jmena).
termy3(2, Jmena, not(P)) :- !, termy3(1, Jmena, P).
termy3(N, Jmena, V) :-
    N > 2,
    B is N-2,
    C is N-1,
    between(1,B,X),
    D is C - X,
    termy3(X, Jmena, P),
    termy3(D, Jmena, F),
    ( V = and(F,P) ; V = or(F,P) ).

termy3(N, Jmena, not(E)) :-
    N > 2,
    A is N-1,
    termy3(A, Jmena, E).