--http://forum.matfyz.info/viewtopic.php?f=169&t=11944

prumer :: Fractional a => [a] -> a
prumer xs = sum xs / fromIntegral(length xs)

okenkovePrumery :: Fractional a => Int -> [a] -> [a]
okenkovePrumery k xs | length xs == k        = prumer okenko : []
                     | length xs > k         = prumer okenko : okenkovePrumery k zbytek
                    where 
                        okenko = take k xs
                        zbytek = drop 1 xs