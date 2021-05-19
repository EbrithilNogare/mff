# Dokumentace linearniho programu pro hledani nejmensiho poctu skupin, kde se kazdy zna s kazdym

autor: David Napravnik

## Generator a jeho implementace
Generator ze vstupu nacte grafove hrany a jejich vahy.

Nasledne eliminuje hrany ktere nejsou soucasti cyklu, nebot
u takovychto hran mame jistotu, ze je nebudeme v LP odebirat,
tak je muzeme odebrat rovnou.


## Generator a jeho pouzivani
Generator ze standartniho vstupu precte na prvnim radku ve tretim slove pocet hran grafu.
Na zbylych radcich (ukoncenych EOF) jsou hrany grafu, oddelene znackou "-->" a v zavorce maji vehu hrany.

Predpokladejme, ze soubor *input.txt* je spravne formatovany,
pak nam nasledujici prikaz vygeneruje datovy sobor pro LP.

```
cat input.txt | python .\decycler.py > decycler.data
```

## Vysledny linearni program
Vysledek se vypisuje ve formatu:
```
#OUTPUT: ${vaha odebranych hran}
v_${id vrcholu} --> ${id vrcholu}
#OUTPUT END
```
LP je rozdelen na dve casti
- model - staticky soubor pro vsechny grafy s linearnim programem
- data - soubor generovany generatorem pro kazdy graf zvlast

## Testovani a benchmark
pro testovani je pripraven script pro powershell *run tests.ps1*, ktery
nacte testy ze slozky tests a pro kazdy z nich spusti generator a
glpsol. Vysledek vypise na standartni vystup ve formatu
`${nazev testu}	${vysledna vaha odebranych hran}	${cas} s`.
na zacatku scriptu je mozne upravit maximalni cas,
po ktery ma test bezet a po teto dobe je zastaven a misto vysledku je
vepsan znak "?" a misto casu pak ">300" (nebo jine cislo, podle max casu).
