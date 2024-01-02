# Velke otazky

## 🟣Definujte Splay strom. Vyslovte a dokažte větu o amortizované složitosti operace Splay.

### Definice splay stromu

- binarni vyhledavaci strom
- samovyvazujici se
- pouziva splay operace (zig/zag ...)

### Amortizovana slozitost operace splay:

- amortizovana cena je nejvyse 3( r'(v) - r(v) ) + 1
- coz dava slozitost $O(\log n)$

#### Dukaz

- Celkova cena je rovna sume jednotlivych kroku
- Mejme hloubky x po 0 az k krocih: $r_0(x), ..., r_k(x)$
- Cena jednoho kroku je $3r_i(x) - 3r_{i-1}(x)$, plus 1 pokud je to ZIG rotace
- Amortizovana cena $≤ \sum^t_{i=1}(3r_i(x) - 3r_{i-1}(x)) + 1$
- Amortizovana cena $≤ 3r_t(x) - 3r_0(x) + 1$

## 🟣Definujte (a,b)-strom. Popište, jak na něm probíhají operace Find, Insert a Delete. Rozeberte jejich slozitost v nejhorším případě.

### Definice

- vyhledavaci strom
- vsechny vrcholy krome korene maji pocet synu A az B
- koren ma nejvyse B synu
- Vsechny listy jsou ve stejne vysce
- Plati a >= 2 & b >= 2a - 1

### Find

- V kazdem vrcholu najdeme nejmensi vetsi klic a do jeho ditete vstoupime
- Koncime v listu

### Insert

- Najdeme spravneho otce
- Pridame prvek
- Pokud se nevejde rozdedlime deti na dve casti a pridame novy klic do rodice
- Opakujeme dokud se klice nevejdou

### Delete

- Najdeme spravneho otce
- Odebereme prvek
- Pokud je klicu malo, presuneme klic z bratra, nebo se spojime s bratrem

### Nejhorsi pripad - slozitost

- vsechny $O(\log n)$

## 🟣Formulujte cache-aware a cache-oblivious algoritmy pro transpozici čtvercové matice. Rozeberte jejich časovou složitost a I/O složitost.

### Cache aware

- Rekurzivne delime matice na ctvrtiny dokud nemame matice 1x1
- $A = (\dfrac{A_{11}|A_{12}}{A_{21}|A_{22}})$
- $A_{11}$ a $A_{22}$ se pouze transponuji
- $A_{12}$ a $A_{21}$ se transponuji a prohodi navzajem

### Cache oblivious

- rozdelime matici na submtice $d*d$ tak ze $2d^2 \leq M$ (2 submatice se vejdou do cache naraz)
- ztransponujeme obe submatice
- prohodime je

### Slozitost

#### Cache aware

- pocet podmatic je $\dfrac{k^2}{d^2}$
- $O(\dfrac{k^2}{B})$
- $k \times k$ je velikost matice
- $d \times d$ je velikost podmatice
- $B$ je velikost bloku pameti

#### Cache oblivious

- $\dfrac{k^2}{d^2} * 4d \leq \dfrac{8k^2}{B} = O(\dfrac{k^2}{B})$

## 🟣Definujte c-univerzální a k-nezávislé rodiny hešovacích funkcí. Formulujte a dokažte větu o střední délce řetězce v hešování s řetězci. Ukažte příklady c-univerzálních a k-nezávislých rodin pro hešování přirozených čísel.

### c-univerzální

- pocet hesovacich funkci $h \in H$ takovych ze $h(x) = h(y)$ je nejvyse $\dfrac{c|H|}{m}$ , (pro vsechna $x \neq y$)
- nahodne zvolena $h \in H$ splnuje $P[h(x) = h(y)] \leq \dfrac{c}{m}$

### k-nezávislý

- $k \in N; K = \{1, ..., k\}$ a $c \geq 1$
- $P[h(x_i) = z_i \forall i \in K] \leq \dfrac{c}{m^k}$
- pro vsechna po dvou ruzna $x_1, ...,  x_k \in U$ a vsechna $z_1, ..., z_k \in M$

### věta o střední délce řetězce v hešování s řetězci

- Jestlize $H$ je c-univerzalni, pak ocekavany pocet prvku v prihradne $h(x)$ pro $x \in U$ je nejvyse $\dfrac{cn}{m}$

#### Dukaz

- $E[|\{y \in S : h(x) = h(y)\}|] = \sum_{y \in S} P[h(x) = h(y)] \leq \dfrac{cn}{m}$

### příklady

#### c-univerzální

Multiply shift

- $h_a(x) = (ax \mod 2^w) >> (w-l)$
- 2-univerzalni

Poly mod prime

- $h_{a, b}(x) = ax + b \mod p$
- d-univerzalnni

#### k-nezávislý

Tabulkove hashovani

- $h(x) = T_1(x^1) \text{ XOR } ... \text{ XOR } T_d(x^d) $
- 3-nezavisle

## 🟣Popište a analyzujte hešování s lineárním přidáváním. (Linear probing)

- INSERT
  - Vkladanou hodnotu zahesujeme a vlozime na index rovny jeho hashu
  - Pokud tam uz neco je, tak pridame k indexu 1 a zkusime to znovu (opakujeme dokud prvek nevlozime)
- DELETE
  - nelze, pouze vytvorime pomnicek a obcas prehashujeme
- Vytvari shluky, coz neni dobre
- Pratelska na cache
- poloprazda a bez velkych shluku se chova dobre

## 🟣Definujte vícerozměrné intervalové stromy. Rozeberte časovou a prostorovou složitost konstrukce a obdélníkových dotazů (včetně zrychlení kaskádováním).

- Vylepsene intervalove stromy (zlepsi slozitost dotazu)
- Binarni vyhledavaci strom podle $x$

  - Kazdy vrchol ma dalsi BVS podle $y$

- Prostor: $O(n \log^{d-1} n)$
- Cas:
  - BUILD: $O(n \log^{d-1} n)$
  - INSERT: $O(\log^d n)$
  - DELETE: $O(\log^d n)$
  - FIND RANGE: $O(\log^d n + p)$
    - kaskadovani bude v $O(\log^{d-1} n + p)$
    - vsem prvkum nahore predpocitame pozici ve stromu nize

## 🟣Definujte suffixové pole a LCP pole. Popište a analyzujte algoritmy na jejich konstrukci.

- Sufixove pole $S$

  - pro retezec $\alpha$ delky $n$
  - udava lexikograficke poradi sufixu daneho slova $\alpha$
  - lze vybudovat v $O(n)$
    - postavime sufixovy strom a pak jej sepiseme do pole

- LCP pole
  - LCP$(\alpha, \beta) =$ max$\{k | \alpha[:k] == \beta[:k]\}$
  - udava delku nejdelsiho spolecneho prefixu $\alpha$ a $\beta$
  - lze vybudovat v $O(n)$ se sufixovym polem

---

# Malé otázky

## 🟣Popište „nafukovací pole“ se zvětšováním a zmenšováním. Analyzujte jeho amortizovanou složitost.

- zvetsi se na 2x pri zaplneni
- zmensi se na 1/2 pri 1/4 zaplneni
- Amortizovane $O(1)$

## 🟣Popište vyhledávací stromy s líným vyvažováním (BB[α] stromy). Analyzujte jejich amortizovanou složitost.

- binarni vyhledavaci strom
- kazdy podstrom syna musi byt velikosti maximalne $\alpha$ nasobek vsech deti jejich otce
- $\dfrac{1}{2} \le \alpha \le 1$

## 🟣Navrhněte operace Find, Insert a Delete na Splay stromu. Analyzujte jejich amortizovanou složitost.

- vse amortizovane v $O(\log n)$
- zig / zag dvojrotace rotace krom korene

## 🟣Vyslovte a dokažte věty o amortizované složitosti operací Insert a Delete na (a,2a-1)-stromech a (a,2a)-stromech.

- Sekvence $m$ Insertu a Deletu na zezacatku prazdnem (a, 2a) stromu vykona $O(m)$ akci.

### Dukaz

- Cena = pocet zmenenych vrcholu
- Ukazeme ze potencial je mensi roven nule
- Mejme funkci $f(k)$ jakozto zmenu $k$ deti, ktera musi splnovat
  - $|f(i) - f(i+1)| \leq c$, kde $c$ je libovolna konstanta
  - $f(2a)  \geq f(a) + f(a+1) + c + 1$
  - $f(a-2) + f(a-1) \geq f(2a - 2) + c + 1$
- nastavime $c=2$ a vykouzlime:

| k    | a-2 | a-1 | a   | ... | 2a-2 | 2a-1 | 2a  |
| ---- | --- | --- | --- | --- | ---- | ---- | --- |
| f(k) | 2   | 1   | 0   | 0   | 0    | 1    | 2   |

- INSERT prida klic ($O(1)$), zmeni potencial o $O(1)$ a vykona nekolik rozdeleni s amortizovanou slozitosti 0.
- DELETE odebere klic ($O(1)$) a vykona sekvenci spojeni s amortizovanou slozitosti 0.
  - Pripadne pokud si vezme dite bratra, coz ma slozitost $O(1)$ a muze se stat pouze jednou.

Takze amortizovana cena je konstantni pro obe operace, jelikoz potencial je porad nezaporny a zacina v nule.
Tudiz $m$ operaci Insert a Delete provede $O(m)$ modifikaci vrcholu.

## 🟣Analyzujte k-cestný Mergesort v cache-aware modelu. Jaká je optimální volba k?

- Optimalni hodnota K = ⌊M/2B⌋
- Pocet pruchodu klesne na $⌈\log_k N⌉$
- Jeden krok zabere $O(\log K)$ a cely mergesort $O(N \log N)$

## 🟣Vyslovte a dokažte Sleatorovu-Tarjanovu větu o kompetitivnosti LRU.

- LRU = least-recently used
- NEPLATI: LRU je k-kompetetivni, neboli $C_{LRU} \leq k*C_{OPT}$

Veta:

- $C_{LRU} \leq \dfrac{M_{LRU}}{M_{LRU} - M_{OPT}} * C_{OPT} + M_{OPT}$

Dukaz:

- mame epoxhy $E_0$ ... $E_k$
- LRU zaplati v kazde epoche $M_{LRU}$ a v prvni maximalne $M_{LRU}$
- v nenulove epoche $i$:
  1. ruzne bloky: OPT plati alespon bloky ktere nemel v cachi ($M_{LRU} - M_{OPT}$)
  2. LRU zaplati za blok 2x: alespon $M_{LRU}$ ruznych bloku
- v nulove epoche:
  1. LRU i OPT zacina s prazdnou cache: $C_{LRU} =$ # ruznych bloku = $C_{OPT}$
  2. LRU zacina s neprazdnou oproti OPT: neuskodi LRU
  3. LRU i OPT maji neprazdnou cache: to je ta $+ M_{OPT}$

## 🟣Popište systém hešovacích funkcí odvozený ze skalárního součinu. Dokažte, že je to 1-univerzální systém ze $Z_p^k$ do $Z_p$.

- Mejme d-dimenzionalni vektory nad telesem $Z_p$
- $h_t(x) = t \times x$

### Dukaz

- $P[h_t(x) = h_t(y)] = P[x \times t = y \times t] = P[(x-y)\times t = 0] = P[\sum^d_{i=1}(x_i - y_i)t_i = 0] = P[(x_d - y_d)t_d = - \sum^{d-1}_{i=1}(x_i - y_i)t_i]$
- Posledni krok rika, ze posledni iterace sumy by se musela presne trefit a na to ma pravdepodobnost $1/p$
- Neboli existuje prave jedno $t_d$ takove aby rovnost platila a zaroven $t_d \in Z_p$

## 🟣Popište systém lineárních hešovacích funkcí. Dokažte, že je to 2-nezávislý systém ze $Z_p$ do [m].

- $h_{a,b}(x) = ((ax+b)\mod p) \mod m$
- Mejme linearni system $L$, jez je (2,1)-nezavisly v $Z_p$ a po zmoduleni do $m$ je (2,4)-nezavisly podle dukazu nize

### Dukaz

- snazime se dokazat $P[h_{a,b}(x) = i ∧ h_{a,b}(y) = j] \leq 4 / m^2$, kde $x,y \in [p]$ a $i, j \in [m]$ a nahodnou dvojici $(a,b)$
- obe strany posoudime nezavisle a pak jejich pravdepodobnost pronasobime
- pravdepodobnost jedne strany je maximalne $2p/mp$
- pravdepodobnost obou zaroven je maximalne $(2p/mp)^2 = 4/m^2$
- tudiz je stale 2-nezavisly

## 🟣Sestrojte k-nezávislý systém hešovacích funkcí ze $Z_p$ do [m].

- Necht $H$ je (k,c)-nezavisla rodina hash funkci z $U$ do $[r]$

🔴 TODO

## 🟣Sestrojte 2-nezávislý systém hešovacích funkcí hešující řetězce délky nejvýše L nad abecedou [a] do [m].

- pouzijeme poly-mod-prime
  - $h_{a, b, c}(x_1, ... x_d) = (b + c*\sum^{d-1}_{i=0}x_{i+1}*a^i \mod p) \mod m$ , $(p > m)$
- retezec doplnime zprava nulami do delky L
- pri vypoctu hashe muzeme ignorovat prazdne znaky, nebot 0 \* cokoliv je 0 a sumu to nezmeni

## 🟣Popište a analyzujte Bloomův filtr.

- Datova struktura
- Umi insert, neumi deleate a find dava false-positive
- pametove usporty
- Insert vklada na pozici zahashovane hodnoty
- False positive je n/m (n je pocet prvku, m je velikost nasi datove struktury)
- Multi-band (k hash funkci)
  - $k = ⌈\log 1 / \epsilon⌉$, kde $\epsilon$ je pravdepodobnost false-positive
  - potrebna pamet je $m ⌈\log 1 / \epsilon⌉$ , ($m$ muze byt treba $2n$)

## 🟣Ukažte, jak provádět 1-rozměrné intervalové dotazy na binárním vyhledávacím stromu.

- najdeme levy vrchol, najdeme pravy vrchol a vsechny vrcholi / podstromy mezi nimi vypiseme
- $O(\log n + p)$, kde $p$ je pocet vracenych vrcholu

## 🟣Definujte k-d stromy a ukažte, že 2-d intervalové dotazy trvají Ω($\sqrt{n}$).

- Binarni vyhledavaci strom
- na i-te hladine porovnavame i-tou dimenzi
- Build trva $O(n \log n)$

intervalovy dotaz maximalne $\sqrt n$:

- varovny priklad:
  - mame dotaz na 2d strom a hledame vsechny prvky na ose $y$
  - na kazde sude urovni se musi prochazet oba synove
  - pocet navstivenych listu je $2^{(\log n) / 2} = \sqrt n$

```
                              ⬤
|      *             ┌────────┴─────────┐
|     *              ⬤                 ◯
|    *           ┌────┴────┐        ┌───┴────┐
|   *            ⬤        ⬤       ◯       ◯
|  *            / \       / \      / \      / \
| *            ⬤   ◯   ⬤   ◯   ◯ ◯    ◯ ◯
|*             /\       /\
*            [⬤] ⬤     ⬤ ⬤

```

## 🟣Ukažte, jak dynamizovat k-rozměrné intervalové stromy (stačí Insert).

- amortizovane $O(\log^d n)$
- jeden insert prida
  - 1 vrchol v 1. dimenzi
    - insert do stromu 1. dimenze trva $O(\log n)$, (vcetne lineho vyvazovani)
    - bohuzel se obcas musi ppostavit cely strom v 2. dimenzi coz trva $O(n \log n)$
  - $O(\log n)$ vrcholu v 2. dimenzi
    - insert do stromu 2. dimenze trva $O(\log n)$
    - takze v 2. dimenzi mame celkovou slozitost $O(\log^2 n)$
  - ... v dalsich dimenzich

## 🟣Ukažte, jak použít suffixové pole a LCP pole na nalezení nejdelšího společného podřetězce dvou řetězců.

- spojime retezce $\alpha$, "#" a $\beta$, kde # je novy symbol do retezce $\alpha$#$\beta$
- sestrojme pro tento retezec LCP
- hledame dvojici po sobe jdoucich LCP indexu $i$ a $j$, takovou ze:
  - $i$ je pred znakem # a $j$ je za znakem #, nebo presne obracene
- vratime dvojici ktera bude mit nejvetsi spolecny podretezec
- slozitost $O(n)$

## 🟣Ukažte, jak paralelizovat (a,b)-strom.

- pouzijeme top-down balancovani
- INSERT:
  - drzime zamek na aktualnim vrcholu a jeho rodici
  - pokud je potreba tak vrchol rozdelime
  - pote odemkneme rodice
  - zamkneme dite ktere potrebujeme a pokracujeme tam
- DELETE:
  - stejne jako insert
  - ale zamykame i bratra, kdyby nahodou bychom jej potrebovali pripojit
    - tady muze nastat deadlock, tak musime vybirat systematicky, treba jen presne o jedna leveho bratra
  - pro odstraneni ne-listu se muze stat, ze potrebujeme zamknout root pri hledani vhodneho potomka
    - radeji vytvorit pomnik misto mazani

## 🟣Navrhněte a analyzujte bezzámkovou implementaci zásobníku.

- Mejme zasobnik, ktery ma hodnotu, atomicky ukazatel na naslednika a atomicky ukazatel na hlavu

PUSH

```{.c .numberLines}
Repeat:
  $h <- stack.head
  n.next <- h
  if CAS(stack.heead, h, n) = h: return
```

POP

```{.c .numberLines}
Repeat:
  h <- stack.head
  s <- h.next
  if CAS(stack.heead, h, s) = h: return h
```

- muze nastat _livelock_, ale velmi nepravdepodobne

## 🟣Popište atomická primitiva a jejich vlastnosti. Vysvětlete problém ABA a jeho řešení.

- Atomicita znamena, ze ostatni danou vec vydi jako nezapocatou, nebo dokoncenou
- primitiva:
  - Read and write - normalni pamet RAM
  - Exchange - vymeni hodnoty atomickeho registru a lokalni pameti
  - Test and set bit - nastavi bit na hodnotu a vrati puvodni
  - Fetch and add - pripocte cislo a vrati puvodni hodnotu
  - Compare and swap (CAS) - dostane old a new, pokud je v registru old, tak ho vymeni za new a puvodni hodnotu vrati
  - Load linked and store conditional (LL/SC) - po precteni se prida watcher, ktery pri zapisu povoli zapsat jen pokud jej nikdo nemenil

ABA

- problem s bezzamkovym zasobnikem, kde muze nastat pridani do zasobniku
- CAS sice uvidi tu samou hodnotu, ale ta byla 2x zmenena mezitim
- vyresi se pouzitim LL/SC misto CAS
  - nebo double CAS

priklad:
Program:

```{.c .numberLines}
x <- POP
y <- POP
PUSH(x)
```

Stav pred: `head -> A -> B -> C -> null`

| Vlakno 1 | Vlakno 2 |         |
| :------: | :------: | :-----: |
| x <- POP |          |   x=A   |
| y <- POP |          |   y=B   |
|          | x <- POP |   x=C   |
|          | y <- POP |   y=D   |
| PUSH(x)  |          | push(A) |
|          | PUSH(x)  | push(C) |

Stav po: `head -> C -> A -> null`
Seriove ma spravne byt: `head -> A -> D -> null`
