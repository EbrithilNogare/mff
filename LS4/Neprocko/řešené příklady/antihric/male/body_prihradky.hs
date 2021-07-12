--http://forum.matfyz.info/viewtopic.php?f=169&t=10104

manhattanDistance :: (Num a, Ord a) => [a] -> [a] -> a
manhattanDistance x y = sum $ zipWith (\a b -> abs (a-b)) x y

zarad :: (Num a, Ord a) => ([a], [a]) -> [[a]] -> ( [[a]], [[a]] )
zarad (x,y) [] = ([x], [y]) 
zarad (x,y) (z : zs) | dx <= dy     = (z:xs, ys)
                     | otherwise    = (xs, z:ys) 
            where
                dx = manhattanDistance z x
                dy = manhattanDistance z y
                (xs, ys) = zarad (x,y) zs