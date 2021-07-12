import Functional

--DFS a BFS v haskellu

--reprezentace seznamem nasledniku
data Graf a = G [a] [(a,[a])]

testGraf = G ['a', 'b', 'c', 'd', 'e', 'f'] [('a', ['b', 'c']), ('b', ['d']), ('c', ['b','e']), ('d', []), ('e',['c']), ('f',['d','e'])]

vrcholy :: Graf a -> [a]
vrcholy (G v e) = v

hrany :: Graf a -> [(a,a)]
hrany (G v e) = [(v,naslednik) | (v,naslednici) <- e, naslednik <- naslednici]

hranyZ :: Eq a => a -> Graf a -> [(a,a)]
hranyZ v g = filter(\(i,j) -> i == v) (hrany g)

naslednici :: Eq a => a -> Graf a -> [a]
naslednici u (G v e) = case idx of
                            Just i  -> snd (e !! i)
                            Nothing -> error "Neplatny vrchol"
            where idx = findIndex (\x -> fst x == u) e

--dfs najde vsechny cesty (v dfs strome)           
dfs :: Eq a => Graf a -> a -> a -> [[a]]
dfs g u v = dfs' g u v [u]

dfs' :: Eq a => Graf a -> a -> a -> [a] -> [[a]]
dfs' g u w xs@(z : _) | z == w && length xs > 1     = [reverse xs]
                      | null kam                    = []
                      | otherwise                   = concat $ map (\k -> dfs' g k w (k : xs)) kam
            where kam = filter (\x -> not $ elem x xs) $ map snd (hranyZ u g)


--bfs najde nejkratsi cestu
bfs :: Eq a => Graf a -> a -> a -> [a]
bfs g u v = bfs' g v [u] [ [u] ]

bfs' :: Eq a => Graf a -> a -> [a] -> [[a]] -> [a]
bfs' g v _ [] = []
bfs' g v visited (p@(u : _) : paths) | length p > 1 && u == v      = reverse p
                                     | otherwise                   = bfs' g v (visited ++ ws) (paths ++ cesty)
            where
                ws = filter(\x -> not $ elem x visited) $ map snd (hranyZ u g) --kam muzu
                cesty = map (\w -> w : p) ws
