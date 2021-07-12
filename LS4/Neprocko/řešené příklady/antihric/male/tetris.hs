import Functional

--http://forum.matfyz.info/viewtopic.php?f=169&t=11412&p=41088&hilit=tetris#p41088

removeCols0 :: [[Int]] -> [[Int]]
removeCols0 ([] : _) = []
removeCols0 m | elem 0 sloupec       = sloupec : removeCols0 (map tail m)
              | otherwise            = removeCols0 (map tail m)
        where 
            sloupec = map head m

tetris :: [[Int]] -> [[Int]]
tetris m = transpose removedCols
        where removedCols = removeCols0 m