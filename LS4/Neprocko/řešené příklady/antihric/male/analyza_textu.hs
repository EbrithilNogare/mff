import Functional

--http://forum.matfyz.info/viewtopic.php?f=169&t=11954&sid=b70d17efce35710d058f2d076ebb18eb

type Slovo = (String, Int)
type Radka = (String, Int)

extractSlova :: Int -> Radka -> [Slovo]
extractSlova _ ([], _) = []
extractSlova n (text, cislo) | length slovo >= n = (slovo, cislo) : extractSlova n (zbytek, cislo)
                             | otherwise         = extractSlova n (zbytek, cislo)
            where
                slovo = takeWhile (\x -> x /= '\t' && x /= ' ') text
                zbytek = drop (length slovo + 1) text


stat :: String -> Int -> [Slovo]
stat text n = slova
    where
        radky = splitOn '\n' text
        ocislovaneRadky = zip radky [1..length radky]
        slova = qsort $ concat $ map (\r -> extractSlova n r) ocislovaneRadky

