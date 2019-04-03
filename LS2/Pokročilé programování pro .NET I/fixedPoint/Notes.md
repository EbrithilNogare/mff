Výsledky benchmarkù pro poèítání ve Fixed notaci
================================================

# specifikace
Porovnávaly se tyto datové typy:
* Q24_8
* Q16_16
* Q8_24
* Float
* Double

Porovnávali se tyto operace:
* Sèítání
* Odèítání
* Násobení
* Dìlení
* Gausova eliminace

Benchmarky byly spuštìny s defaultním configem (tedy bez žádného)
Platformou byl Intel s hardwarovou podporou dìlení a desetiných èísel


# výsledky testù

|                                         	|    Q24_8    	| Q16_16      	| Q8_24       	| Float     	| Double    	|
|:---------------------------------------:	|:-----------:	|-------------	|-------------	|-----------	|-----------	|
|                 AddTests                	| 260.3110 ns 	| 263.5006 ns 	| 266.6582 ns 	| 0.0388 ns 	| 0.0422 ns 	|
|              SubtractTests              	| 264.3520 ns 	| 264.3670 ns 	| 266.9845 ns 	| 0.0238 ns 	| 0.0155 ns 	|
|              MultiplyTests              	| 262.7965 ns 	| 262.1757 ns 	| 266.3476 ns 	| 0.0577 ns 	| 0.0376 ns 	|
|               DivideTests               	| 272.6980 ns 	| 285.5533 ns 	| 271.6650 ns 	| 0.0089 ns 	| 0.0204 ns 	|
|        AddTestsWithoutDeclaration       	|  94.3261 ns 	|  96.7004 ns 	|  87.4589 ns 	| 0.0019 ns 	| 0.0078 ns 	|
|    AddTestsWithoutDeclarationForCycle   	| 99,859.3 ns 	| 90,994.1 ns 	| 87,650.3 ns 	|  593.0 ns 	|  596.3 ns 	|
| SubtractTestsWithoutDeclarationForCycle 	| 94,394.8 ns 	| 90,432.1 ns 	| 87,801.4 ns 	|  592.2 ns 	|  612.9 ns 	|
| MultiplyTestsWithoutDeclarationForCycle 	| 99,357.3 ns 	| 90,735.2 ns 	| 88,476.7 ns 	|  596.7 ns 	|  579.3 ns 	|
|  DivideTestsWithoutDeclarationForCycle  	|   98.131 us 	|   98.907 us 	|   91.472 us 	|  1.133 us 	|  1.138 us 	|
|         **GauseEliminationTests**     	|   234.09 us 	|   243.65 us 	|   242.82 us 	|  97.16 us 	|  42.41 us 	|



První ètyøi skupiny testù byly zamìøeny na jednotlivé operace
Poslední skupina na použití v komplexnìjším øešení

Pomìry mezi výsledky èistých operací a komplexního øešení
jsou øádovì rozdílné
**pomìr 10000 oproti 10** tedy musí mýt nìjakou pøíèinu.

V testech na operace se nachází deklarace a jediná operace.
Tudíž mùžeme øíct, že nejvíce èasu nám zabírá
ona deklarace promìných typu FixedPoint.


V testu "AddTestsWithoutDeclaration" byly odebrány deklarace a
byly umístìny pøed metody a tudíž se nepoèítali do èasu.
**Rozdíl se snížil**, ale stále by šlo testovat èistì operace lépe.
Zkusme izolovat danou operaci zvýšením poètu opakování.


Opakovích bylo nastaveno *1000* a z výslekù pozorujeme
**stabilitu typu FP** a naopak **zpomalení pro
typy float a double**, které se pøi vysokém poètu opakování
stávají až 20x ménì efektivní. Což je dáno tím, že samotný for cyklus 
v sobì obsahuje pøièítání a porovnání.



# Závìr
 Z celkových výsledkù vyplývá, že použití Fixed point typù je **øádovì
 horší**, než používat vestavìné floating point typy.

 Dále mùžeme øíct, že **dìlení není nijak výraznì horší**,
 než jiné operace na tomto typu procesoru.




 # Zdroje
 [gausova eliminace (Licenced CPOL)](https://www.codeproject.com/Tips/388179/Linear-Equation-Solver-Gaussian-Elimination-Csharp)