# Definujte (a, b)-stromy a cerveno-cerne stromy a srovnejte je.
- (A, B)-strom je vyhledavaci strom
- vrcholy maji alespon A a maximalne B synu
- pro koren neplati A pravidlo
- vseechny listy jsou ve stejne vysce
## Popiste operace v (a, b)-stromech a analyzujte jejich slozitost.
- insert = O(B * log_A N)
- delete = O(B * log_A N)
- find = O(log B * log_A N), O(log N) pokud B=poly(A)
## Popiste pouziti (a, b)-stromu v paralelnim programovani.
- Pokud plati B ≥ 2A
- muzeme paralelizovat find, insert i delete
- preventivni rozdelovani pri findu
# Definujte Splay stromy.
- Binarne vyhledavaci strom
- Nedavne prvky jsou rychle dostupne
## Napiste vazenou analyzu a uvedte jeji dusledky

## Popiste operace a analyzujte jejich slozitost
amortizovane:
- find O(log N)
- insert O(log N)
- delete O(log N)
# Definujte sufixove a LCP pole a strom.
- Sufoxova pole udava lexikograficke poradi sufixu daneho slova (a rika kde sufix ve slove zacina) 
- LCP(A, B) = nejdelsi spolecny prefix A a B
- Sufixovy strom = trie obsahujici sufixi, kde synove jsou lexikograficke poradi
## Popiste jejich konstrukci a analyzujte slozitost.
Sufixove pole
- udelat binarni vyhledavaci strom sufixu
- indexy ze sufixoveho stromu do pole (zplacatit strom)
- vytvoreni v O(n) (counting sort, bucket sort pro normalni abecedy)
LCP:
- vezmeme dva porovnavane prvky (lexikograficky hned za sebou), najdeme nejnizsi spolecneho rodice a jeho hloubka je rovna LCP
## Popiste aplikace
- nejdelsi opakujici se podretezec
- pocet ruznych podretezcu velikosti k
- najit jehlu v kupce sena 
# Definute intervalove stromy.
- vyhledavaci strom ve kterem nalezneme vrcholi A a B a vse mezi nimi bude odpoved inervalem
## Popiste vkladani a mazani a analyzujte slozitost techto operaci.
- insert - O(log N) - klasicky jako v BVS
- delete - O(log N) - klasicky jako v BVS
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
- Vahove vyvazeny strom
- Binarne vyhledaveci strom
- pocet vrcholu jednoho podstromu je nejvyse α nasobkem druheho podstromu (podstromy synu jednoho rodice)
# Napiste definici universalniho a k-nezavisleho hesovaciho systemu.
- Universalni = hesovaci funkce, ktera funguje "vetsinou dobre"
- c-universalni = pocet | h(x) = h(y) | je nejvyse c * |H| / m , pro vsechna x, y

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
- multiply-mod-prime: ax + b % p; (2,1)-nezavisly; (2,c)-nezavisly -> 2c universalni
- scalar-mod-prime: Hesovani posloupnosti pevne delky 
- - sum(a_i * x_i) % p = 1-universalni
- - b + sum(a_i * x_i) % p = (2,1)-nezavisly
- - b + sum(a_i * x_i) % p % m = (2,4)-nezavisly
## Popiste hesovani s linearnim pridavanim a analyzujte jeho slozitost.

## Popiste hesovani se separovanymi retezci. Analyzujte slozitosti operaci a delku nejdelsiho retezce.
- tabulka velikosti m
- hesovaci funkce h: U-> M

## Popiste kukacci hesovani a operace.
- 2 hesovaci funkce
- amortizovane O(1)
## Popiste tabulkove hesovani a analyzujte jeho nezavislost.
- bitovy zapis rozdelime na D casti
- kazdou cast zahesujeme ruznou hash funkci
- nakonci casti spojime pomoci XORu
- 3-nezavisle
## Popiste zpusoby hesovani retezcu.
- pevna delka: scalar-mod-prime = b + sum(a_i * x_i % p) % m
- ruzna delka: poly-mod-prime - b + c * sum(x_i * a^i % p) % m
## Rozeberte souvislost universalniho a k-nezavisleho hesovaciho systemu

## Vytvorte k-nezavisly hesovaci system.

# Napiste ruzne algoritmy na transpozici matic a analyzujte jejich slozitost.
- trivialni
- cache-aware
- cache-obvious
# Popiste vkladani a mazani do dynamickeho pole a analyzujte slozitost.
- insert prida prvek a pokud neni misto, tak zdvojnasobi pole - amortizovane O(1)
- delete odebere prvek a pokud je moc volneho mista, tak pole zmensi - amortizovane O(1)
# Popiste zpusoby vyhodnocovani intervalovych dotazu.

# Vysvetlete rozdil mezi algoritmy v cache-oblivious a cache-aware modelu a uvedte priklady jejich analyzy.
