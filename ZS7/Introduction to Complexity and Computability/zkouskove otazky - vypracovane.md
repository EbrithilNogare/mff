## 🟣 Riceova veta, dukaz pomoci m-prevoditelnosti:

Necht C je trida **castecne rozhodnutelnych jazyku**. Potom jazyk $L_C=\{\langle M\rangle |L(M)\in C\}$
je **rozhodnutelny**, prave kdyz je trida $C$ **trivialni**, tj. bud je prazdna, nebo obsahuje vsechny castecne rozhodnutelne jazyky.

### Dukaz

🔴 todo

## 🟣 A2 Savicova veta:

pro kazdou funkci $f(n)\geq \log_2 n$ plati, ze $NSPACE(f(n))\subseteq SPACE(f^2(n))$

### Dukaz

- Mejme jazyk $L\in NSPACE(f(n))$
- Existuje NTS $M$, ktery prijima $L$ v prostoru $O(f(n))$
- Popiseme DTS $M'$, ktery rozhoduje L v prostoru $O(f^2(n))$
- Idea:
  - Se vstupem $x$, hledej cestu z pocatku do prijimajiciho stavu v grafu $G_{M,x} = (V,E)$
- Rozdel a panuj:
  - hledame cestu z $C_1$ do $C_2$ pres $C_m$, ta bude dlouha max $O(f(n))$
  - rekurzivne hledame cestu polovicni delky, az dokud neni delka nulova.
  - pocet ruznych vrcholu $C_m$ je $O(f(n))$
- Celkove $O(f^2(n))$
- a prostorove nam staci $O(f(n))$
- Je-li $f(n)$ neznama tak zkousime $1, 2, 3 ... i$ dokud tu cestu nenajdeme.

## 🟣 A3 Deterministická prostorová hierarchie:

Pro kazdou prostorove konstruovatelnou funkci $f:N->N$ existuje jazyk A, ktery je rozhodnutelny v prostoru $O(f(n))m$ nikoli vsak v prostoru $o(f(n))$.

### Dukaz

- A definujeme popsanim stroje $D$, ktery rozhoduje $A$
- $D$ pracuje v prostoru $O(f(n))$
- Pro kazdy stroj $M$, ktery pracuje v prostoru $o(f(n))$ plati, ze $L(M) \neq L(D)$
- Ukazeme ze neexistuje T', ktery by prijimal stejny jazyk v $o(f(n))$
- Vyuzijeme diagonalizaci
- Konstrukce D
  - Simuluj $M(⟨M⟩)$ v prostoru $f(n)$
  - Pokud $M$ odmitl, prijmi, jinak odmitni
  - Retezec musi byt vsech delek $⟨M⟩10^*$
  - vstup $x = ⟨M⟩10^{n_0}$
  - prostor $f(n)$ staci k simulaci $M(x)$
  - $D(x)$ prijme, prave kdyz $M(x)$ odmitne
  - Tedy $L(M) \neq L(D)$
  - problem zasstaveni: zastav pokud simulace vyzaduje vice nez $2^{f(n)}$
- $g(n) = o(f(n)) => (\exist n_0 \in N)(\forall n \geq n_0)[c_M g(n)\leq f(n)]$
- existuji konstanty $c_M$ a $n_0$, takove ze se vstupem $x$ delky $n \geq n_0$
  - $M(x)$ lze simulovat v prostoru $c_M g(n) \leq f(n)$
  - $M(x)$ skonci vypocet do $2^{C_Mg(n)} \leq 2^{f(n)}$ kroku
- $L(D) \neq L(M)$

## 🟣 A4 Deterministická časová hierarchie:

Pro kazdou casove konstruovatelnou funkci $f:N->N$ existuje jazyk A, ktery je rozhodnutelny v case $O(f(n))$, nikoli vsak v case $o(\frac{f(n)}{log f(n)})$

### Dukaz

- podobne jako v pripade prostoru
- je potreba simulovat $M(x)$ s pocitanim kroku
- TM musi mit jednu pasku
- Definujeme $A=L(D)$
- instrukce $M(x)$ musi trvat maximalne $c_M$ kroku simulace
- snizujeme pocitadlo o 1 po kazdem kroku D v case $O(\log f(n))$
- vstup x tvaru $⟨M⟩10^*$
- pridame dalsi 2 pasky
  - 2.paska ma aktualni stav vypoctu
  - 3.paska je binarni pocitadlo a je zarovnana s hlavou
    - to udela v case $O(\log(f(n)))$
- Dohromady zabere $O(f(n))$ kroku na simulaci $M(x)$ a manipulaci s citacem
- Sporem
  - predpokladame ze takovy stroj existuje
  - Skrz simulaci na stroji N se odvodi spor a tedy takovy stroj $M$ existovat nemuze

## 🟣 A5 Cookova-Levinova věta (NP-úplnost SAT):

Problem SAT je NP-uplny.

### Dukaz

- SAT patri do NP
  - Z definice, pokud dostaneme vektor $t \in \{0, 1\}^n$, muzeme spocitat hodnotu formule $\phi(t)$ v polynomialnim case.
  - Certifikatem kladne odpovedi je splnujici ohodnoceni, coz potvrdi, ze SAT patri do NP.
- NP-tezkost splnitelnosti:
  - Ukazeme prevod z problemu Kachlikovani na SAT.
  - Kachlikovani je specificky problem s barevnymi kachliky, jejichz umisteni na ctvercove siti se zkouma.
  - Konstruujeme formuli ϕ v konjunktivni normalni forme (KNF), ktera je splnitelna, prave kdyz existuje pripustne vykachlikovani.
- Konstrukce formule ϕ:
  - Definujeme mnoziny urcujici nekompatibilni dvojice kachliku (V a H) podle barev.
  - Pro kazdy sloupec a radek definujeme mnoziny Ui, Bi, Li a Ri pro okrajove barvy.
  - Konstruujeme formuli ϕ pomoci klauzuli Ci,j, ai,j, Bi,j, yi,j, 6i,j, εu, εb, εl a εr.
- Dukaz splnitelnosti:
  - Ukazujeme, ze pokud existuje pripustne vykachlikovani S, lze vytvorit splnujici ohodnoceni formule ϕ.
  - Opakovanym pouzitim ohodnoceni xi,j,k = 1 na odpovidajicich pozicich podle S vytvorime splnujici ohodnoceni.
- Dukaz opacneho smeru:
  - Ukazujeme, ze pokud existuje splnujici ohodnoceni formule ϕ, lze z nej vycist pripustne vykachlikovani S.
  - Prirazujeme kachliky na zaklade hodnot v[i, j, k] v ohodnoceni.
- Zaver:
  - Dukaz ukazuje, ze problem SAT je NP-uplny, coz znamena, ze je NP-tezky a zaroven patri do tridy NP.

---

## 🟣 B1 Univerzální Turingův stroj a nerozhodnutelnost jazyka univerzálního Turingova stroje:

### Univerzální Turingův stroj

Univerzalni Turinguv stroj je $U$
Vstupem je retezec $\langle M,x\rangle$, kde $M$ je Turinguv stroj a $x$ je binarni retezec.
$U$ simuluje praci $T$M nad vstupem $x$.
$U(⟨M,x⟩)↓$ prave kdyz $M(x)↓$ a $U(⟨M,x⟩)$ prijme prave kdyz $M(x)$ prijme.

### Nerozhodnutelnost jazyka

🔴 todo

## 🟣 B2 RAM a ekvivalence s Turingovým strojem:

🔴 todo

## 🟣 B3 Vlastnosti (Turingovsky) rozhodnutelných a částečně rozhodnutelných jazyků (uzávěrové vlastnosti, Postova věta, enumeratory):

🔴 todo

## 🟣 B4 Definice zakladnich trid slozitosti a dukaz NTIME(f(n)) $\subseteq$ SPACE(f(n)):

🔴 todo

## 🟣 B5 Definice zakladnich trid slozitosti a dukaz vety o vztahu a casu (($\forall L \in$ NSPACE(f(n)))($\exist c_L$)[L $\in$ TIME($2^{c_Lf(n)}$)]):

🔴 todo

## 🟣 B6 Dvě definice třídy NP a jejich ekvivalence:

🔴 todo

## 🟣 B7 Polynomialní převod SAT na 3-SAT:

🔴 todo

## 🟣 B8 Polynomialní převod 3-SAT na Vrcholové pokrytí:

🔴 todo

## 🟣 B9 Definice třídy FPT a kernelu a jejich souvislost. Kernelizace Vrcholového pokrytí:

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
