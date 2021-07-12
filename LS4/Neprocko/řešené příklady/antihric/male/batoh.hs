import Kombinace
import Functional

--klasika - problem batohu

batoh :: Int -> [(Int, Int)] -> [(Int, Int)]
batoh 0 _ = []
batoh w [] | w >= 0 = []
batoh w ((v,c) : ps) | c > 0 && vb + v <= w     = (v, c) : b
                     | c > 0 && vb + v > w      = maximumBy (\b b' -> cenaBatohu b > cenaBatohu b') noveBatohy
                     | c < 0                    = b
    where
        b = batoh w ps
        vb = vahaBatohu b
        nejsou = deleteFirstOccurence ((v,c):ps) b --nejsou v batohu
        noveBatohy = b : [nb | h <- [1..length b], k <- kombinace h b, j <- [1..length nejsou], k2 <- kombinace j nejsou, let nb = nejsou ++ (deleteFirstOccurence b k), vahaBatohu nb <= w]

vahaBatohu :: [(Int, Int)] -> Int
vahaBatohu = sum . map fst 

cenaBatohu :: [(Int, Int)] -> Int
cenaBatohu = sum . map snd

deleteFirstOccurence :: Eq a => [a] -> [a] -> [a]
deleteFirstOccurence xs [] = xs
deleteFirstOccurence xs (y : ys) = case i of 
                                        Just idx -> deleteFirstOccurence (deleteAt idx xs) ys
                                        Nothing  -> deleteFirstOccurence xs ys
            where
                i = elemIndex y xs