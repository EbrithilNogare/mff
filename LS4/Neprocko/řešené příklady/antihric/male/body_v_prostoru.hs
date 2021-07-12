--http://forum.matfyz.info/viewtopic.php?f=169&t=11052&p=40651&hilit=v+prostoru#p40651

body :: (Num a, Ord a) => [(a,a,a)] -> [(a,a,a)]
body xs = bodyImpl 0 xs

deleteAt :: Int -> [a] -> [a]
deleteAt _ [] = []
deleteAt idx (x : xs) | idx > 0     = x : deleteAt (idx - 1) xs
                      | idx == 0    = deleteAt (idx - 1) xs
                      | otherwise   = x : deleteAt (idx - 1) xs

majiVetsiSouradnici :: (Num a, Ord a) => (a,a,a) -> [(a,a,a)] -> Bool
majiVetsiSouradnici (p,q,r) = and . map (\(x,y,z) -> x > p || y > q || z > r)

bodyImpl :: (Num a, Ord a) => Int -> [(a,a,a)] -> [(a,a,a)]
bodyImpl idx xs | idx >= length xs = []
                | majiVetsiSouradnici p ostatni = p : bodyImpl (idx+1) xs
                | otherwise                     = bodyImpl (idx + 1) xs
            where
                p = xs !! idx
                ostatni = deleteAt idx xs