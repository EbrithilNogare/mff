import Functional

data NTree a = NTree a [NTree a] deriving Show

--http://forum.matfyz.info/viewtopic.php?f=169&t=8368&p=34206&hilit=podstromy#p34206

testStrom = NTree (1, 'a') [
                            NTree (3, 'x') [
                                        NTree (5, 't') [], NTree (1, 'v') []
                                           ], 
                            NTree (0, 'z') [
                                        NTree (11, 'p') [], NTree (8, 'c') []
                                           ]
                           ]

sortNT :: Ord k => (k -> k -> Bool) -> NTree (k,h) -> NTree (k,h)
sortNT cmp (NTree v xs) = NTree v (sortPole cmp xs)

sortPole :: Ord k => (k -> k -> Bool) -> [NTree (k,h)] -> [NTree (k,h)]
sortPole _ [] = []
sortPole cmp xs = map (\y -> sortNT cmp y) ys
        where
            ys = qsortBy (\(NTree (a,_) _) (NTree (a',_) _) -> cmp a a') xs