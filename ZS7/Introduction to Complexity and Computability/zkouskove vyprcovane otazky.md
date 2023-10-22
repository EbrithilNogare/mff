## A1 Definice m-prevoditelnosti a m-uplnosti. Riceova veta:

M-prevoditelnost je vlastnost, kdy můžeme efektivně převést problém A na problém B, což značíme A ≤m B. To znamená, že pokud máme řešení B, můžeme ho využít k řešení A. M-uplnost je vlastnost, kdy problém B je tak obtížný, že všechny problémy v dané třídě jsou na něj m-prevoditelné. Riceova věta nám říká, že není možné algoritmicky určit žádnou netriviální vlastnost rekurzivních jazyků.

## A2 Savicova věta:

Savicova věta je teorém, který se týká formálních jazyků a dokazuje, že problém ekvivalence bezkontextových gramatik je nerozhodnutelný. To znamená, že není možné algoritmicky rozhodnout, zda dvě bezkontextové gramatiky generují stejný jazyk.

## A3 Deterministická prostorová hierarchie:

Deterministická prostorová hierarchie je hierarchie tříd jazyků, kde každá třída obsahuje jazyky, které lze rozpoznat deterministickým Turingovým strojem s omezeným množstvím paměti. Tato hierarchie nám ukazuje, že existují jazyky, které vyžadují stále více paměti pro své rozpoznání, a že některé problémy jsou složitější než jiné.

## A4 Deterministická časová hierarchie:

Deterministická časová hierarchie je hierarchie tříd jazyků, kde každá třída obsahuje jazyky, které lze rozpoznat deterministickým Turingovým strojem s omezeným časem. Tato hierarchie ukazuje, že existují jazyky, které vyžadují stále více času pro své rozpoznání, což nám umožňuje třídit problémy podle jejich časové složitosti.

## A5 Cookova-Levinova věta (NP-úplnost SAT):

Cookova-Levinova věta je klíčovým výsledkem teorie složitosti. Dokazuje, že problém SAT (splnitelnost booleovské formule) je NP-úplný, což znamená, že je v třídě NP a že všechny problémy v NP lze na něj m-prevést. Tato věta položila základ pro teorii NP-úplných problémů a zahrnuje konstrukci kódování všech problémů v NP.

## B1 Univerzální Turingův stroj a nerozhodnutelnost jazyka univerzálního Turingova stroje:

Univerzální Turingův stroj je Turingův stroj, který dokáže simulovat jiný Turingův stroj na základě jeho kódu a vstupu. Toto umožňuje definovat univerzální jazyk, který obsahuje kódy všech Turingových strojů, kteří zastavují na prázdném vstupu. Gödelova první nerozhodnutelnostní věta nám říká, že tato univerzální jazyk není rozhodnutelný.

## B2 RAM a ekvivalence s Turingovým strojem:

RAM (Random Access Machine) je teoretický model počítače, který vychází z konceptu paměti s náhodným přístupem. Důležitým výsledkem je ekvivalence RAM s Turingovými stroji, což znamená, že všechny problémy, které lze řešit na RAM, lze řešit také na Turingově stroji a naopak.

## B3 Vlastnosti (Turingovsky) rozhodnutelných a částečně rozhodnutelných jazyků (uzávěrové vlastnosti, Postova věta, enumeratory):

Turingovsky rozhodnutelné jazyky jsou třída jazyků, které lze rozhodnout algoritmem na Turingově stroji. Tyto jazyky mají mnoho vlastností, včetně uzávěrových vlastností, což znamená, že jsou uzavřené vzhledem k operacím, jako jsou sjednocení, průnik a doplněk. Postova věta nám říká, že neexistuje algoritmus, který by dokázal rozhodnout, zda libovolný Turingův stroj na daném vstupu zastaví. Enumeratory jsou algoritmy, které generují všechny řetězce z daného jazyka.

## B4 Definice zakladnich trid slozitosti a dukaz NTIME(f(n)) $\subseteq$ SPACE(f(n)):

Základní třídy složitosti, jako P, NP, a NSPACE(f(n)), se používají k třídění problémů podle jejich složitosti. Důkaz NTIME(f(n)) ⊆ SPACE(f(n)) ukazuje, že problémy, které lze řešit v deterministickém čase f(n), lze také řešit v prostoru f(n), což je důležité pro analýzu prostorové složitosti algoritmů.

## B5 Definice zakladnich trid slozitosti a dukaz vety o vztahu a casu (($\forall L \in$ NSPACE(f(n)))($\exist c_L$)[L $\in$ TIME($2^{c_Lf(n)}$)]):

Tato věta ukazuje vztah mezi časem a prostorem pro jazyky, které jsou rozpoznávány nedeterministickými Turingovými stroji. Říká nám, že pokud jazyk L lze rozpoznat v prostoru f(n), existuje konstanta c_L, takže L lze rovněž rozpoznat v čase 2^(c_L * f(n)).

## B6 Dvě definice třídy NP a jejich ekvivalence:

Třída NP má dvě ekvivalentní definice. První definice je založena na nedeterministických Turingových strojích a říká, že jazyk L je v NP, pokud existuje nedeterministický Turingův stroj, který L rozpoznává v polynomiálním čase. Druhá definice je založena na sertifikátech a říká, že L je v NP, pokud existuje polynomiální algoritmus, který ověřuje, zda daný certifikát patří do L.

## B7 Polynomialní převod SAT na 3-SAT:

Polynomialní převod SAT na 3-SAT je konstrukce, která vezme booleovskou formuli v konjunktivní normální formě (CNF) a převede ji na ekvivalentní formuli v 3-konjunktivní normální formě (3-CNF). Tento převod zachovává splnitelnost formule, a proto nám umožňuje pracovat s 3-SAT variantou problému SAT, což je užitečné pro některé důkazy v teorii složitosti.

## B8 Polynomialní převod 3-SAT na Vrcholové pokrytí:

Polynomialní převod 3-SAT na Vrcholové pokrytí je konstrukce, která vezme instanci 3-SAT problému a převede ji na ekvivalentní instanci problému Vrcholové pokrytí na grafu. Tento převod je užitečný pro dokazování NP-úplnosti problému Vrcholové pokrytí, protože 3-SAT je NP-úplný, a to nám umožňuje ukázat, že i Vrcholové pokrytí je NP-úplný.

## B9 Definice třídy FPT a kernelu a jejich souvislost. Kernelizace Vrcholového pokrytí:

Třída FPT (Fix-parameter tractable) obsahuje problémy, které jsou těžké obecně, ale jsou řešitelné v rámci parametrického hledání. Kernelizace je technika, která se používá k redukci instancí problému na kompaktnější podobu nazývanou "kernel." Pro problém Vrcholového pokrytí je kernelizace proces, kdy se snažíme omezit počet vrcholů v instanci tak, aby závisel na parametru (např. velikosti vrcholového pokrytí). Tím získáváme problémy, které jsou v třídě FPT a jsou efektivně řešitelné.

