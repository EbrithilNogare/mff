## 🟣 Riceova veta, dukaz pomoci m-prevoditelnosti:

Necht C je trida **castecne rozhodnutelnych jazyku**. Potom jazyk $L_C=\{\langle M\rangle |L(M)\in C\}$
je **rozhodnutelny**, prave kdyz je trida $C$ **trivialni**, tj. bud je prazdna, nebo obsahuje vsechny castecne rozhodnutelne jazyky.

### Dukaz

🔴 todo

### m-prevoditelnost

Necht A a B jsou dva jazyky nad abecedou $\sum$.
Rekneme, ze jazyk A je m-prevoditelny na jazyk B, pokud existuje totalni algoritmicky vycislitelna funkce $f:\sum^* -> \sum^*$, pro kterou plati, ze
$(\forall x\in\sum^*)[x\in A<=>f(x)\in B]$
Tento fakt oznacime pomoci $A\leq_m B$

## 🟣 A2 Savicova veta:

pro kazdou funkci $f(n)\geq \log_2 n$ plati, ze $NSPACE(f(n))\subseteq SPACE(f^2(n))$

### Dukaz

Mejme jazyk $L\in NSPACE(f(n))$,
nedeterministicky Turinguv stroj $M=(Q,\sum,\delta, q_0, F)$, prijimajici jazyk $L = L(M)$ v prostoru $O(f(n))$

Vytvorime deterministicky Turinguv stroj M' v prostoru $O(f^2(n))$.
Uvazme vstup $x\in\sum^*$. Vime ze $x\in L$ prave kdyz existuje cesta grafem automatu.
🔴 todo

## 🟣 A3 Deterministická prostorová hierarchie:

Deterministická prostorová hierarchie je hierarchie tříd jazyků, kde každá třída obsahuje jazyky, které lze rozpoznat deterministickým Turingovým strojem s omezeným množstvím paměti. Tato hierarchie nám ukazuje, že existují jazyky, které vyžadují stále více paměti pro své rozpoznání, a že některé problémy jsou složitější než jiné.

### Veta

Pro kazdou prostorove konstruovatelnou funkci $f:N->N$ existuje jazyk A, ktery je rozhodnutelny v prostoru $O(f(n))m$ nikoli vsak v prostoru $O(f(n))$.

### Dukaz

🔴 todo

## 🟣 A4 Deterministická časová hierarchie:

Deterministická časová hierarchie je hierarchie tříd jazyků, kde každá třída obsahuje jazyky, které lze rozpoznat deterministickým Turingovým strojem s omezeným časem. Tato hierarchie ukazuje, že existují jazyky, které vyžadují stále více času pro své rozpoznání, což nám umožňuje třídit problémy podle jejich časové složitosti.

### Veta

Pro kazdou casove konstruovatelnou funkci $f:N->N$ existuje jazyk A, ktery je rozhodnutelny v case $O(f(n))$, nikoli vsak v case $O(\frac{f(n)}{log_2 f(n)})$

### Dukaz

🔴 todo

## 🟣 A5 Cookova-Levinova věta (NP-úplnost SAT):

Cookova-Levinova věta je klíčovým výsledkem teorie složitosti. Dokazuje, že problém SAT (splnitelnost booleovské formule) je NP-úplný, což znamená, že je v třídě NP a že všechny problémy v NP lze na něj m-prevést. Tato věta položila základ pro teorii NP-úplných problémů a zahrnuje konstrukci kódování všech problémů v NP.

### Veta

Problem SAT je NP-uplny.

### Dukaz

🔴 todo

---

## 🟣 B1 Univerzální Turingův stroj a nerozhodnutelnost jazyka univerzálního Turingova stroje:

Univerzální Turingův stroj je Turingův stroj, který dokáže simulovat jiný Turingův stroj na základě jeho kódu a vstupu. Toto umožňuje definovat univerzální jazyk, který obsahuje kódy všech Turingových strojů, kteří zastavují na prázdném vstupu. Gödelova první nerozhodnutelnostní věta nám říká, že tato univerzální jazyk není rozhodnutelný.

### Univerzální Turingův stroj

Univerzalni Turinguv stroj je $U$
Vstupem je retezec $\langle M,x\rangle$, kde $M$ je Turinguv stroj a $x$ je binarni retezec.
$U$ simuluje praci $T$M nad vstupem $x$.
$U(⟨M,x⟩)↓$ prave kdyz $M(x)↓$ a $U(⟨M,x⟩)$ prijme prave kdyz $M(x)$ prijme.

### nerozhodnutelnost jazyka

🔴 todo

## 🟣 B2 RAM a ekvivalence s Turingovým strojem:

RAM (Random Access Machine) je teoretický model počítače, který vychází z konceptu paměti s náhodným přístupem. Důležitým výsledkem je ekvivalence RAM s Turingovými stroji, což znamená, že všechny problémy, které lze řešit na RAM, lze řešit také na Turingově stroji a naopak.
🔴 todo

## 🟣 B3 Vlastnosti (Turingovsky) rozhodnutelných a částečně rozhodnutelných jazyků (uzávěrové vlastnosti, Postova věta, enumeratory):

Turingovsky rozhodnutelné jazyky jsou třída jazyků, které lze rozhodnout algoritmem na Turingově stroji. Tyto jazyky mají mnoho vlastností, včetně uzávěrových vlastností, což znamená, že jsou uzavřené vzhledem k operacím, jako jsou sjednocení, průnik a doplněk. Postova věta nám říká, že neexistuje algoritmus, který by dokázal rozhodnout, zda libovolný Turingův stroj na daném vstupu zastaví. Enumeratory jsou algoritmy, které generují všechny řetězce z daného jazyka.
🔴 todo

## 🟣 B4 Definice zakladnich trid slozitosti a dukaz NTIME(f(n)) $\subseteq$ SPACE(f(n)):

Základní třídy složitosti, jako P, NP, a NSPACE(f(n)), se používají k třídění problémů podle jejich složitosti. Důkaz NTIME(f(n)) ⊆ SPACE(f(n)) ukazuje, že problémy, které lze řešit v deterministickém čase f(n), lze také řešit v prostoru f(n), což je důležité pro analýzu prostorové složitosti algoritmů.
🔴 todo

## 🟣 B5 Definice zakladnich trid slozitosti a dukaz vety o vztahu a casu (($\forall L \in$ NSPACE(f(n)))($\exist c_L$)[L $\in$ TIME($2^{c_Lf(n)}$)]):

Tato věta ukazuje vztah mezi časem a prostorem pro jazyky, které jsou rozpoznávány nedeterministickými Turingovými stroji. Říká nám, že pokud jazyk L lze rozpoznat v prostoru f(n), existuje konstanta c_L, takže L lze rovněž rozpoznat v čase 2^(c_L \* f(n)).
🔴 todo

## 🟣 B6 Dvě definice třídy NP a jejich ekvivalence:

Třída NP má dvě ekvivalentní definice. První definice je založena na nedeterministických Turingových strojích a říká, že jazyk L je v NP, pokud existuje nedeterministický Turingův stroj, který L rozpoznává v polynomiálním čase. Druhá definice je založena na sertifikátech a říká, že L je v NP, pokud existuje polynomiální algoritmus, který ověřuje, zda daný certifikát patří do L.
🔴 todo

## 🟣 B7 Polynomialní převod SAT na 3-SAT:

Polynomialní převod SAT na 3-SAT je konstrukce, která vezme booleovskou formuli v konjunktivní normální formě (CNF) a převede ji na ekvivalentní formuli v 3-konjunktivní normální formě (3-CNF). Tento převod zachovává splnitelnost formule, a proto nám umožňuje pracovat s 3-SAT variantou problému SAT, což je užitečné pro některé důkazy v teorii složitosti.
🔴 todo

## 🟣 B8 Polynomialní převod 3-SAT na Vrcholové pokrytí:

Polynomialní převod 3-SAT na Vrcholové pokrytí je konstrukce, která vezme instanci 3-SAT problému a převede ji na ekvivalentní instanci problému Vrcholové pokrytí na grafu. Tento převod je užitečný pro dokazování NP-úplnosti problému Vrcholové pokrytí, protože 3-SAT je NP-úplný, a to nám umožňuje ukázat, že i Vrcholové pokrytí je NP-úplný.
🔴 todo

## 🟣 B9 Definice třídy FPT a kernelu a jejich souvislost. Kernelizace Vrcholového pokrytí:

Třída FPT (Fix-parameter tractable) obsahuje problémy, které jsou těžké obecně, ale jsou řešitelné v rámci parametrického hledání. Kernelizace je technika, která se používá k redukci instancí problému na kompaktnější podobu nazývanou "kernel." Pro problém Vrcholového pokrytí je kernelizace proces, kdy se snažíme omezit počet vrcholů v instanci tak, aby závisel na parametru (např. velikosti vrcholového pokrytí). Tím získáváme problémy, které jsou v třídě FPT a jsou efektivně řešitelné.
🔴 todo

## 🟣 B10 Definice třídy $FPT$ a parametrizovaný algoritmus pro Vrcholové pokrytí založený na prohledávání s omezenou hloubkou (se složitostí menší než $O^*(2^k)$).

🔴 todo

## 🟣 B11 Třída #P a #P-úplnost, důkaz těžkosti počítání cyklů v grafu.

🔴 todo

## 🟣 B12 Třída co-NP a co-NP-úplnost.

🔴 todo

## 🟣 B13 Pseudonáhodné generátory, jednosměrné funkce a jejich souvislost s kryptografií (symetrické šifrování, bit-commitment).

🔴 todo

## 🟣 B14 Příklad zjemnělé redukce (redukce SETH na OV nebo OV na hledání regulárního výrazu v textu).

🔴 todo
