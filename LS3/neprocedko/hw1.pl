% 1. domácí úloha
%
% a) Implementujte logaritmus o základu 2 (dolní celou část) na unárně
% reprezentovaných číslech.
%
% logtwo(+N, ?Vysledek)
%
% Nápověda: Může se vám hodit pomocný predikát pro půlení.
%
% logtwo(0, R).
% false.
%
% logtwo(s(s(s(0))), R).
% R = s(0).
%
% logtwo(s(s(s(s(0)))), R).
% R = s(s(0)).
%
% b) Implementujte predikát, který spočte n-té Fibonacciho číslo lépe než
% v exponenciálním čase (ideálně pouze lineárně mnoho sčítání).
%
% fib(+N, ?Vysledek)
%
% Nápověda: Zkuste nejdřív implementovat obecnější predikát, kde si můžete
% zvolit počáteční čísla.
%
% F_0 = 4
% F_1 = 5
% F_2 = 4 + 5 = 9
% F_3 = 5 + 9 = 14
%
% generalizedFib(3, 4, 5, R).
% R = 14.
%
%
% c) (BONUSOVÁ ÚLOHA) Implementuje predikát pro sčítání dvou binárních čísel.
%
% Můžete použít např. následující reprezentaci:
%
% 13[dec] = 1101[bin] = b(1, b(0, b(1, b(1, e))))
%
% Příklad použití:
% addBin(b(1, b(0, b(1, e))), b(1, b(1, b(0, b(1, e)))), R).
% R = b(0, b(0, b(0, b(0, b(1, e))))).
%
% resp.
%
% addBin([1, 0, 1], [1, 1, 0, 1], R).
% R = [0, 0, 0, 0, 1].


toNat(N, R) :-
	integer(N),
	toNat_(N, R).
  
  toNat_(N, R) :- N > 0 ->
	(N2 is N - 1, toNat_(N2, R2), R = s(R2));
	R = 0.
  
fromNat(0, 0).
fromNat(s(N), R) :-
	fromNat(N, R2),
	R is R2 + 1.
  
nat(0).
  nat(s(X)) :- nat(X).
  
add(0, Y, Y) :- nat(Y).
add(s(X), Y, s(Z)) :-
	add(X, Y, Z).
  
less(0, s(Y)) :- nat(Y).
less(s(X), s(Y)) :- less(X, Y).

%%% reseni

%% logaritmus

half(N, V) :-
	add(V, V, N);
	add(s(V), s(V), s(N)).

logtwo(s(0), 0).
logtwo(s(N), s(K)) :-
	half(s(N),M),
	logtwo(M,K).    


%% fibonaci

generalizedFib(0, A, _, A).
generalizedFib(s(I), A, B, R) :-
	add(A,B,AB),
	generalizedFib(I,B,AB,R).

fib(N, Fib) :-
	toNat(N,I),
	generalizedFib(I,0,s(0),R),
	fromNat(R,Fib).


%% binary addition

% alias
addBin(A, B, R) :- addBin(A, B, R, 0).

% array decomposition
addBin([], [], [], 0).
addBin([], [B|BX], [B|BX], 0).
addBin([A|AX], [], [A|AX], 0).
addBin([A|AX], [B|BX], [R|RX], 0) :- addBinComb(A, B, R, AX, BX, RX, 0).
addBin([], [], [1], 1).
addBin([], [B|BX], RX, 1) :- addBin([1], [B|BX], RX).
addBin([A|AX], [], RX, 1) :- addBin([A|AX], [1], RX).
addBin([A|AX], [B|BX], [R|RX], 1) :- addBinComb(A, B, R, AX, BX, RX, 1).

% bin adder
% addBinComb(A, B, C, _, _, _, D) ~ C = A + B + D
addBinComb(0, 0, 0, AX, BX, RX, 0) :- addBin(AX, BX, RX, 0).
addBinComb(0, 1, 1, AX, BX, RX, 0) :- addBin(AX, BX, RX, 0).
addBinComb(1, 0, 1, AX, BX, RX, 0) :- addBin(AX, BX, RX, 0).
addBinComb(1, 1, 0, AX, BX, RX, 0) :- addBin(AX, BX, RX, 1).
addBinComb(0, 0, 1, AX, BX, RX, 1) :- addBin(AX, BX, RX, 0).
addBinComb(0, 1, 0, AX, BX, RX, 1) :- addBin(AX, BX, RX, 1).
addBinComb(1, 0, 0, AX, BX, RX, 1) :- addBin(AX, BX, RX, 1).
addBinComb(1, 1, 1, AX, BX, RX, 1) :- addBin(AX, BX, RX, 1).


%% tests

tests() :-
	toNat(3, K3),
	toNat(4, K4),
	toNat(5, K5),
	toNat(14, K14),
	generalizedFib(K3, K4, K5, K14),
	\+logtwo(0, _),
	logtwo(s(s(s(0))), s(0)),
	logtwo(s(0), 0),
	fib(1,1),
	fib(2,1),
	fib(3,2),
	fib(4,3),
	fib(5,5),
	fib(6,8),
	addBin([0], [0], [0]),
	addBin([1], [1], [0, 1]),
	addBin([0], [1, 1, 1, 1], [1, 1, 1, 1]),
	addBin([1, 0, 1], [1, 1, 0, 1], [0, 0, 0, 0, 1]),
	!.
