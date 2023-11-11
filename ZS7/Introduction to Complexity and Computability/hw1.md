~David~ ~Napravnik~

-----
# 1. HW

#### 1
##### a)
Mejme turinguv stroj $M$ = (Q, $\sum$, $\delta$, $q_0$, F) takovy ze:
- mnozina stavu Q = {*goBack*, *increase*, *DONE*}
- abeceda $\sum$=\{0, 1, $\lambda$\}
- prechodova funkce $\delta$
- pocatecni stav $q_0 =$ *goBack*
- mnozina koncovych stavu F = {*DONE*}

Pouzijeme jednostranou nekonecnou pasku, jejiz pravy krajni symbol je $\lambda$ a vlevo bude pred cislem nekonecne $\lambda$.

**Pro pricteni jednicky pouzijeme nasledujici princip:**
- zacneme ve stavu *goBack* s hlavou ukazujici na prvni bit cisla
- posuneme hlavu na posledni bit cisla a prejdeme do stavu *increase* 
- pokud vidime 1 zapiseme 0 a jdeme doleva, ponechame stav *increase*
    - (tento krok opakujeme, dokud nezpropagujeme jednicku)
- pokud vidime 0 zapiseme 1 a prejdeme do stavu *DONE*

**Ukazka:**
| Vstup | $\lambda$ | $\lambda$ | $\lambda$ | [1] | 1 | $\lambda$ |
| :--: | :--: | :--: | :--: | :--: | :--: | :--: |
| Stav *goBack* | $\lambda$ | $\lambda$ | $\lambda$ | 1 | [1] | $\lambda$ |
| Stav *goBack* | $\lambda$ | $\lambda$ | $\lambda$ | 1 | 1 | [$\lambda$] |
| Stav *increase* | $\lambda$ | $\lambda$ | $\lambda$ | 1 | [1] | $\lambda$ |
| Stav *increase* | $\lambda$ | $\lambda$ | $\lambda$ | [1] | 0 | $\lambda$ |
| Stav *increase* | $\lambda$ | $\lambda$ | [$\lambda$] | 0 | 0 | $\lambda$ |
| Stav *DONE* | $\lambda$ | $\lambda$ | [0] | 0 | 0 | $\lambda$ |

##### b)
- $q_0$ = *goBack*
- F = {*Done*}
- $\delta$:
    - $\delta$(goBack, 0) = (goBack, 0, R)
    - $\delta$(goBack, 1) = (goBack, 1, R)
    - $\delta$(goBack, $\lambda$) = (increase, $\lambda$, L)
    - $\delta$(increase, 1) = (increase, 0, L)
    - $\delta$(increase, 0) = (Done, 1, N)
    - $\delta$(increase, $\lambda$) = (Done, 1, N)


#### 2
Mejme turinguv stroj $M$ = ($Q$, $\sum$, $\delta$, $q_0$, F) takovy ze:
- abeceda $\sum$=\{a..z\}
- instrukce Z=\{L, R\}
- stavy Q={$\alpha$ ... $\delta$}

Pak $M'$ = (Q', $\sum$, $\delta$', $q_0$, F') bude turinguv stroj takovy ze:
- abeceda $\sum$ zustava stejna
- instrukce dostanou moznost nedelat nic: Z'=\{L, R, N\}
- stavy ze prenasobenim instrukcemi ztrojnasobi na Q'=\{$\alpha$, $\alpha L$, $\alpha R$ ... $\delta$, $\delta L$, $\delta R$\}
- prechodova $\delta$' funkce se zmeni z:
    - $\delta$(q, c) = (q', c', Z)
	na:
    - $\delta$(q, c) = (qZ, c', N)
    - $\delta$(qZ, c') = (q', , Z)
