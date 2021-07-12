import Functional

--http://forum.matfyz.info/viewtopic.php?f=169&t=8308&p=34050&hilit=paretoMax#p34050

--u je dominovan v  -> True
--otherwise         -> False
jeDominovan :: Ord a => [a] -> [a] -> Bool
jeDominovan u v = and $ zipWith (\su sv -> sv >= su) u v

paretoMax :: (Ord a, Eq a) => [[a]] -> [[a]]
paretoMax xs = [x | x <- xs, and $ map(\y -> not $ jeDominovan x y) (deleteFirst x xs) ]