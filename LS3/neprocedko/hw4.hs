main = do
  print (rleDecode (rleEncode "hello"))
  print (take 5 primes)
  print (sortWith (<) [10,9..1])
  print (sortWith (>) [10,9..1])
  
code :: Char -> [(Int,Char)] -> [(Int,Char)]
code c [(0,_)] = [(1,c)]
code c ((n,x):ts) 
  | c == x = (n+1,x):ts
  | otherwise = (1,c):(n,x):ts

rleEncode :: String -> [(Int, Char)]
rleEncode = foldr code [(0,' ')]

rleDecode :: [(Int, Char)] -> String
rleDecode = concatMap block
  where
    block ((num, char)) = replicate num char



primes :: [Integer]
primes = cond (2 : [3, 5..])
  where
    cond (p:xs) = p : cond [x|x <- xs, mod x p /= 0]



sortWith :: (a -> a -> Bool) -> [a] -> [a]
sortWith _ [] = []
sortWith _ [a] = [a]
sortWith f a =
  mergeWith f (sortWith f firstHalf) (sortWith f secondHalf)
    where
      firstHalf = take ((length a) `div` 2) a
      secondHalf = drop ((length a) `div` 2) a

mergeWith :: (a -> a -> Bool) -> [a] -> [a] -> [a]
mergeWith _ a [] = a
mergeWith _ [] b = b
mergeWith f (a:as) (b:bs)
  | f a b = a:(mergeWith f as (b:bs))
  | otherwise = b:(mergeWith f (a:as) bs)

