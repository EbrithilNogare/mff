% 2. domácí úloha
%
% a) Implementujte predikát flat(+List, ?Result), který zploští libovolně
% zanořený seznam seznamů List.
%
% flat([], R).
% R = [].
%
% flat([[]], R).
% R = [].
%
% flat([a,b,c], R).
% R = [a,b,c].
%
% flat([a,[[],b,[]],[c,[d]]], R).
% R = [a,b,c,d].
%
% Tento predikát měl být deterministický (speciálně otestujte, že po odmítnutí
% neprodukuje duplikátní/nesprávné výsledky). Pokuste se o efektivní
% implementaci pomocí akumulátoru.

flat([], []).
flat([Head | Body], Result) :-
	flat(Head, FlatedHead),
	flat(Body, FlatedBody),
	push(FlatedHead, FlatedBody, Result),
	!.
flat(NotArray, [NotArray]).

push([], X, X).
push(X, [], X).
push([L | M], R, [L | MR]) :- push( M, R, MR).


% b) Implementuje predikát transp(+M, ?R), který transponuje matici M (uloženou
% jako seznam seznamů). Pokud M není ve správném formátu (např. řádky mají
% různé délky), dotaz transp(M, R) by měl selhat.
%
% transp([], R).
% R = [].
%
% transp([[],[],[]], R).
% R = [].
%
% transp([[a,b],[c,d],[e,f]], R).
% R = [[a,c,e],[b,d,f]].
%
% transp([[a],[b,c],[d]], R).
% false.

transp(Empty, []) :-
	flat(Empty, []). % testuje jestli je cely sloupec prazdny
transp(Matrix, [Row | Rows]) :-
	getColumn(Matrix, Row, RestMatrix),
	transp(RestMatrix, Rows).

getColumn([], [], []).
getColumn([[Head | Tail] | Rows], [Head | HeadRest], [Tail | TailRest]) :-
	getColumn(Rows, HeadRest, TailRest).


% c) (BONUSOVÁ ÚLOHA) Implementuje vkládání prvku pro AVL stromy.
%
% Použijte následující reprezentaci:
% prázdný strom: nil
% uzel: t(B,L,X,R) kde
%   L je levý podstrom,
%   X je uložený prvek,
%   R je pravý podstrom,
%   B je informace o vyvážení:
%     B = l (levý podstrom je o 1 hlubší)
%     B = 0 (oba podstromy jsou stejně hluboké)
%     B = r (pravý podstrom je o 1 hlubší)
%
% avlInsert(+X, +T, -R)
% X je vkládané číslo, T je strom před přidáním, R je strom po přidání
%
% avlInsert(1, nil, R).
% R = t(0, nil, 1, nil).
%
% avlInsert(2, t(0, nil, 1, nil), R).
% R = t(r, nil, 1, t(0, nil, 2, nil)).
%
% avlInsert(1, t(0, nil, 1, nil), R).
% R = t(0, nil, 1, nil).

test :-
	flat([], []),
	flat([[]], []),
	flat([a,b,c], [a,b,c]),
	flat([a,[[],b,[]],[c,[d]]], [a,b,c,d]),

	transp([], []),
	transp([[],[],[]], []),
	transp([[a,b],[c,d],[e,f]], [[a,c,e],[b,d,f]]),
	\+transp([[a],[b,c],[d]], _),
	!.
