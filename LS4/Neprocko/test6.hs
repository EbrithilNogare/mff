-- 4. Haskell: Doplnění hypergrafu (3 podotázky) 
-- Hypergraf je zadán množinou vrcholů a množinou hyperhran, což jsou alespoň dvouprvkové podmnožiny množiny vrcholů. Naší cílem je definovat funkci doplnění, která doplní do hypergrafu H všechny dvouprvkové (hyper)hrany pro ty dvojice vrcholů, které nejsou společně obsaženy v žádné hyperhraně vstupního hypergrafu H. Funkce tedy např. z hypergrafu
-- • s vrcholy {1,2,3,4,5} a hyperhranani {1,3,5} a {2,3,4}
-- • vytvoří hypergraf se stejnými vrcholy a hyperhranami {1,3,5},{2,3,4},{1,2},{1,4},{5,2} a {5,4}
-- (a) Definujte datový typ pro reprezentaci hypergrafu. Pokuste se o co nejobecnější definici (vrcholy mohou být reprezentovány nejen čísly, ale i znaky, řetězci apod.)
-- (b) Specifikujte typovou signaturu funkce
-- doplneni ::
-- (c) Funkci definujte.

kombinace :: (Eq a, Integral b) => b -> [a] -> [[a]]
kombinace 0 _ = [[]]
kombinace _ [] = []
kombinace n (x : xs) | n > 0 = kombinace n xs ++ map (x:) (kombinace (n-1) xs)

-- a)
data Hypergraf a = Hypergraf [a] [[a]] deriving Show

prvek :: Eq a => [a] -> [[a]] -> Bool
prvek _ [] = False
prvek x (y : ys) = (x == y) || (reverse x == y) || (prvek x ys)

rozdil :: Eq a => [[a]] -> [[a]] -> [[a]]
rozdil xs ys = foldr (\x a -> if prvek x a then a
                              else if not $ prvek x ys then (x : a)
                                   else a
                         ) [] xs

-- b), c)
doplneni :: Eq a => Hypergraf a -> Hypergraf a
doplneni (Hypergraf xs ys) = Hypergraf xs (ys ++ dalsiHrany)
        where
             vsechnyMozneHrany = kombinace 2 xs
             vnitrniHrany = foldr (\hrana ak -> ak ++ kombinace 2 hrana) [] ys
             dalsiHrany = rozdil vsechnyMozneHrany vnitrniHrany

main = do 
    print $ doplneni(Hypergraf [1,2,3,4,5] [[1,3,5],[2,3,4]])

-- Komentář:
-- - možná by byl stručnější program bez explicitní konstrukce dvojic z hyperhran.

-- body: 10/10
