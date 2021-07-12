import Functional

--Máte seznam s a  seznam dvojic (a,b). Máte vypsat všechny permutace původního seznamu, kde pro každou dvojici je v permutaci a před b. 
--http://forum.matfyz.info/viewtopic.php?f=169&t=4049


permutacePoradi :: [(Int, Int)] -> [Int] -> [[Int]]
permutacePoradi _ [] = [[]]
permutacePoradi poradi xs = [ (x:zs) | x <- xs, zs <- permutacePoradi poradi (deleteFirst x xs), splenoPoradi poradi (x : zs)]

splenoPoradi :: [(Int, Int)] -> [Int] -> Bool
splenoPoradi xs p = all (\(x,y) -> elemIdx x p <= elemIdx y p) xs

elemIdx :: Eq a => a -> [a] -> Int
elemIdx v xs | null indices = (-1)
             | otherwise    = head indices
    where indices = indicesOfValue v xs 0