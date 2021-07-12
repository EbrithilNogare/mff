import Functional

--http://forum.matfyz.info/viewtopic.php?f=169&t=11043&p=40642&hilit=grupa#p40642 (tady to je v prologu, ale bylo to min 1x uz zadano i na haskell)

testGrupa1 = [
    ['e', 'a', 'b'], --1
    ['a', 'b', 'e'], --3
    ['b', 'e', 'a']  --3 
    ]

testGrupa2 = [
    ['e', 'a', 'b', 'c'], --1
    ['a', 'b', 'e', 'c'], --4
    ['b', 'c', 'a', 'e'], --3
    ['c', 'e', 'c', 'b']  --3
    ]

testGrupa3 = [
    ['e', 'a', 'b', 'c'], --1
    ['a', 'b', 'e', 'c'], --4
    ['b', 'c', 'a', 'b'], --3
    ['c', 'e', 'c', 'e']  --2 
    ]

transform :: [[Char]] -> ([Char], [[Char]])
transform vstup = (h,transpose c) 
    where
        h = head vstup
        a = tail vstup
        b = transpose a
        c = tail b

op :: ([Char],[[Char]]) -> Char -> Char -> Char
op (h,v) a b = case i of
                Just ii ->  case j of
                            Just jj -> (v !! (ii-1)) !! (jj-1)
                            Nothing -> error "Neplatne pismenko."

                Nothing -> error "Neplatne pismenko."
        where
            i = findIndex (\x -> x==a) h
            j = findIndex (\x -> x==b) h

rad :: ([Char],[[Char]]) -> Char -> Int
rad _ 'e' = 1
rad v a = rad' v 1 a a
            where
                rad' :: ([Char],[[Char]]) -> Int -> Char -> Char -> Int
                rad' _ n 'e' _ = n
                rad' v n a b = rad' v (n+1) c b
                    where
                        c = op v a b