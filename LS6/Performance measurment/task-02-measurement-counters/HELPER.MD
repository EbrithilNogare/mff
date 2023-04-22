Floats:
    - nechat program bezet 60s
    - unroll 1000x instrukci nasobeni
    - asm() pomuze s kompilaci
    - best score: 7.1
Correct branch prediction:
    - `for(i;i<1000;i++)(ams("");)`
    - mega solution je spustit `while(true)` v jinem vlakne a pak ho po case zabit
    - best score: 1.0
Miss branch prediction:
    - predgenerovat si nahodna cisla (to zabere hromadu casu)
    - ty nacitat z pole, ktere se vejde do cache (single instruction load)
    - best score: 0.2
LLC
    - skakat napric pameti
    - nechat program bezet dele
    - megaa solution: AVX instrukce (nekolik missu v jedne instrukci)
    - best score: 18
