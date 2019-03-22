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

## první bìh
* Benchmarky.AddTests
  *    Q24_8Test           :  456.6098 ns
  *    Q16_16Test          :  444.5341 ns
  *    Q8_24Test           :  470.4036 ns
  *    FloatTest           :  0.0391 ns
  *    DoubleTest          :  0.0387 ns
* Benchmarky.SubtractTests
  *    Q24_8Test           :  442.0263 ns
  *    Q16_16Test          :  442.6848 ns
  *    Q8_24Test           :  448.5160 ns
  *    FloatTest           :  0.0405 ns
  *    DoubleTest          :  0.0303 ns
* Benchmarky.MultiplyTests
  *    Q24_8Test           :  444.9907 ns
  *    Q16_16Test          :  444.6477 ns
  *    Q8_24Test           :  449.4225 ns
  *    FloatTest           :  0.0271 ns
  *    DoubleTest          :  0.0141 ns
* Benchmarky.DivideTests
  *    Q24_8Test           :  454.4039 ns
  *    Q16_16Test          :  466.1081 ns
  *    Q8_24Test           :  457.4034 ns
  *    FloatTest           :  0.0368 ns
  *    DoubleTest          :  0.0388 ns
* Benchmarky.GauseEliminationTests
  *    Q24_8Test           :  322.60 us
  *    Q16_16Test          :  322.40 us
  *    Q8_24Test           :  330.52 us
  *    DoubleTest          :  28.30 us


První ètyøi skupiny testù byly zamìøeny na jednotlivé operace
Poslední skupina na použití v komplexnìjším øešení

Pomìry mezi výsledky èistých operací a komplexního øešení
jsou øádovì rozdílné
**pomìr 10000 oproti 10** tedy musí mýt nìjakou pøíèinu.

V testech na operace se nachází deklarace a jediná operace.
Tudíž mùžeme øíct, že nejvíce èasu nám zabírá
ona deklarace promìných typu FixedPoint.


Testy jsem tedy pøedìlal a **deklaraci "vyndal" mimo test**.

## druhý bìh
* Benchmarky.AddTestsWithoutDeclaration
  *    Q24_8Test           :  170.8000 ns
  *    Q16_16Test          :  185.1193 ns
  *    Q8_24Test           :  165.4359 ns
  *    FloatTest           :  0.0357 ns
  *    DoubleTest          :  0.0418 ns

Po takovéto úpravì již máme reálné benchmarky èistì operací.
**Rozdíl se snížil**, ale stále by šlo testovat èistì operace lépe.

Zkusme izolovat danou operaci zvýšením poètu opakování.


## tøetí bìh
* Benchmarky.AddTestsWithoutDeclarationForCycle
  *    Q24_8Test           :  1,668.693 us
  *    Q16_16Test          :  1,620.104 us
  *    Q8_24Test           :  1,632.980 us
  *    FloatTest           :  6.456 us
  *    DoubleTest          :  10.239 us

Opakovích bylo nastaveno *10000* a z výslekù pozorujeme
**stabilitu typu FP** a naopak neèekané **zpomalení pro
typy float a double**, které se pøi vysokém poètu opakování
stávají až 20x ménì efektivní.

# Závìr
 Z celkových výsledkù vyplývá, že použití Fixed point typù je **øádovì
 horší**, než používat vestavìné floating point typy.

 Dále mùžeme øíct, že **dìlení není nijak výraznì horší**,
 než jiné operace na tomto typu procesoru.

 Dále vydíme, že aèkoliv benchmarky FixedPoint jsou stabilní,
 benchmarky u float a double jsou **nestabilní** a testy 
 u tìchto typù mají vyskou míru šumu.



 # Zdroje
 [gausova eliminace (Licenced CPOL)](https://www.codeproject.com/Tips/388179/Linear-Equation-Solver-Gaussian-Elimination-Csharp)