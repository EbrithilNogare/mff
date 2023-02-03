# Definujte (a, b)-stromy a cerveno-cerne stromy a srovnejte je.
- A, B jsou cela cisla splnujici a ≥ 2 a b ≥ 2a-1
- - vnitrni vrcholy maji alespon A a maximalne B synu
- - vseechny listy jsou ve stejne vysce
- Cerveno-cerne stromy
- - binarni vyhledavaci strom
- - kazdy list obsahuje stejny pocet cernych rodicu (cesta do korene)
- - kazdy cerveny vrchol ma cerneho otce
## Popiste operace v (a, b)-stromech a analyzujte jejich slozitost.
- insert = O(B * log_A N)
- delete = O(B * log_A N)
- find = O(log B * log_A N), O(log N) pokud B=poly(A)
## Popiste pouziti (a, b)-stromu v paralelnim programovani.
- Pokud plati B ≥ 2A
- muzeme paralelizovat find, insert i delete
- preventivni rozdelovani pri insert a spojovani pri delete
# Definujte Splay stromy.
- Binarne vyhledavaci strom
- Nedavne prvky jsou rychle dostupne
- rotace
- - zig = x je v levem podstromu
- - zag = x je v pravem podstromu
- - zig zag
- - zig zig
## Napiste vazenou analyzu a uvedte jeji dusledky
- amortizovane jedna operace SPLAY je O(log n)
- celkove K operaci SPLAY je O((n+k) log n)
## Popiste operace a analyzujte jejich slozitost
amortizovane:
- find O(log N)
- insert O(log N)
- delete O(log N)
# Definujte sufixove a LCP pole a strom.
- Sufixova pole udava lexikograficke poradi sufixu daneho slova (a rika kde sufix ve slove zacina) 
- LCP(A, B) = nejdelsi spolecny prefix A a B
- Sufixovy strom = trie obsahujici sufixi, kde synove jsou v lexikografickem poradi
## Popiste jejich konstrukci a analyzujte slozitost.
Sufixove pole
- udelat strom sufixu
- indexy ze sufixoveho stromu do pole (zplacatit strom)
- vytvoreni v O(n) (counting sort, bucket sort pro normalni abecedy)
LCP:
- vezmeme dva porovnavane prvky (lexikograficky hned za sebou), najdeme nejnizsi spolecneho rodice a jeho hloubka je rovna LCP
- z sufixoveho stromu splostenim
## Popiste aplikace
- nejdelsi opakujici se podretezec
- pocet ruznych podretezcu velikosti k
- najit jehlu v kupce sena 
# Definute intervalove stromy.
- binarni vyhledavaci strom podle X
- kazdy vrchol bude mit vlastni binarni vyhledavaci strom podle Y, vsech svych podvrcholu
- v kazdem vrcholu je jeden bod
- vyhledavaci strom ve kterem nalezneme vrcholi A a B a vse mezi nimi bude odpoved inervalem
## Popiste vkladani a mazani a analyzujte slozitost techto operaci.
- insert - O(log N) - klasicky jako v BVS
- delete - O(log N) - klasicky jako v BVS
- find - O(n) pokud vracime vse a O(log N) pokud vracime jen vlstnost skupiny (velikost treba)
# Lock-free programming
- Nektere vlakno musi dobehnou, i kdyz pro jine muze nastat live-lock
- pomoci atomickych operaci
- test and set operace
- LL | SC = load lock | store condition (zapise jen pokud to co precetl predtim se nezmenilo a necetlo)
- CAS - Compare and swap (adresa, ocekavana hodnota, co kdyztak zapsat)
- CAS2 - muzeme zkontrolovat 2 prvky
- WCAS - 
# Napiste a dokazte vetu Sleator-Tarjan o kompetitivite LRU a OPT strategii vymeny stranek.
- Necht s1, ... , sk je posloupnost pristupu do pameti
- Necht P_opt a P_lru je pocet bloku v cache pro strategie opt a lru
- Necht F_opt a F_lru je pocet prenesenych bloku
- P_lru > P_opt
- Pak F_lru ≤ P_lru / ( P_lru − P_opt) * F_opt + P_opt

# Napiste definici BB[α]-stromu. Popiste operace a jejich slozitost.
- Binarne vyhledaveci strom
- Vahove vyvazeny strom
- pocet vrcholu kazdeho podstromu je nejvyse α nasobkem vsech vrcholu stromu (podstromy synu jednoho rodice)
- .5 < α < 1
# Napiste definici universalniho a k-nezavisleho hesovaciho systemu.
- Universalni
- - Mame hashovaci system H s hash funkcemi h
- - Plati P[h(x)=z] = 1/m pro vsechna x∈U a z∈M
- c-Universalni
- - pocet hashovacich funkci h ∈ H takovych ze h(x) = h(y) je nejvyse c * |H| / m , pro vsechna ruzna x, y
- - nahodne zvolena h∈H splnuje P[h(x)=h(y)] ≤ c/m pro kazde ruzne x, y
- (k,c)-nezavisly
- - Nech k∈N, K={1...k} a c ≥ 1
- - pokud nahodna h∈H splnuje:
- - P[h(x_i) = z_i ∀i ∈ K] ≤ c/m^k
- - pro vsechna po dvou ruzna x_1 ... x_k ∈ U a vsechna z_1 ... z_k ∈ M
- k-nezavisly =
- - je (k,c)-nezavisly pro nejake c
## Popiste Bloom filtry a analyzujte je.
- datova struktura ktera umi pridavat prvky a zjistovat zda dany prvek byl vlozen
- mame dlouhe klice, ktere se nevejdou do pameti
- staci ze spravne funguje vetsinu casu
- udelame pole a k hash funkci
- pri pridani prvku nastavime k-krat v poli jednicku  (indexi urci hash funkce)
- analyza:
- - pravdepodobnost nuly = (1-1/m)^kn
- - pravdepodobnost false positive = (1-p)^k = 0.7^(m/n); n=velikost mnoziny, m=velikost pole
## Popiste hesovaci system multiply-mod-prime a scalar-mod-prime a analyzujte universalitu a nezavislost.
- multiply-mod-prime: ax + b % p; (2,1)-nezavisly;
- scalar-mod-prime: Hesovani posloupnosti pevne delky 
- - sum(a_i * x_i) % p = 1-universalni
- - b + sum(a_i * x_i) % p = (2,1)-nezavisly
- - b + sum(a_i * x_i) % p % m = (2,4)-nezavisly
## Popiste hesovani s linearnim pridavanim a analyzujte jeho slozitost.
- zahesujeme prvek a pokud se da vlozit do pole tak jej vlozime, kdyz ne, tak pricteme 1, modulo M a zkusime to znovu
## Popiste hesovani se separovanymi retezci. Analyzujte slozitosti operaci a delku nejdelsiho retezce.
- neboli retizkove heshovani
- tabulka velikosti m
- hesovaci funkce h: U-> M
- nejdelsi retezec: P[X>c] < e^(cu-u) / c^cu
## Popiste kukacci hesovani a operace.
- 2 hesovaci funkce
- amortizovane O(1)
## Popiste tabulkove hesovani a analyzujte jeho nezavislost.
- bitovy zapis rozdelime na D casti
- kazdou cast zahesujeme ruznou hash funkci
- nakonci casti spojime pomoci XORu
- 3-nezavisle
## Popiste zpusoby hesovani retezcu.
- pevna delka: scalar-mod-prime = b + sum(x_i * a_i % p) % m
- ruzna delka: poly-mod-prime = b + c * sum(x_i * a^i % p) % m
## Rozeberte souvislost universalniho a k-nezavisleho hesovaciho systemu
- (2,c)-nezavisly -> 2c universalni
## Vytvorte k-nezavisly hesovaci system.
- pomoci poly-mod-prime
- bude (k,2)-nezavisly pro p ≥ 2km
# Napiste ruzne algoritmy na transpozici matic a analyzujte jejich slozitost.
- trivialni
- cache-aware
- cache-obvious
# Popiste vkladani a mazani do dynamickeho pole a analyzujte slozitost.
- insert prida prvek a pokud neni misto, tak zdvojnasobi pole - amortizovane O(1)
- delete odebere prvek a pokud je moc volneho mista, tak pole zmensi - amortizovane O(1)
# Popiste zpusoby vyhodnocovani intervalovych dotazu.

# Vysvetlete rozdil mezi algoritmy v cache-oblivious a cache-aware modelu a uvedte priklady jejich analyzy.
