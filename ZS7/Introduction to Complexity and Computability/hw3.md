~David~ ~Napravnik~

---

# 3. HW

#### 1 Show that the following problem is algorithmically undecidable

Pro dukaz pouzijeme redukci na problem zastaveni, ktery vime ze je nerozhodnutelny.

Sestrojime turinguv stroj _M_, ktery bude simulovat beh jineho stroje _M'_ na vstupu _x_.
Pokud se _M'_ zastavi, _M_ vypise na duhou pasku symbol $\alpha$, jinak vypisuje $\lambda$ (nevypisuje nic).

Pokud by tedy byl tento problem algoritmicky rozhodnutelny, pak by slo algoritmicky rozhodnout i problem zastaveni, coz nelze, tudiz i tento problem je algoritmicky nerozhodnutelny.

#### 2. a) Show that $L_u \leq_m S$

Sestrojime funkci $ f: \sum^_ \rightarrow \sum^_$ takovoum ze $\forall x \in \sum^*: x \in L_u \text{ iff } f(x) \in S$

$f(\langle M,x\rangle ) = \langle N\rangle$, kde $N(y)$ ignoruje svuj vstup $y$ a jen simuluje $M(x)$, pokud prijme simuluje $M(x^r)$, pokud prijme i ten, tak prijima, jinak neprijima.
Plati tedy, ze $L(N) = \sum^*$, pokud $x\in L(M)$, jinak $L(N) = \emptyset$

#### 2. b) Show that $L_u \leq_m \bar{S}$

$f(\langle M,x\rangle ) = \langle N\rangle$, kde $N(y)$ ignoruje svuj vstup $y$ a jen simuluje $M(x)$, pokud zastavi a neprijme pak $N$ prijme, jinak simulujeme $M(x^r)$, ktere pokud zastavi a neprijme, tak $N$ prijme, jinak $N$ neprijme.
