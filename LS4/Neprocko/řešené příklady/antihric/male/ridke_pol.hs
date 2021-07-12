import Functional

--skladani i nasobeni ridkych polynomu
--nekdy byva zadano jedno nebo druhe

--http://forum.matfyz.info/viewtopic.php?f=169&t=7877&p=33015&hilit=skl%C3%A1d%C3%A1n%C3%AD#p33015 (skladani polynomu)
--nekde na foru je i nasobeni


                --koef, exp
data RidkyP = P [(Int, Int)] deriving Show
--nulty polynom reprezentujeme jako P []

p1 = P [(3,3), (5,2), (2,1), (7,0)]
p2 = P [(7,3), (1,2), (3,1), (1,0)]

vynasobDvojci :: (Int, Int) -> (Int, Int) -> (Int, Int)
vynasobDvojci (k, e) (k', e') = (k*k', e+e')

sumaKoefProExp :: Int -> [(Int, Int)] -> Int
sumaKoefProExp exp = sum . foldr (\(x,y) a -> if y == exp then (x : a)
                                              else a) []

mult :: RidkyP -> RidkyP -> RidkyP
mult (P xs) (P ys) | null xs     = P []
                   | null ys     = P []
                   | otherwise   = P [(sumaKoefProExp exp roznasobene, exp) | exp <- exponenti]
                    where
                        roznasobene = [ vynasobDvojci c c'| c <- xs, c' <- ys]
                        exponenti = unique $ map snd roznasobene

koefMult :: Int -> RidkyP -> RidkyP
koefMult k (P xs) = P (map (\(l,e) -> (l*k,e)) xs)

power :: RidkyP -> Int -> RidkyP
power (P []) 0 = error "Nulty polynom na nultou."
power _ 0 = P [(1,0)]
power p 1 = p
power p r = power (mult p p) (r-1)

secti :: RidkyP -> RidkyP -> RidkyP
secti (P xs) (P ys) = P (map (\xs -> (sum $ map fst xs, snd $ head xs)) grouped)
    where
        sorted = qsortBy (\(x,y) (x',y') -> y > y') (xs ++ ys)
        grouped = groupBy (\(x,y) (x',y') -> y==y') sorted


comp :: RidkyP -> RidkyP -> RidkyP
comp (P xs) q = foldl1 (\a p -> secti a p) (map (\(k,e) -> koefMult k (power q e)) xs)