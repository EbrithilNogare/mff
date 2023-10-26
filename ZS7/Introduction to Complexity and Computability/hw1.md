~David~ ~Napravnik~

-----
# 1. HW

#### 1
##### a)
Mejme turinguv stroj $M$ = (Q, $\sum$, $\delta$, $q_0$, F) takovy ze:
- mnozina stavu Q = {*increase*, *DONE*}
- abeceda $\sum$=\{0, 1, $\lambda$\}
- prechodova funkce $\delta$
- pocatecni stav $q_0 =$ *increase*
- mnozina koncovych stavu F = {*DONE*}

Pouzijeme jednostranou nekonecnou pasku, jejiz pravy krajni symbol je $\lambda$ a vlevo bude pred cislem nekonecne nul.

**Pro pricteni jednicky pouzijeme nasledujici princip:**
- zacneme ve stavu *increase* s hlavou ukazujici na posledni bit cisla
- pokud vidime 1 zapiseme 0 a jdeme doleva, ponechame stav *increase*
    - (tento krok opakujeme, dokud nezpropagujeme jednicku)
- pokud vidime 0 zapiseme 1 a prejdeme do stavu *DONE*

**Ukazka:**
| Vstup | 0 | 0 | 0 | 1 | [1] | $\lambda$ |
| :--: | :--: | :--: | :--: | :--: | :--: | :--: |
| Stav *increase* | 0 | 0 | 0 | [1] | 0 | $\lambda$ |
| Stav *increase* | 0 | 0 | [0] | 0 | 0 | $\lambda$ |
| Stav *DONE* | 0 | 0 | [1] | 0 | 0 | $\lambda$ |

##### b)
- $q_0$ = *increase*
- F = {*Done*}
- $\delta$:
    - $\delta$(increase, 0) = (goBack, 1, L)
    - $\delta$(increase, 1) = (increase, 0, R)
    - $\delta$(goBack, 0) = (goBack, 0, L)
    - $\delta$(goBack, 1) = (goBack, 1, L)
    - $\delta$(goBack, $\lambda$) = (DONE, $\lambda$, R)


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
