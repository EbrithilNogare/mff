import Permutace
import Functional

--http://forum.matfyz.info/viewtopic.php?f=169&t=10934

cmp :: (Ord a, Num a) => [a] -> [a] -> Bool
cmp x y | maximum x < maximum y       = True
        | maximum x > maximum y       = False
        | otherwise                   = x < y

kticeN :: Int -> Int -> [[Int]]
kticeN k n = qsortBy cmp (permutaceOpakovani k [0..n])

kticeM :: Int -> Int -> [[Int]]
kticeM k m = qsortBy cmp $ filter(\x -> maximum x == m) (permutaceOpakovani k [0..m])

ktice :: Int -> [[Int]]
ktice k = [jedna | m <- [0..], jedna <- qsortBy cmp (kticeM k m)]