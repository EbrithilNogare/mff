import Functional

--tohle je krylovina
--spiralovity vypis matice po obvodu

middle :: [a] -> [a]
middle xs | length xs >= 2      = init $ tail xs
          | otherwise           = []


oloupani :: Num a => [[a]] -> [a]
oloupani [x] = x
oloupani [] = []
oloupani m = init p ++ init q ++ init r ++ init s ++ oloupani b
        where
            mt = transpose m
            p = head m
            q = last mt
            r = reverse $ last m
            s = reverse $ head mt

            a = middle m
            b = transpose $ middle $ transpose a