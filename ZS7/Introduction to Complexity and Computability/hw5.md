~David~ ~Napravnik~

---

# 5. HW

## 1) Show that problem Vertex Cover is polynomial time reducible to problem Dominating Set.

Musime upravit graf tak, ze:

- Odebereme osamocene vrcholy
- Kazdou hranu doplnime o novy vrchol a propojime jej s puvodnimi vrcholy dane hrany.

Tudiz pokud existuje hrana, kterou vrcholove pokryti "nevydi", tak existuje vrchol (vytvoreny z teto hrany), takovy, ze neni sousedem vrcholu vmnozite $S$.

![graphTransformation](./graphTransformation.svg)

## 2 a) Show that problem Partition is polynomial time reducible to the Knapsack problem.

Vpodstate problem hledani dvou hromadek prevedeme na hledani jedne s tim, ze zbytek bude jakoby ta druha hromadka.
Pozmenime B a K tak aby prijimaly pouze reseni, kdy $\sum_{a\in A} s(a) = 2B = 2K$

- Mnozina A zustane stejna.
- Ceny zustavaji stejne: $v(a) = s(a)$
- Cenu batohu nastavime rovnou polovine celkove ceny: $K = \frac{1}{2} \sum_{a\in A} s(a)$
- Kapacitu nastavime stejne: $B = \frac{1}{2} \sum_{a\in A} s(a)$

## 2 b) Show that problem Partition is polynomial time reducible to the Scheduling problem.

Dva batohy prevedeme na dva procesory a budeme chtit, aby dobehli v presne dany moment.

- Mnozina zustava stejna: $U=A$
- Procesory potrebujeme dva: $m=2$
- Casova slozitost, bude ekvivalentni cene: $d(a) = s(a)$
- Cas stanovyme jako polovinu cen: $D = \frac{1}{2} \sum_{a\in A} s(a)$
