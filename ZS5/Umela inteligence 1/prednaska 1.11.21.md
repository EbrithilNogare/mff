## Pattern database
neco jako sachova koncovka

vezmeme jen nejake vstupni informace u nichz vime jak je vyhodnotit a
zbytek doresime jinak  

## Local search
mame jen jeden stav (konstantni misto), zahazujeme cestu

vpodstate hladovy algoritmus  

nezjistime jestli jsme v cyklu

nevime jakym smerem jit, pokud oba sousedni stavy jsou ekvivalentni

### Hill climmbing (HC)
zvolime lepsi sousedni stav a tam se vydame,
aktuaalni stav zapomeneme

#### Problem hrebenove krajiny
mnoho dobrych stavu, ale jen jeden nejlepsi

vrati lokalni nejlepsi stav jako nejlepsi (ale ne globalne)

#### Stochastic hill climb
nebere nejlepsi cestu, ale vybira si libovolnou lepsi.
Bud prvni cestu co najde, nebo nahodnym vyberem.

### Random walk (nahodna prochazka)
zarucuje vysledek, ale bude to trvat velmi dlouho

### Simulated annealing (Simulace teploty)
kombinace Random walku a HC

inspirovavno tvrzenim materialu pomoci lokalniho zahrivani a ochlazovani a
zaroven globalniho ochlazovani

Vyber nahodne cesty, ktera se bud prijme pokud
1) je zlepsujici
2) je zhorsujici, ale ne moc (to jak moc se muze zhorsit se casem snizuje)

## Local beam search
mame k stavu (k je na zacatku dana konstanta)

vybirame k novych stavu ze vsech aktualnich k stavu  
ne 1 : 1, nektere stavy muzou prispet vice, nez jine

### Stochastic beam search
to same co Stochastic hill climb, ale s k stavy

### Genericky algoritmus
Mame populaci a dalsi generace (stavy) vznikaji skrizenim dvou predchozich satvu

# offline vs online prohledavani
**Offline**: naplanujeme celou cestu dopredu a pak jen nasledujeme cestu.  
**Online**: nejdrive udelej akci a pak teprve vypocitej dalsi akci.

nedelat offline problem pomoci online (jde to, ale je to blbost),
neboli kdyz znam cele prostredi, tak nepotrebuji spravnou cestu kontrolovat

## Porovnani kvality
### Competitive ratio
spustime oneline i offline algoritmus a porovname jak si online vedl

### Dokazovani nalezeni online cile
Poridime si vsevedouciho oponenta, ktery problem upravuje tak, aby
nas algoritmus selhal.
Pokud oponent prohraje ve vsech pripadech, tak nas algortmus vzdy najde cil.
Neboli vidime cely svet a nemuze nas nic prekvapit.

## Local online search
Pokud jsme v prohlubni, tak ji postupne zaplnujeme.


