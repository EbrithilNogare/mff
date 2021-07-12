import Functional

--testovaci vstupy
--hr2sous [ (1, 'a','b'), (2,'b','a'), (3,'b','c'), (4,'c','d') ]

--sous2hr [('a',[(1,'b')]),('b',[(2,'a'),(3,'c')]),('c',[(4,'d')])]

--nekdy se to hodi, jinak to jednou bylo hricem zadane
--http://forum.matfyz.info/viewtopic.php?f=169&t=8308&p=34050&hilit=hr2sous#p34050

insertNasledovnik :: (Num b, Eq b, Eq a) => a -> a -> b -> [(a, [(b,a)])] -> [(a, [(b,a)])]
insertNasledovnik i j w xs = case idx of 
                            Just _  -> foldr (\(v,ys) a -> if v == i then (v, (w,j) : ys) : a 
                                                           else (v,ys) : a) [] xs                                                           
                            Nothing -> (i, [(w,j)]) : xs
            where idx = findIndex (\(x,fs) -> x == i) xs

hr2sous :: (Num b, Eq b, Eq a) => [(b,a,a)] -> [(a, [(b,a)])]
hr2sous = foldr (\(w,x,y) a -> insertNasledovnik x y w a) []

vyrobHrany :: (Num b, Eq b, Eq a) => (a, [(b,a)]) -> [(b,a,a)]
vyrobHrany xs = [(fst naslednik, vrchol, snd naslednik) | naslednik <- naslednici]
        where 
            vrchol = fst xs
            naslednici = snd xs

sous2hr :: (Num b, Eq b, Eq a) => [(a, [(b,a)])] -> [(b,a,a)]
sous2hr xs =  foldr (\x a -> vyrobHrany x ++ a) [] xs