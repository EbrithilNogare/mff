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
- Nektere vlakno musi dobehnou, i kdyz pro jine muze nastat live-lock.
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
## Popiste hesovaci system multiply-mod-prime a scalar-mod-prime a analyzujte universalitu a nezavislost.

## Popiste hesovani s linearnim pridavanim a analyzujte jeho slozitost.

## Popiste hesovani se separovanymi retezci. Analyzujte slozitosti operaci a delku nejdelsiho retezce.

## Popiste kukacci hesovani a operace.

## Popiste tabulkove hesovani a analyzujte jeho nezavislost.

## Popiste zpusoby hesovani retezcu.

## Rozeberte souvislost techto pojmu

## Vytvorte k-nezavisly hesovaci system.

# Napiste ruzne algoritmy na transpozici matic a analyzujte jejich slozitost.

# Popiste vkladani a mazani do dynamickeho pole a analyzujte slozitost.

# Popiste zpusoby vyhodnocovani intervalovych dotazu.

# Vysvetlete rozdil mezi algoritmy v cache-oblivious a cache-aware modelu a uvedte priklady jejich analyzy.
