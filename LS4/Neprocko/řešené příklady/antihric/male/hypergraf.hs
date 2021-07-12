import Kombinace

--http://forum.matfyz.info/viewtopic.php?f=169&t=11969&p=41802&hilit=hypergraf#p41802

data Hypergraf a = Hypergraf [a] [[a]] deriving Show

prvek :: Eq a => [a] -> [[a]] -> Bool
prvek _ [] = False
prvek x (y : ys) = jePrvek || jeObracenePrvek || prvek x ys
        where
            jePrvek = x == y
            jeObracenePrvek = reverse x == y

difference :: Eq a => [[a]] -> [[a]] -> [[a]]
difference xs ys = foldr (\x a -> if prvek x a then a
                                  else if not $ prvek x ys then (x : a)
                                  else a
                         ) [] xs

doplneni :: Eq a => Hypergraf a -> Hypergraf a
doplneni (Hypergraf xs ys) = Hypergraf xs (ys ++ hranyPridat)
        where
             vsechnyMozneHrany = kombinace 2 xs
             hranyUvnitr = foldr (\hhrana ak -> ak ++ kombinace 2 hhrana) [] ys
             hranyPridat = difference vsechnyMozneHrany hranyUvnitr