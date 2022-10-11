# 2.cviko

## agregacni metoda


## Penizkova metoda
setrime si operace a pak je vyuzijeme az bude potreba

napr. pro insert chceme 3 operace,
1 -> insert
2 -> si ulozime
az budeme chtit prodlouzit pole, penizku na to bude dost

## bankovni metoda
to same jako penizkova, jen si penizky davame do banky

## priklady
### 1
- budeme vybirat ⎡*k/p*⎤ penizku; amortizovanost selze pro p>k
- budeme vybirat *p* penizku; amortizovanost zustava
- budeme vybirat *k* penizku; 

### 2
pro pridavani chceme 3 penizky
pro mazani nam staci 1 (vymazat polovinu pole je zdarma, jinak az budeme na ctvrtine)

### 3
to same jako penizkova metoda jen pridame konstrantu oznacujici posun zacatku pole.
pole nechame pretekat a az se potka konec se zacatkem tak budeme alokovat nove

### 4
budeme si setrit tolik penizku jak dlouhe cislo zatim mame
