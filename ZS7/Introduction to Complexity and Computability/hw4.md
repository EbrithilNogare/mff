~David~ ~Napravnik~

<style>
td, th {
   border: none!important;
}
</style>

---

# 4. HW

|     |                       |                                                                      |                       |                             |                       |                             |                       |
| --: | --------------------: | :------------------------------------------------------------------: | :-------------------- | :-------------------------: | :-------------------- | :-------------------------: | :-------------------- |
|  1) |            SPACE$(n)$ |                                  ?                                   | TIME$(2^{\log^3 n})$  |
|  2) |  TIME$(2^{\log^3 n})$ |                      $\stackrel{3}{\supseteq}$                       | NSPACE$(\log^2 n)$    |
|  3) |    NSPACE$(\log^2 n)$ |                      $\stackrel{3}{\subseteq}$                       | TIME$(2^{\log^3 n})$  | $\stackrel{1ii}{\subseteq}$ | NTIME$(2^{\log^3 n})$ |
|  4) | NTIME$(2^{\log^3 n})$ | $\stackrel{\log^3 n \leq n \log n \text{; for }n \geq 1}{\subseteq}$ | NTIME$(2^{n \log n})$ |
|  5) | NTIME$(2^{n \log n})$ |                                  ?                                   | SPACE$(n)$            |
|  6) |            SPACE$(n)$ |                      $\stackrel{5}{\supsetneq}$                      | SPACE$(\log^4n)$      |  $\stackrel{4}{\supseteq}$  | NSPACE$(\log^2 n)$    |
|  7) |  TIME$(2^{\log^3 n})$ |                     $\stackrel{1ii}{\subseteq}$                      | NTIME$(2^{\log^3 n})$ |
|  8) |    NSPACE$(\log^2 n)$ |                      $\stackrel{3}{\subseteq}$                       | TIME$(2^n)$           | $\stackrel{6}{\subsetneq}$  | TIME$(2^{n \log n})$  | $\stackrel{1ii}{\subseteq}$ | NTIME$(2^{n \log n})$ |
|  9) | NTIME$(2^{\log^3 n})$ |                                  ?                                   | SPACE$(n)$            |
| 10) | NTIME$(2^{n \log n})$ |                     $\stackrel{1ii}{\supseteq}$                      | TIME$(2^{n \log n})$  | $\stackrel{6}{\supsetneq}$  | TIME$(2^{\log^3 n})$  |
