module Kombinace where

--kombinace se mi hodili furt

kombinace :: (Eq a, Integral b) => b -> [a] -> [[a]]
kombinace 0 _ = [[]]
kombinace _ [] = []
kombinace n (x : xs) | n > 0 = kombinace n xs ++ map (x:) (kombinace (n-1) xs)