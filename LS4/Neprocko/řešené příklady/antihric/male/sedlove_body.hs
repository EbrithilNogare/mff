import Functional

--http://forum.matfyz.info/viewtopic.php?f=169&t=4099&p=19692&hilit=sedlovy#p19692

testM = [
         [1,2,3],
         [4,5,6],
         [7,8,9]
        ] :: [[Int]]

jeMaxVRadku :: (Int, Int) -> [[Int]] -> Bool
jeMaxVRadku (i,j) m = ((m!!i)!!j) == maximum (m!!i)

jeMinVRadku :: (Int, Int) -> [[Int]] -> Bool
jeMinVRadku (i,j) m = ((m!!i)!!j) == minimum (m!!i)

sedloveBody :: [[Int]] -> [(Int, Int)]
sedloveBody m = [(i,j) | i <- [0..h-1], j <- [0..w-1], ((jeMaxVRadku (i,j) m && jeMinVRadku (j, i) mt) || (jeMinVRadku (i,j) m && jeMaxVRadku (j, i) mt))]
    where
        mt = transpose m
        h = length m
        w = length (head m)