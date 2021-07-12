module Functional where

--narozdil od prologoveske verze (functional.pl), zde jsou implementovany funkce, ktere v haskellu existuji
--avsak u zkousky jsou casto treba, tak aby mi netrvalo je implementovat, tak jsem si par funkci zkusil

qsort :: Ord a => [a] -> [a]
qsort [] = []
qsort (x : xs) = qsort mensi ++ [x] ++ qsort vetsi
    where 
        mensi = filter (<=x) xs
        vetsi = filter (>x) xs

qsortBy :: (a -> a -> Bool) -> [a] -> [a]
qsortBy _ [] = []
qsortBy p (x : xs) = qsortBy p mensi ++ [x] ++ qsortBy p vetsi
    where 
        mensi = filter (\y -> p y x) xs
        vetsi = filter (\y -> not (p y x)) xs

unique :: Eq a => [a] -> [a]
unique [] = []
unique (x : xs) = x : unique (filter (/=x) xs)

elemIndex :: Eq a => a -> [a] -> Maybe Int
elemIndex v xs | null indices     = Nothing
               | otherwise        = Just $ head indices
                where indices = indicesOfValue v xs 0

elemIndices :: Eq a => a -> [a] -> [Int]
elemIndices v xs = indicesOfValue v xs 0

indicesOfValue :: (Eq a) => a -> [a] -> Int -> [Int]
indicesOfValue _ [] _ = []
indicesOfValue v (x : xs) i | v == x        = i : indicesOfValue v xs (i+1)
                            | otherwise     = indicesOfValue v xs (i+1)

isDivisible :: (Integral i) => i -> i -> Bool
isDivisible m n = m `mod` n == 0

primes :: [Int]
primes = eratosthenes [2..]

eratosthenes :: [Int] -> [Int]
eratosthenes (p : xs) = p : eratosthenes [x | x <- xs, x `mod` p /= 0]

primeDecomposition :: Int -> [Int]
primeDecomposition n | length decomposed == 1   = []
                     | otherwise                = decomposed
        where 
            possibleFactors = takeWhile (\p -> p <= n) primes

            decomposition :: Int -> [Int] -> [Int]
            decomposition 1 _ = []
            decomposition n factors@(f : fs) | isDivisible n f   = f : decomposition (n `div` f) factors
                                             | otherwise         = decomposition n fs

            decomposed = decomposition n possibleFactors

nsd :: Int -> Int -> Int
nsd m 0 = m
nsd m n = nsd n (m `mod` n)

nsn :: Int -> Int -> Int
nsn m 0 = 0
nsn m n = abs(n*m) `div` (nsd m n)

findIndex :: (a -> Bool) -> [a] -> Maybe Int
findIndex p xs | null indices   = Nothing
               | otherwise      = Just $ head indices
            where indices = allSatisfying p xs 0

findIndices :: (a -> Bool) -> [a] -> [Int]
findIndices p xs = allSatisfying p xs 0

allSatisfying :: (a -> Bool) -> [a] -> Int -> [Int] 
allSatisfying _ [] _ = []
allSatisfying p (x : xs) i | p x        = i : allSatisfying p xs (i+1)
                           | otherwise  = allSatisfying p xs (i+1)

putAt :: Int -> a -> [a] -> [a]
putAt _ _ [] = []
putAt 0 y (x : xs) = y : xs
putAt i y (x : xs) = x : putAt (i-1) y xs

insertAt :: Int -> a -> [a] -> [a]
insertAt 0 y xs = y : xs
insertAt i y (x : xs) = x : insertAt (i-1) y xs

deleteAt :: Int -> [a] -> [a]
deleteAt 0 (x : xs) = xs
deleteAt i (x : xs) = x : deleteAt (i-1) xs

deleteFirst :: Eq a => a -> [a] -> [a]
deleteFirst y xs = case idx of
                        Nothing -> xs
                        Just i  -> deleteAt i xs
                where
                    idx = elemIndex y xs

deleteAll :: Eq a => a -> [a] -> [a]
deleteAll x = filter(/=x)

swap :: Int -> [a] -> Int -> [a] -> ([a],[a])
swap i xs j ys = (newXs, newYs)
                where 
                    x = xs !! i
                    y = ys !! j
                    newYs = putAt j x ys 
                    newXs = putAt i y xs

replaceFirst :: Eq a => a -> [a] -> a -> [a]
replaceFirst _ [] _ = []
replaceFirst p (x : xs) q | p /= x      = x : replaceFirst p xs q
                          | otherwise   = q : xs

replaceAll :: Eq a => a -> [a] -> a -> [a]
replaceAll _ [] _ = []
replaceAll p (x : xs) q | p /= x       = x : replaceAll p xs q
                        | otherwise    = q : replaceAll p xs q

                        
--jde to udelat tisickrat lepe pomoci takeWhile a drop
group :: Eq a => [a] -> [(a, Int)]
group [] = []
group (x : xs) = occurence 1 x xs 
                where
                    occurence :: Eq a => Int -> a -> [a] -> [(a, Int)]
                    occurence n y [] = [(y,n)]
                    occurence n y (x : xs) | y == x     = occurence (n+1) y xs
                                           | otherwise  = (y, n) : occurence 1 x xs


groupBy :: (a -> a -> Bool) -> [a] -> [[a]]
groupBy f xs = reverse $ groupBy' f xs []
                where
                    groupBy' :: (a -> a -> Bool) -> [a] -> [[a]] -> [[a]]
                    groupBy' _ [] a = a
                    groupBy' f (x : xs) a = groupBy' f rest ((x : toBeGrouped) : a)
                            where
                                toBeGrouped = takeWhile (\y -> f x y) xs
                                rest = drop (length toBeGrouped) xs


splitOn :: Char -> String -> [String]
splitOn _ [] = []
splitOn c xs = chunk : splitOn c rest
            where
                chunk = takeWhile (\x -> x /= c) xs
                rest = drop (length chunk + 1) xs

transpose :: [[a]] -> [[a]] 
transpose ([] : _) = []
transpose [] = []
transpose xs = map head xs : transpose (map tail xs)

maximumBy :: Foldable t => (a -> a -> Bool) -> t a -> a
maximumBy f xs = foldr1 (\x a -> if f x a then x else a) xs