?- consult('diff_seznamy.pl').

%mega knihovna veci, ktery jsem porad kolem dokola psal
%a nebavilo me je psat

group(Xs, Ys) :- group(Xs, A-A, Ys).
group([],Acc-A,Acc) :- A = [].
group([X | Xs], Acc-A, Ys) :- 
    takeWhile(equal(X), Xs, Z),
    length(Z, L),
    K is L + 1,
    drop(L, Xs, Zs),
    W =.. [f,X,K],
    push_back(W, Acc-A, NewAcc-B),
    group(Zs, NewAcc-B, Ys).



groupBy(Binary, Xs, Ys) :-
    groupBy(Binary, Xs, [], YsR),
    reverse(YsR, Ys).

holds(Binary, X, Y) :- call(Binary, X, Y).

groupBy(_, [], Acc, Acc) :- !.
groupBy(Binary, [X | Xs], Acc, Out) :-
    takeWhile(holds(Binary, X), Xs, Ys),
    length(Ys, L),
    drop(L, Xs, Rest),
    groupBy(Binary, Rest, [[X | Ys] | Acc], Out).



groupBy1(GPred, TPred, Xs, Ys) :- 
    map(GPred, Xs, X),
    map(TPred, Xs, Y),
    groupBy1Impl(X, Y, [], Ys).

groupBy1Impl([], [], Acc, Acc).
groupBy1Impl([X | Xs], Ys, Acc, Out) :-
    takeWhile(equal(X), Xs, Z),
    length(Z, L),
    K is L + 1,
    drop(L, Xs, Zs),
    take(K, Ys, Ws),
    drop(K, Ys, NewYs),
    append(Acc, [Ws], NewAcc),
    groupBy1Impl(Zs, NewYs, NewAcc, Out).


id(X, X).

take(0, [_ | _], []) :- !.
take(_, [], []) :- !.
take(N, [X | Xs], [X | Zs]) :- N > 0, M is N-1, take(M, Xs, Zs).

drop(0, Xs, Xs) :- !.
drop(N, [], []) :- N > 0, !.
drop(N, [_ | Xs], Zs) :- N > 0, M is N-1, drop(M, Xs, Zs).


takeWhile(Pred, [X | _], []) :- \+ call(Pred, X), !.
takeWhile(_, [], []) :- !.
takeWhile(Pred, [X | Xs], [X | Ys]) :- 
    call(Pred, X),
    takeWhile(Pred, Xs, Ys).

pushFront(X, Xs, [X | Xs]).

map(_, [], []) :- !.
map(Unary, [X | Xs], [Y | Ys]) :-
    call(Unary, X, Y),
    map(Unary, Xs, Ys).

foldl(_, [], Y, Y) :- !.
foldl(Binary, [X | Xs], Acc, Y) :-
    call(Binary, X, Acc, NewAcc),
    foldl(Binary, Xs, NewAcc, Y).

foldl1(Binary, [X | Xs], Y) :- 
    foldl(Binary, Xs, X, Y).

%foldr - akumulator je vpravo
foldr(_, [], Acc, Acc) :- !.
foldr(Binary, [X | Xs], Acc, Y) :-
    foldr(Binary, Xs, Acc, Z),
    call(Binary, X, Z, Y).

foldr1(_, [X], X) :- !.
foldr1(Binary, [X | Xs], Y) :-     
    foldr1(Binary, Xs, Z),
    call(Binary, X, Z, Y).

scanl(Binary, Xs, Acc, Accs) :- scanlImpl(Binary, Xs, [Acc], Accs).

scanlImpl(_, [], Accs, Accs) :- !.
scanlImpl(Binary, [X | Xs], [A | Acc], Accs) :- 
    call(Binary, X, A, NewA),
    scanlImpl(Binary, Xs, [NewA, A | Acc], Accs).

scanl1(Binary, [X | Xs], Out) :- scanl(Binary, Xs, X, Out).


scanr(Binary, Xs, Acc, Accs) :- scanrImpl(Binary, Xs, [Acc], Accs).

scanrImpl(_, [], Acc, Acc) :- !.
scanrImpl(Binary, [X | Xs], Acc, [B | O]) :-
    scanrImpl(Binary, Xs, Acc, O),
    O = [A | _],
    call(Binary, X, A, B).

scanr1(_, [X], [X]) :- !.
scanr1(Binary, [X | Xs], [K | Z]) :- 
    scanr1(Binary, Xs, Z),
    Z = [A | _],
    call(Binary, X, A, K).

plus(X, Y, Z) :- Z is X + Y.
minus(X, Y, Z) :- Z is X - Y.
multiply(X, Y, Z) :- Z is X * Y.
divide(X, Y, Z) :- Z is X / Y.

min(X, Y, X) :- X < Y, !.
min(_, Y, Y).

max(X, Y, X) :- X > Y, !.
max(_, Y, Y).

positive(X) :- X > 0.

even(X) :- Y is mod(X,2), Y == 0.
odd(X) :- \+ even(X).

gcd(X, 0, X) :- !.
gcd(X, Y, O) :- Y =\= 0, Z is mod(X,Y), gcd(Y, Z, O).

lcm(_,0,0).
lcm(X, Y, O) :- Y =\= 0, Z is X * Y, abs(Z,AZ), gcd(X,Y,W), O is AZ / W.

inc(X, Y) :- Y is X+1.
dec(X, Y) :- Y is X-1.

filter(_, [], []) :- !.
filter(Pred, [X | Xs], [X | Ys]) :- 
    call(Pred, X), 
    !, 
    filter(Pred, Xs, Ys).

filter(Pred, [_ | Xs], Ys) :- filter(Pred, Xs, Ys).

unique([],[]).
unique([X | Xs], [X | Ys]) :- 
    filter(notEqual(X), Xs, Z),
    unique(Z, Ys).

head([H | _], H).
tail([_ | T], T).

%init - remove last element
init(Xs, Ys) :-
    length(Xs, L),
    M is L - 1,
    take(M, Xs, Ys).

% list of [X, X, ..., X] n times
replicate(0, _, []) :- !.
replicate(N, X, [X | Ys]) :- 
    N > 0,
    M is N-1,
    replicate(M,X,Ys).

zip([], _, []) :- !.
zip(_, [], []) :- !.
zip([X | Xs], [Y | Ys], [X-Y | Zs]) :- zip(Xs, Ys, Zs).


unzip([], [], []).
unzip([X | Xs], [Y | Ys], [Z | Zs]) :-
    X =.. [_, Y, Z],
    unzip(Xs, Ys, Zs).


zipSep([], _, _, []).
zipSep(_, [], _, []).
zipSep([X | Xs], [Y | Ys], Sep, [Z | Zs]) :-
    Z =.. [Sep, X, Y],
    zipSep(Xs, Ys, Sep, Zs).


zipWith(_, [], _, []) :- !.
zipWith(_, _, [], []) :- !.
zipWith(Binary, [X | Xs], [Y | Ys], [Z | Zs]) :- 
    call(Binary, X, Y, Z),
    zipWith(Binary, Xs, Ys, Zs).
    
sum([], 0).
sum(Xs, Z) :- foldl1(plus, Xs, Z).

product([],1).
product(Xs, Z) :- foldl1(multiply, Xs, Z).

sequence(F,_,T, []) :- F > T, !.
sequence(F,S,T, [X | Xs]) :- 
    F =< T,
    X is F,
    NF is F + S,
    sequence(NF,S,T,Xs).

all(_, []) :- !.
all(Pred, [X | Xs]) :-
    call(Pred, X),
    all(Pred, Xs).

any(Pred, Xs) :-
    member(X, Xs),
    call(Pred, X),
    !.

findIndex(Pred, Xs, I) :- findIndex(Pred, Xs, 0, I).
findIndex(Pred, [X | _], J, J) :- call(Pred, X).
findIndex(Pred, [_ | Xs], J, I) :- K is J+1, findIndex(Pred, Xs, K, I).

% findIndex(equal(3), [1,2,3,4,5,6,3], I).
unify(A, B) :- A = B.
notUnify(A, B) :- A \= B.
notEqual(X, Y) :- \+ equal(X,Y).
equal(X, Y) :- Y == X.
greater(X, Y) :- Y > X.
greaterOrEqual(X, Y) :- Y >= X.
less(X, Y) :- Y < X.
lessOrEqual(X, Y) :- Y =< X.

maxElement(Xs, X) :- foldl1(max, Xs, X).
minElement(Xs, X) :- foldl1(min, Xs, X).

maxElementIndex(Xs, I) :-
    maxElement(Xs, MaxVal),
    indexOfValue(Xs, MaxVal, I).

minElementIndex(Xs, I) :-
    minElement(Xs, MinValue),
    indexOfValue(Xs, MinValue, I).

indexOfValue(Xs, X, J) :- indexOfValue(Xs, X, 0, J).
indexOfValue([X | _], X, I, I).
indexOfValue([_ | Xs], Y, J, I) :-
    K is J + 1,
    indexOfValue(Xs, Y, K, I).

%delete at specified index
deleteAt(0, [_ | Xs], Xs) :- !.
deleteAt(I, [X | Xs], [X | Ys]) :- 
    I > 0,
    J is I-1,
    deleteAt(J, Xs, Ys).

deleteFirst(_, [], []) :- !.
deleteFirst(X, [Y | Ys], [Y | Zs]) :-
    X \= Y,!,
    deleteFirst(X, Ys, Zs).

deleteFirst(X, [X | Ys], Ys) :- !.

deleteAll(X, Ys, Zs) :- filter(notEqual(X), Ys, Zs).


%put at specified index
putAt(0, Y, [_ | Xs], [Y | Xs]) :- !.
putAt(I, Y, [X | Xs], [X | Ys]) :-
    I > 0,
    J is I-1,
    putAt(J, Y, Xs, Ys).

%insert at index value X
insertAt(I, X, Xs, NewXs) :-
    length(Xs, L),
    K is L - 1,
    I >= 0,
    I =< K,
    insertAtImpl(I, X, Xs, NewXs).

insertAt(I, X, Xs, NewXs) :-
    length(Xs, L),
    I == L,
    !,
    append(Xs, [X], NewXs).

insertAtImpl(0, X, Xs, [X | Xs]) :- !.
insertAtImpl(I, X, [Y | Xs], [Y | Ys]) :-
    I > 0,
    J is I - 1,
    insertAtImpl(J, X, Xs, Ys).

index(0, [X | _], X) :- !.
index(I, [_ | Xs], Y) :- J is I - 1, index(J, Xs, Y).

%swap x[i] with y[j]
swap(I, Xs, J, Ys, XsNew, YsNew) :-
    index(I, Xs, X),
    index(J, Ys, Y),
    putAt(I, Y, Xs, XsNew),
    putAt(J, X, Ys, YsNew).


%replace all occurence of A in Xs by B
replaceAll(_,[],_,[]) :- !.
replaceAll(A, [A | Xs], B, [B | Ys]) :- !, replaceAll(A, Xs, B, Ys).
replaceAll(A, [X | Xs], B, [X | Ys]) :- 
    X =\= A,
    replaceAll(A, Xs, B, Ys).

replaceFirst(A, [A | Xs], B, [B | Xs]) :- !.
replaceFirst(A, [X | Xs], B, [X | Ys]) :- 
    X =\= A,
    replaceFirst(A, Xs, B, Ys).

%is N multiple of M
%is N divisible by M
isMultipleOf(M, N) :- R is mod(N, M), R == 0.
isNotMultipleOf(M, N) :- \+ isMultipleOf(M, N).

%Xs contains all primes from interval [2..N]
eratosthenes(N, Xs) :- 
    N >= 2,
    sequence(2, 1, N, Seq),
    removeMultiples(Seq, [], XsR),
    reverse(XsR,Xs).

removeMultiples([], Out, Out).
removeMultiples([P | Nums], Acc, Out) :-
    NewAcc = [P | Acc],
    filter(isNotMultipleOf(P), Nums, PossiblePrimes),
    removeMultiples(PossiblePrimes, NewAcc, Out).

prime(N) :- 
    eratosthenes(N, Primes),
    member(N, Primes).

primeDecomposition(N, Decomposition) :-
    eratosthenes(N, Primes),
    decompose(N, Primes, [], Decomposition),
    !.

decompose(1, _, DecompositionR, Decomposition) :- length(DecompositionR, L), L > 1, !, reverse(DecompositionR, Decomposition).
decompose(N, [P | Primes], Acc, Decomposition) :- 
    \+ isMultipleOf(P, N),
    decompose(N, Primes, Acc, Decomposition).

decompose(N, [P | Primes], Acc, Decomposition) :-
    isMultipleOf(P, N),
    NewAcc = [P | Acc],
    M is N / P,
    decompose(M, [P | Primes], NewAcc, Decomposition).

fst(X, Y) :- X =.. [_, Y, _].
snd(X, Y) :- X =.. [_, _, Y].

transpose([[] | _], []) :- !.
transpose([],[]) :- !.
transpose(M, [R | T1]) :-
    map(head, M, R),
    map(tail, M, S),
    transpose(S, T1).

qsort([],[]).
qsort([X | Xs], Ws) :-
    filter(lessOrEqual(X), Xs, Ys),
    filter(greater(X), Xs, Zs),
    qsort(Ys, YsS),
    qsort(Zs, ZsS),
    append(YsS, [X | ZsS], Ws).

cmp(Cmp,X,Y) :- call(Cmp,X,Y).
cmpN(Cmp,X,Y) :- \+ call(Cmp,X,Y).

qsortBy(_, [], []).
qsortBy(Cmp, [X | Xs], Ws) :-
    filter(cmpN(Cmp,X), Xs, Ys),
    filter(cmp(Cmp,X), Xs, Zs),
    qsortBy(Cmp, Ys, YsS),
    qsortBy(Cmp, Zs, ZsS), !,
    append(YsS, [X | ZsS], Ws).

concat([],[]).
concat([Xs | Xss], Ys) :-
    concat(Xss, Zs),
    append(Xs, Zs, Ys).


m(Pred, X, Acc, X) :- call(Pred, X, Acc), !.
m(_, _, Acc, Acc).
maximumBy(Pred, Xs, X) :- foldl1(m(Pred),Xs,X).