% 2. Prolog: Koncepty
% Jeden objekt je zadán uspořádaným seznamem dvojic klíč-hodnota. Na vstupu máte seznam objektů. Napište proceduru koncept/2, která vyrobí nejmenší koncept zahrnující všechny vstupní objekty. Koncept je seznam dvojic klíč-seznam_hodnot. Koncept zahrnuje objekt, pokud koncept má všechny klíče objektu a v seznamu hodnot příslušného klíče u konceptu je obsažena hodnota klíče u objektu. Pokud objekt nějaký klíč konceptu nemá, bude v seznamu hodnot konceptu hodnota nedef.
% Příklad: 
% ?- koncept([[barva-modra, motor-diesel, pocet_kol-6],  % TIR
% 
%             [barva-bila, motor-plyn, pocet_mist-40],   % bus
% 
%             [motor-elektro, pocet_mist-5] ],  Koncept). % osobni
% 
% Koncept = [barva-[modra,bila,nedef], motor-[diesel,plyn,elektro], pocet_kol-[6,nedef], pocet_mist-[40,5,nedef]]


klicPridej([K-D|Xy], K, V, [K-DV|Xy]) :- append(D, [V], DV), !.
klicPridej([L|Xy], K, V, [L|R]) :- klicPridej(Xy, K, V, R).
klicPridej([], K, V, [K-[V]]) :- !.

koncept(In, Out) :- koncept(In, [], Out).
koncept([], Aku, Aku).
koncept([X|Xs], R, Aku) :- koncept_stav(X, R, NR), koncept(Xs, NR, Aku).
koncept_stav([], Aku, Aku).
koncept_stav([K-H|Xs], R, Aku) :- klicPridej(R, K, H, NR), koncept_stav(Xs, NR, Aku).

test(Koncept) :-
	koncept([[barva-modra, motor-diesel, pocet_kol-6],  % TIR
             [barva-bila, motor-plyn, pocet_mist-40],   % bus
             [motor-elektro, pocet_mist-5] ],  Koncept). % osobni
