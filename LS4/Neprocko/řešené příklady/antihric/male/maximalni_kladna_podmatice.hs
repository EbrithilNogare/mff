--http://forum.matfyz.info/viewtopic.php?f=169&t=10536

jeMaximalni :: (Num a, Ord a) => [[a]] -> (Int, Int) -> Bool
jeMaximalni m (i,j) | i + 1 == rows && j + 1 == cols    = True
                    | i + 1 == rows                     = not $ jeKladna m (i, j+1)
                    | j + 1 == cols                     = not $ jeKladna m (i+1, j)
                    | otherwise                         = (not $ jeKladna m (i+1, j)) && (not $ jeKladna m (i, j+1))
                where
                    rows = length m
                    cols = length (m!!0)

jeHledana :: (Num a, Ord a) => [[a]] -> (Int, Int) -> Bool
jeHledana m (i,j) = jeMaximalni m (i,j) && jeKladna m (i,j)

jeKladna :: (Num a, Ord a) => [[a]] -> (Int, Int) -> Bool
jeKladna m (i,j) = and [ ((m!!i')!!j') > 0 | i' <- [0..i], j' <- [0..j]]

indexyPodmatic :: (Num a, Ord a) => [[a]] -> [(Int,Int)]
indexyPodmatic m = [(i,j) | i <- [0..(length m - 1)], j <- [0..(length(m !! 0)-1)] ]

maximalniKladnaPodmatice :: (Num a, Ord a) => [[a]] -> [(Int, Int)]
maximalniKladnaPodmatice m = [(i+1,j+1) | (i,j) <- indexyPodmatic m, jeHledana m (i,j)]