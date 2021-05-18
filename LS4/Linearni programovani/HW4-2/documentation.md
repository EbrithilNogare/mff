# Dokumentace linearniho programu pro hledani nejmensiho poctu skupin, kde se kazdy zna s kazdym

autor: David Napravnik

## Generator a jeho implementace
Generator ze vstupu nacte grafove hrany a vygeneruje z nich komplement grafu.
Nasledne eliminuje hrany tvorici smycku na kedinem vrcholu.
Cimz problem prevedeme na problem obarvitelnosti.

Prvni optimalizace je, ze hrany jsou cteny pouze v jednom smeru,
tudiz vznikne orientovany graf a vymizi nam tudiz cca polovina promenych.

Jako druhou optimalizaci je nalezeni dvojic vrcholu, takovych ze vrchol *a*
dominuje vrcholu *b* a vrcholu *b* tedy priradime stejnou barvu jako ma vrchol *a*.
Neboli sousede vrcholu *b* jsou podmnozinou sousedu *a*.
Cimz eliminujeme male mnozstvi vrcholu, ale solveru velice usnadnime praci.

Dalsi optimalizace, jako predbarveni nejvetsi kliky neni mozne,
kvuli zpusobu pocitani poctu barev a zaroven bych opakovanim teto metody
dostal reseni uz v generatoru a solver LP by pouze zopakoval vysledek.


## Generator a jeho pouzivani
Generator ze standartniho vstupu precte na prvnim radku ve druhem slovu pocet hran grafu.
Na zbylych radcich (ukoncenych EOF) jsou hrany grafu, oddelene znackou "--"

Predpokladejme, ze soubor *input.txt* je spravne formatovany,
pak nam nasledujici prikaz vygeneruje datovy sobor pro LP.

```
cat input.txt | python .\HW4.py > coloring.data
```


## Vysledny linearni program
LP se resi pomoci metody ASS-S.
Vysledek se vypisuje ve formatu:
```
#OUTPUT: ${pocet barev}
v_${id vrcholu} : ${prirazena barva}
#OUTPUT END
```
LP je rozdelen na dve casti
- model - staticky soubor pro vsechny grafy s linearnim programem
- data - soubor generovany generatorem pro kazdy graf zvlast

## Testovani a benchmark
pro testovani je pripraven script pro powershell *run test.ps1*, ktery
nacte testy ze slozky tests a pro kazdy z nich spusti generator a
glpsol. Vysledek vypise na standartni vystup ve formatu
`${nazev testu}	${pocet barev}	${cas} s`.
na zacatku scriptu je mozne upravit maximalni cas,
po ktery ma test bezet a po teto dobe je zastaven a misto vysledku je
vepsan znak "?" a misto casu pak ">300" (nebo jine cislo, podle max casu).

## Reference
[1] [New Integer Linear Programming Models for the Vertex Coloring Problem](https://arxiv.org/pdf/1706.10191.pdf)
