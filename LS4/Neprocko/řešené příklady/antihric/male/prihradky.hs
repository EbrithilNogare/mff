--http://forum.matfyz.info/viewtopic.php?f=169&t=8851

--prihradky [2, 5, 11, 25] [10, 2, 1, 0, 3, 6, 30] 
--[(0, [1, 0], (2, [2, 3]), (5, [10, 6]), (11, []), (25, [30])] 

prihradky :: (Num a, Ord a, Eq a) => [a] -> [a] -> [(a,[a])]
prihradky ps@(p : _) xs = (minimum mensi, mensi) : prihradky' ps xs 
            where mensi = filter (<p) xs

prihradky' :: (Num a, Ord a, Eq a) => [a] -> [a] -> [(a,[a])]
prihradky' [p] xs = [(p, ys)]
            where ys = filter(>=p) xs

prihradky' (p1 : p2 : ps) xs = (p1, ys) : prihradky' (p2 : ps) xs
            where ys = filter(\x -> x >= p1 && x <= p2) xs