%nejake priklady z DCG
%jen tak

list_length(0) --> [].
list_length(L) --> [_], list_length(K), {L is K+1}.

sum(0) --> [].
sum(S) --> [X], {number(X)}, sum(T), {S is T + X}.

foldrImpl(_, Acc, Acc) --> [].
foldrImpl(Binary, Acc, Out) --> [X], foldrImpl(Binary, Acc, A), {call(Binary, X, A, Out)}.
foldr(Binary, Xs, Acc, Out) :- phrase(foldrImpl(Binary, Acc, Out), Xs).

foldlImpl(_, Acc, Acc) --> [].
foldlImpl(Binary, Acc, Out) --> [X], {call(Binary, X, Acc, A)}, foldlImpl(Binary, A, Out).
foldl(Binary, Xs, Acc, Out) :- phrase(foldlImpl(Binary, Acc, Out), Xs).

filterImpl(_, []) --> [].
filterImpl(Pred, [X | O]) --> [X], filterImpl(Pred, O), {call(Pred, X)}.
filterImpl(Pred, O) --> [X], filterImpl(Pred, O) , {\+ call(Pred, X)}.
filter(Pred, Xs, Ys) :- phrase(filterImpl(Pred, Ys), Xs).