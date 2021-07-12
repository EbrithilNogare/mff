--http://forum.matfyz.info/viewtopic.php?f=169&t=11969

--efektivni implementace neopakujici vypocty

safehead :: [[a]] -> [a]
safehead xs | null xs   = []
            | otherwise = head xs

kumulace :: Num a => [[a]] -> [[a]]
kumulace m = reverse $ foldl (\km r -> kumulaceRadku (safehead km) r : km) [] m

kumulaceRadku :: Num a => [a] -> [a] -> [a]
kumulaceRadku [] r = scanl1 (+) r
kumulaceRadku kr r = foldr (\(a,b,c,d) acc -> a+b+c-d : acc) [] xs
    where xs = zip4 kr r

zip4 :: Num a => [a] -> [a] -> [(a,a,a,a)]
zip4 kr r = zip4' kr r (0:kr) 0
    where
        zip4' :: Num a => [a] -> [a] -> [a] -> a -> [(a,a,a,a)]
        zip4' _ [] _ _ = []
        zip4' (a : kr) (b : r) (c : kr1) d = (b,d,a,c) : zip4' kr r kr1 (b+d+a-c)
