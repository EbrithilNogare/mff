--http://forum.matfyz.info/viewtopic.php?f=169&t=10479

rle :: Eq a => [a] -> [Either a (a,Int)]
rle [] = []
rle (x : xs) | len == 1     = Left x : rle zs
             | otherwise    = Right (x, len) : rle zs
        where
            ys = takeWhile (==x) xs
            len = 1 + length ys
            zs = drop (len - 1) xs

rleInv :: Eq a => [Either a (a,Int)] -> [a]
rleInv = concat . map genOdpPocet
            where
                genOdpPocet :: Either a (a, Int) -> [a]
                genOdpPocet (Left k) = [k]
                genOdpPocet (Right (k, i)) = replicate i k