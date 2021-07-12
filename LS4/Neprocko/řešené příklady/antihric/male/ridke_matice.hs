import Functional
            --(height, width, [(i, j, value)])
data RidkaM = M (Int, Int, [(Int, Int, Int)]) deriving Show

m1 = M (3,4, [(1,2,1), (2,1,3), (2,3,5), (3,1,9), (3,2,8), (3,3,7)])

--http://forum.matfyz.info/viewtopic.php?f=169&t=9645&p=38384&hilit=matice#p38384

transposeR :: RidkaM -> RidkaM
transposeR (M (h, w, as)) = M (w, h, asT)
        where asT = map (\(y,x,a) -> (x,y,a)) as

mult :: RidkaM -> RidkaM -> RidkaM
mult m1 m2 = normalni2ridka $ matrixMult (ridka2normalni m1) (ridka2normalni m2)

normalni2ridka :: [[Int]] -> RidkaM
normalni2ridka m = M(h,w,as)
        where
            w = length $ head m
            h = length m 
            as = radky2ridke m

radky2ridke :: [[Int]] -> [(Int, Int, Int)]
radky2ridke m = radky2ridke' 0 m
        where
            radky2ridke' :: Int -> [[Int]] -> [(Int, Int, Int)]
            radky2ridke' _ [] = []
            radky2ridke' i (x : xs) = radek2ridky i x ++ radky2ridke' (i+1) xs

radek2ridky :: Int -> [Int] -> [(Int, Int, Int)]
radek2ridky i x = foldr (\(j, v) a -> if v /= 0 then (i+1, j+1, v) : a else a) [] xz
        where xz = zip [0..length x - 1] x


ridka2normalni :: RidkaM -> [[Int]]
ridka2normalni (M (h,w,as)) = foldr (\(i,j,v) m -> putAt (i-1) ( (putAt (j-1) v (m!!(i-1)) ) ) m ) nulova as
        where nulova = replicate h $ replicate w 0

matrixMult :: [[Int]] -> [[Int]] -> [[Int]]
matrixMult m1 m2 | w1 == h2     = matrixMult' m1 mt
                 | otherwise    = error "Matice nemaji kompatibilni rozmery na nasobeni."
        where
            w1 = length $ head m1
            h2 = length m2
            mt = transpose m2

matrixMult' :: [[Int]] -> [[Int]] -> [[Int]]
matrixMult' [] _ = []
matrixMult' (m : ms) m2 = vecMatrixMult m m2 : matrixMult' ms m2

vecMatrixMult :: [Int] -> [[Int]] -> [Int]
vecMatrixMult _ [] = []
vecMatrixMult x (y : mt) = (vecMult x y) : vecMatrixMult x mt

vecMult :: [Int] -> [Int] -> Int
vecMult xs ys = sum $ zipWith (\x y -> x * y) xs ys