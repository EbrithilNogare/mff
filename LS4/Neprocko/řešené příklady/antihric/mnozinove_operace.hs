module MnozinoveOp where
import Functional

--mnozinoviny

intersection :: Eq a => [a] -> [a] -> [a]
intersection xs = foldr (\y a -> if elem y xs && (not $ elem y a) then y:a else a) []

union :: Eq a => [a] -> [a] -> [a]
union xs ys = unique (xs ++ ys)

difference :: Eq a => [a] -> [a] -> [a]
difference xs ys = foldr (\x a -> if (not $ elem x ys) && (not $ elem x a) then x:a else a) [] xs

setPushFront :: Eq a => a -> [a] -> [a]
setPushFront x xs | elem x xs   = xs
                  | otherwise   = x:xs

setPushBack :: Eq a => a -> [a] -> [a]
setPushBack x xs | elem x xs   = xs
                 | otherwise   = xs++[x]

setAppend :: Eq a => [a] -> [a] -> [a]
setAppend xs ys = unique (xs ++ ys)

symmetricDifference :: Eq a => [a] -> [a] -> [a]
symmetricDifference xs ys = (difference xs ys) `union` (difference ys xs)

cartesianProduct :: [a] -> [b] -> [(a,b)]
cartesianProduct xs ys = [(x,y) | x <- xs, y <- ys]
