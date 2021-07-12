import Functional
import Kombinace

--http://forum.matfyz.info/viewtopic.php?f=169&t=11756

moznaNahrada :: Int -> [Char] -> String-> [Char]
moznaNahrada i abc text = filter (/=(text !! i)) abc

nahrad :: (Int, Char) -> (Int, Char) -> (Int, Char) -> String -> String
nahrad (i1, n1) (i2, n2) (i3, n3) text = putAt i3 n3 $ putAt i2 n2 $ putAt i1 n1 text

nahrada :: [Char] -> String -> [[Int]] -> [String]
nahrada _ _ [] = []
nahrada abc text (i : indexy) = [ nahrad (i1,n1) (i2, n2) (i3, n3) text | 
                                n1 <- moznaNahrada i1 abc text, n2 <- moznaNahrada i2 abc text, n3 <- moznaNahrada i3 abc text] ++ nahrada abc text indexy
                        where
                            i1 = i !! 0
                            i2 = i !! 1
                            i3 = i !! 2

change3 :: [Char] -> String -> [String]
change3 abc text = nahrada abc text indexy
            where 
                indexy = kombinace 3 [0..length text - 1]
