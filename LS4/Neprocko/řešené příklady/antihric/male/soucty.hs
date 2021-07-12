--http://forum.matfyz.info/viewtopic.php?f=169&t=11954

safehead :: Num a => [a] -> a
safehead (x : xs) = x
safehead [] = 0

soucty :: (Eq p, Ord p, Num p) => [p] -> (Int, Int, p)
soucty (x : xs) = souctyImpl xs x 0 0 x 0 0 1

souctyImpl :: (Eq p, Ord p, Num p) => [p] -> p -> Int -> Int -> p -> Int -> Int -> Int -> (Int, Int, p)
souctyImpl [] gm gmSIdx gmEIdx lm lmSIdx lmEIdx currIdx = (gmSIdx, gmEIdx, gm)
souctyImpl (x : xs) gm gmSIdx gmEIdx lm lmSIdx lmEIdx currIdx 
        | x >= 0 && x < (lm + x)   = souctyImpl xs gm gmSIdx gmEIdx (lm + x) lmSIdx (lmEIdx + 1) (currIdx + 1)
        | x > (lm + x)             = souctyImpl xs gm gmSIdx gmEIdx x currIdx currIdx (currIdx + 1)
        | otherwise                = if lm > gm then souctyImpl xs lm lmSIdx lmEIdx (safehead xs) (currIdx + 1) (currIdx + 1) (currIdx + 1)
                                     else            souctyImpl xs gm gmSIdx gmEIdx (safehead xs) (currIdx + 1) (currIdx + 1) (currIdx + 1)