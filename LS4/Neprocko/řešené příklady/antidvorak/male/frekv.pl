?- consult('../functional.pl').

%http://forum.matfyz.info/viewtopic.php?f=169&t=11756&p=41531&hilit=frekv#p41531

rozdel(Xs, Sude, Liche) :- sude(Xs, Sude, Liche).

sude([], [], []).
sude([X | Xs], [X | Sude], Liche) :- liche(Xs, Sude, Liche).

liche([], [], []).
liche([X | Xs], Sude, [X | Liche]) :- sude(Xs, Sude, Liche).

vytvor_dvojce([X1, X2 | Xs], [ X1-X2 | Zbytek ]) :- !, vytvor_dvojce([X2 | Xs], Zbytek).
vytvor_dvojce([], []).
vytvor_dvojce([_], []).

vyskyt(X, L) :- X =.. [f, _, L].

slitSudy([], L, _, Accu, Freq) :- append(Accu, L, Freq), !.
slitSudy(S, L, Kolik, Accu, Freq) :-
    take(Kolik, S, Prvky),
    drop(Kolik, S, S2),
    append(Accu, Prvky, NovyAccu),
    last(Prvky, Posledni),
    vyskyt(Posledni, Kolik2),
    slitLichy(S2, L, Kolik2, NovyAccu, Freq).

slitLichy(S, [], _, Accu, Freq) :- append(Accu, S, Freq), !.
slitLichy(S, L, Kolik, Accu, Freq) :-
    take(Kolik, L, Prvky),
    drop(Kolik, L, L2),
    append(Accu, Prvky, NovyAccu),
    last(Prvky, Posledni),
    vyskyt(Posledni, Kolik2),
    slitSudy(S, L2, Kolik2, NovyAccu, Freq).


slit(S, L, Freq) :-
    slitSudy(S, L, 1, [], Freq).

frekv(Cs, Freq) :- 
    vytvor_dvojce(Cs, Dvojce),
    rozdel(Dvojce, Sude, Liche),
    group(Sude, SudeGrouped),
    group(Liche, LicheGrouped),
    slit(SudeGrouped, LicheGrouped, Freq).
    
