module Powerset where

--mnozina vsech mnozin

powerset :: [a] -> [[a]]
powerset [] = [[]]
powerset (x : xs) = ps ++ map (x:) ps
        where ps = powerset xs
