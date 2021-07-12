import Functional

--jen tak generator slov

        --delka  abeceda    slova
generuj :: Int -> [Char] -> [String]
generuj 0 abc = []
generuj 1 abc = foldr (\c a -> [c] : a) [] abc
generuj delka abc = [insertAt i znak slovo | slovo <- slova, i <- [0..length slovo - 1], znak <- abc]
            where slova = generuj (delka - 1) abc

              --abeceda    slova
vsechnaSlova :: [Char] -> [String]
vsechnaSlova abc = [slovo | delka <- [0..], slovo <- generuj delka abc]