~David~ ~Napravnik~

-----
# 3. HW

#### 1 Show that the following problem is algorithmically undecidable
Pro dukaz pouzijeme redukci na problem zastaveni, ktery vime ze je nerozhodnutelny.

Sestrojime turinguv stroj *M*, ktery bude simulovat beh jineho stroje *M'* na vstupu *x*.
Pokud se *M'* zastavi, *M* vypise na duhou pasku symbol $\alpha$, jinak vypisuje $\lambda$ (nevyupisuje nic).

Pokud by tedy byl tento problem algoritmicky rozhodnutelny, pak by slo algoritmicky rozhodnout i problem zastaveni, coz nelze, tudiz i tento problem je algoritmicky nerozhodnutelny.

#### 2. a)  Show that $L_u \leq_m S$

Sestrojime funkci $ f: \sum^* \rightarrow \sum^*$ takovoum ze $\forall x \in \sum^*: x \in L_u \text{ iff } f(x) \in S$ 

Funkce $f$ sestroji turinguv stroj $M_x$ ktery vstup $x$ prijima, pouze pokud $x=\langle M_x\rangle$.


$L_u =\{ \langle M\rangle ∣ M$ accepts $\langle M\rangle \}$.




#### 2. b)  Show that $L_u \leq_m \bar{S}$



