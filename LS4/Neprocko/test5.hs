-- 3. Haskell: Otočení v orientované sekvenci. Na vstupu je daný seznam S obsahující dvojice (položka, orientace), kde položky jsou obecné informace nějakého typu (například geny v chromozomu), a orientace je typu Bool (pro sousměrně a protisměrně). Volání funkce otoceni S má vydat seznam všech výsledků Vs jako seznam seznamů dvojic stejného typu, kde jeden výsledek vznikne otočením nějaké souvislé části S, přičemž v otočené části změníte informaci o směru. Délka otočené části je od 1 do délky S, tj. otáčenou spojitou část vybíráte všemi možnými způsoby.
-- a) Napište (obecný) typ funkce otoceni
-- b) Napište funkci otoceni
-- c) Pracovala by Vaše implementace funkce otoceni na nekonečném vstupním seznamu? Šla by napsat správná implementace pro nekonečný seznam? (Stačí myšlenka: proč ano nebo proč ne.)
-- Příklad.
-- > otočení [('a',True),('b',True),('c',False)]
-- [[('a',False),('b',True),('c',False)],[('a',True),('b',False),('c',False)],[('b',False),('a',False),('c',False)],[('a',True),('b',True),('c',True)],[('a',True),('c',True),('b',False)],[('c',True),('b',False),('a',False)]]

-- a)
otoceni :: [(a,Bool)] -> [[(a,Bool)]]

-- b)
otoceni [] = [[]]
otoceni xs = as : case bs of
    [] -> []
    _ -> otoceni bs
    where
        (as, bs) = splitOtoceni xs
        
splitOtoceni :: [(a,Bool)] -> ([(a,Bool)],[(a,Bool)])
splitOtoceni [] = ([],[])
splitOtoceni [x] = ([x], [my_not x])
splitOtoceni ((a,aB):b:xs)
    | not aB  = let (x, y) = splitOtoceni (b:xs) in ((a,aB):x, y)
    | otherwise = ([(a,aB)], b:xs)
        
my_not :: (a,Bool) -> (a,Bool)
my_not (a, b) = (a, not b)

main = do 
    print $ otoceni [('a',True),('b',True),('c',False)]

-- c) ano fungovalo, protoze funkce splitOtoceni bere prvky postupne a zbytek vraci zpatky do otoceni

-- Komentář:
-- - b) asi jste řešil jinou úlohu anebo Vám chybí jedna úroveň zpracování. Nevidím nikde vlastní otáčení části seznamu.

-- body: 3/10
