~David~ ~Napravnik~

-----
# 1. HW

#### 1
##### a)
Mejme turinguv stroj $M$ = (Q, $\sum$, $\delta$, $q_0$, F) takovy ze:
- mnozina stavu Q
- abeceda $\sum$=\{0, 1, $\lambda$\}
- prechodova funkce $\delta$
- pocatecni stav $q_0$
- mnozina koncovych stavu F = {*HOTOVO*}

Pouzijeme jednostranou nekonecnou pasku, jejiz krajni symbol je $\lambda$ a vpravo bude za cislem nekonecne nul.
Po dokonceni programu ze slusnosti vratime hlavu na zacatek.

**Pro pricteni jednicky pouzijeme nasledujici princip:**
- zacneme ve stavu *increase* s hlavou ukazujici na prvni bit cisla
- pokud vidime 1 zapiseme 0 a jdeme doprava, ponechame stav *increase*
    - (tento krok opakujeme, dokud nezpropagujeme jednicku)
- pokud vidime 0 zapiseme 1 a prejedeme hlavou na zacatek pasky, *HOTOVO*

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
