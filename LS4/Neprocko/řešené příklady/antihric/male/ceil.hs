--http://forum.matfyz.info/viewtopic.php?f=169&t=11747&p=41522&hilit=ceil#p41522

ceil :: (Num a, Show a)  => a -> Integer
ceil n | sign == '-'                 = read integerPartStr :: Integer
       | integerPartStr == nStr      = read integerPartStr :: Integer
       | otherwise                   = (read integerPartStr :: Integer) + 1
    where
        nStr = show n
        sign = nStr !! 0
        integerPartStr = takeWhile(\x -> x /= '.') nStr

