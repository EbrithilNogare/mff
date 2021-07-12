import Functional

--dokonce v haskellu mam i dijkstru

data Graf a = G [a] [(Int,a,a)]

testGraf = G ['a','b','c','d','e','h'] [(1, 'a', 'b'), (2, 'a', 'd'), (10,'b','c'), (50, 'b','d'),(4,'b', 'e'), (3,'d','c'), (1,'e','h'), (5,'h','a')]

vrcholy :: Graf a -> [a]
vrcholy (G v _) = v

hrany :: Graf a -> [(Int,a,a)] 
hrany (G _ e) = e

hranyZ :: Eq a => a -> Graf a -> [a]
hranyZ v (G _ e) = foldr (\(w,x,y) a -> if x == v then y : a
                                        else a) [] e

hrana :: Eq a => a -> a -> Graf a -> (Int,a,a)
hrana x y (G _ e) = case i of
                        Just idx -> e !! idx
                        Nothing  -> error "Takove hrana neexistuje."
            where
                i = findIndex (\(w,a,b) -> a == x && b == y) e

ohodnoceniHrany :: (Int,a,a) -> Int
ohodnoceniHrany (w,x,y) = w
                              --cesta s min  nove fronta
extractMin :: [(Int, [a])] -> ((Int, [a]), [(Int, [a])])
extractMin xs | null xs     = error "Prázdná fronta."
              | otherwise   = (head xs, tail xs)

sortedInsert :: (Int, [a]) -> [(Int, [a])] -> [(Int, [a])]
sortedInsert z (x : y : xs) | fst x <= fst z && fst z <= fst y      = (x : z : y : xs)
                            | fst z > fst y && null xs              = (x : y : z : [])
                            | fst z < fst x                         = (z : x : y : xs) 
                            | otherwise                             = (x : sortedInsert z (y : xs))

soucasnaNejkCesta :: Eq a => a -> [(Int, [a])] -> (Int, [a])
soucasnaNejkCesta v ohod = case i of
                        Just idx -> ohod !! idx
                        Nothing  -> error "Neplatný vrchol."
            where
                i = findIndex (\(c,u) -> head u == v) ohod

dijkstra :: (Ord a, Eq a) => Graf a -> a -> a -> (Int, [a])
dijkstra (G vs es) u v = (cena, reverse cestaR)
            where
                inf = maxBound :: Int
                ohod = map (\v -> (inf, [v])) vs
                (cena, cestaR) = dijkstra' (G vs es) v ohod [ (0, [u]) ]

dijkstra' :: (Ord a, Eq a) => Graf a -> a -> [(Int,[a])] -> [(Int, [a])] -> (Int, [a])
dijkstra' g z ohod queue | null queue           = soucasnaNejkCesta z ohod
                         | otherwise            = dijkstra' g z noveOhod (novaFronta ++ noveFronta2)
            where
                (minCesta, novaFronta) = extractMin queue
                cesta = snd minCesta
                kdeJsem = head cesta
                ohodnoceniKdeJsem = fst minCesta
                sousedi = hranyZ kdeJsem g
                kamMuzu = foldr (\x a -> if ohodnoceniKdeJsem + ohodnoceniHrany (hrana kdeJsem x g) < (fst $ soucasnaNejkCesta x ohod) then x : a
                                         else a ) [] sousedi
                noveOhod = map (\(c,ws) -> let w = head ws in
                                           if elem w kamMuzu then (ohodnoceniKdeJsem + ohodnoceniHrany (hrana kdeJsem w g), w : cesta)
                                           else (c,ws) ) ohod
                                          
                noveFronta2 = map (\x -> (fst $ soucasnaNejkCesta x noveOhod, x : cesta)) kamMuzu

-- [(Int,a)] ohodnoceni vrcholu
-- [(Int, [a])] prioritni fronta