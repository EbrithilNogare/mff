% 1. Prolog: Izomorfismus bin. stromů s popisem (3 podotázky)
% Jsou zadány dva binární (zakořeněné) stromy S a T s ohodnocenými vrcholy, přičemž ohodnocení vrcholů se může opakovat. Definujte predikát iso/3, který zjistí, zdali jsou tyto stromy  isomorfní a vydá popis transformace. Volání je iso(+S,+T, -Popis), kde ve třetím argumentu bude popis. Popis je strom stejného tvaru jako S a ve vrcholech má boolovské hodnoty true a false. Hodnota true ve vrcholu znamená, že se děti vrcholu v S mají přehodit, abychom dostali T.
% Dva binární stromy jsou isomofní, pokud lze jeden získat z druhého permutací dětí libovolných vrcholů stromu, tj. vyměněním nebo nevyměněním podstromů vrcholu.  
% (a)  Navrhněte reprezentaci binárního (zakořeněného) stromu s ohodnocenými vrcholy v jazyce Prolog. Vaši reprezentaci ukažte na příkladě.
% (b) Definujte predikát iso/3.
% (c) Je některý z predikátů, které ve vašem řešení používáte (ať už vámi definovaných či knihovních), nedeterministický? Je predikát iso/3 nedeterministický? Lze ho zdeterminičtit (a jak?), pokud nám stačí nejvýš jedno řešení?
% Příklad:
%   S= d       ,     T=  d       , Popis=   t
%    /- --\            /- --\             /- --\
%   b      e          e      b           f      t
%  / \    / \        / \    / \         / \    / \
% a   c  f   g      g   f  a   c       f   f  f   f


% a) popis stromu: t(leftSubtree, parent, rightSubtree)
% priklad viz testovaci data

% b)
% iso(+S, +T, -Popis)
iso(nil, nil, nil).
iso(t(L1,P,R1), t(L2,P,R2), t(L3,P3,R3)) :-
	stejneDeti(L1,L2),
	stejneDeti(R1,R2),
	!,P3 = "f",
	iso(L1, L2, L3),
	iso(R1, R2, R3).
iso(t(L1,P1,R1), t(L2,P1,R2), t(L3,P3,R3)) :-
	stejneDeti(L1,R2),
	stejneDeti(R1,L2),
	!,P3 = "t",
	iso(R1, L2, R3),
	iso(L1, R2, L3).

stejneDeti(nil, nil).
stejneDeti(t(_,P1,_), t(_,P1,_)).


% S=  d       ,     T=  d       , Popis=   t
%    / \               / \                / \
%   b   e             e   b              f   t
%  /\  /\            /\  /\             /\  /\
% a c f g           g f a c            f f f f

grafS(S) :- S = t(t(t(nil,"a",nil),"b",t(nil,"c",nil)),"d",t(t(nil,"f",nil),"e",t(nil,"g",nil))).
grafT(T) :- T = t(t(t(nil,"g",nil),"e",t(nil,"f",nil)),"d",t(t(nil,"a",nil),"b",t(nil,"c",nil))).
test(Popis) :- 
	grafS(S),
	grafT(T),
	iso(S,T,Popis).

% c) je nedeterministicky, protoze muze nastat situace, ze levy i pravy podstrom budou mit unifikovatelne deti 
% neboli bude platit stejneDeti(L1,L2) & stejneDeti(R1,R2) & stejneDeti(L1,R2) & stejneDeti(L1,R2)


% KomentářeKomentář:
% - c) používáte řez, tak klauzule i predikát iso jsou deterministické
% - v důsledku řezu program nenajde všechna řešení. Například za podmínek popsaných na konci c), když L1 a L2 nejsou isomorfní.

% body: 7/10
