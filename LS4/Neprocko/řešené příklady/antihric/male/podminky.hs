--http://forum.matfyz.info/viewtopic.php?f=169&t=11466&p=41155&hilit=podm%C3%ADnky#p41155

podminky :: Eq a => [(a -> Bool)] -> [a] -> [[a]]
podminky [] _ = []
podminky (p : ps) xs = ys : podminky ps zbytek
                    where 
                        ys = foldr (\x a -> if p x then (x : a) else a) [] xs
                        zbytek = foldr (\x a -> if not $ elem x ys then (x : a) else a) [] xs

