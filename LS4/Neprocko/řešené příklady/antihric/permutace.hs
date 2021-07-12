module Permutace where
import Functional

--nekdy se hodi i permutace s opakovanim

permutace :: Eq a => [a] -> [[a]]
permutace [] = [[]]
permutace xs = do
                x <- xs
                let ys = deleteFirst x xs
                zs <- permutace ys
                return $ (x:zs)


permutaceOpakovani :: Int -> [a] -> [[a]]
permutaceOpakovani 0 _ = [[]]
permutaceOpakovani _ [] = []
permutaceOpakovani n xs | n > 0 = concat $ map (\(g,h) -> map (\l -> g:l) h) e
            where
                c = permutaceOpakovani (n-1) xs
                d = replicate (length xs) c
                e = zip xs d