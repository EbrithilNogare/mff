~David~ ~Napravnik~

-----
# 2. HW

#### 1
##### a) Formulate the problem as a language ETx

Jazyk ETx bude obsahovat dvojice *(M, x)*, kde *M* je turinguv stroj a *x* je jeho vstup.
$ETx = \{M, x | M$ zastavi se vstupu x a na konci bude mit prazdnou pasku$\}$

##### b) Show that language ETx is partially decidable by describing a Turing machine accepting it

Tento Turinguv stroj $T$ bude simulovat beh Turingova stroje $M$ se vstupem x.
Pokud se simulace $M$ zastavi a ma na konci prazdou pasku, tak $T$ prijme dvojici $(M, x)$, jinak ji neprijme (nebo se nezastavi).

ETx je castecne rozhodnutelne protoze bud simulace se vstupem x skonci a je prijat / neprijat, nebo se simulace nezastavi.

##### c) Show that language ETx is not decidable

Predpokladejme ze ETx je rozhodnutelny.

Zredukujeme ETx na univerzalni jazyk $L = \{M', x | x \in L(M)\}$.

Jelikoz je rozhodnutelny, tak musi existovat algoritmus $A$, ktery rozhodne ETx.
Dejme nasemu Turingovu stroji $M$ dvojici $\{M', x\}$, **pak se $M$ zastavi** a prijme / neprijme. 

Takze jsme ukazaly, ze jazyk $L$ je rozhodnutelny, coz je **spor** s tvrzeni "univerzalni jazyk $L$ neni rozhodnutelny" $\square$


#### 2

##### a) Formulate the problem as a language ET

$ET = \{M |$ $\exist x: M$ se vstupem $x$ se zastavi a bude mit na konci prazdou pasku$\}$

##### b) Show that language ET is partially decidable

Mejme turinguv stroj $M$ (a pocatecni parametry ($i = k = 1$)).
Pro kazdou kombinaci vstupu $x$ a omezenim na $k$ instrukci spustime simulaci stroje $M$ se vstupem $x_i$ a omezenim $k$:
- Pokud simulace zastavi a ma na konci prazdnou pasku prijmeme $M$
- Pokud simulace zastavi a nema na konci prazdnou pasku tak neprijmeme $M$
- Pokud simulace nezastavi ... tak ji zastavime po $k$ krocich
  - Pokud $k == i$ nastavime $k$++ a $i=1$
  - Jinak nastavime $i$++

Jazyk $ET$ je castecne rozhodnutelny, protoze pokud existuje vstup $x$ ktery zastavi a ma prazdnou pasku, tak simulace tento vstup najde a zastavi se. Dokonce muzeme rict, ze se zastavi po maximalne $ki^2$ instrukcich
