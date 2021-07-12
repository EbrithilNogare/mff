%pitomej bruteforce
%http://forum.matfyz.info/viewtopic.php?f=169&t=11747

compatible(_-Y, Y-_).

configuration(X-X, X-X).
configuration(X-Y, X-Y) :- X \= Y.
configuration(X-Y, Y-X) :- X \= Y.

deleteDomino(D, Dominoes, AvailableDominoes) :-
    configuration(D, C),
    member(C, Dominoes),
    once(select(C, Dominoes, AvailableDominoes)).

appendOptions(Dominoes, [], Option) :-
    member(Domino, Dominoes),
    configuration(Domino, Option).

appendOptions(Dominoes, CurrentChain, Option) :-
    CurrentChain \= [],
    member(Domino, Dominoes),
    last(CurrentChain, LastDomino),
    configuration(Domino, Option),
    compatible(LastDomino, Option).

chain(Dominoes, CurrentChain, CurrentChain) :-
    \+ appendOptions(Dominoes, CurrentChain, _).

chain(Dominoes, CurrentChain, OutputChain) :-
    appendOptions(Dominoes, CurrentChain, D),
    deleteDomino(D, Dominoes, AvailableDominoes),
    append(CurrentChain, [D], NewChain),
    chain(AvailableDominoes, NewChain, OutputChain).

chain(Dominoes, OutputChain) :- chain(Dominoes, [], OutputChain).

chains(Dominoes, OutputChains) :- findall(OutputChain, chain(Dominoes, OutputChain), OutputChains).

areAvailablePieces([], Dominoes, Dominoes).

areAvailablePieces([C | Pieces], Dominoes, RemainingDominoes) :-
    configuration(C, D),
    member(D, Dominoes),
    once(select(D, Dominoes, Rd)),
    areAvailablePieces(Pieces, Rd, RemainingDominoes).


possibleChain(Dominoes, Chains, Chain, RemainingDominoes, RemainingChains) :-
    member(Chain, Chains),
    areAvailablePieces(Chain, Dominoes, RemainingDominoes),
    once(select(Chain, Chains, RemainingChains)).

chain_string([], _, ChainString, ChainString).

chain_string(Dominoes, Chains, BuildingString, ChainString) :-
    possibleChain(Dominoes, Chains, Chain, RemainingDominoes, RemainingChains),
    append(BuildingString, [Chain], NewBuildingString),
    chain_string(RemainingDominoes, RemainingChains, NewBuildingString, ChainString).

chain_string(Dominoes, ChainString) :- 
    chains(Dominoes, Chains),
    chain_string(Dominoes, Chains, [], ChainString).

chain_strings(Dominoes, ChainStrings) :- findall(ChainString, chain_string(Dominoes, ChainString), ChainStrings).

smallestLength([X | Xs], SmallestLength) :- 
    length(X, L),
    smallestLength(Xs, L, SmallestLength).

smallestLength([], SmallestLength, SmallestLength).

smallestLength([X | Xs], CurrentSmallest, SmallestLength) :-
    length(X, L),
    L =< CurrentSmallest,
    smallestLength(Xs, L, SmallestLength).

smallestLength([X | Xs], CurrentSmallest, SmallestLength) :-
    length(X, L),
    L > CurrentSmallest,
    smallestLength(Xs, CurrentSmallest, SmallestLength).

least_chain_string(Dominoes, ChainString) :-
    chain_strings(Dominoes, AvailableChains),
    smallestLength(AvailableChains, SmallestLength),
    member(ChainString, AvailableChains),
    length(ChainString, L),
    L = SmallestLength.
