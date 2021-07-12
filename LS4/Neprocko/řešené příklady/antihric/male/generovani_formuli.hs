data Formula = Const Bool | Not Formula | And Formula Formula | Or Formula Formula deriving Show

--http://forum.matfyz.info/viewtopic.php?f=169&t=11457&p=41144&hilit=Konst#p41144

generateFormulas :: Int -> [Formula]
generateFormulas 0 = [Const False, Const True]
generateFormulas hladina = unarni ++ binarni
                    where
                        fs = generateFormulas (hladina - 1)
                        dvojceFormuli = [(f1, f2) | f1 <- fs, f2 <- fs]
                        unarni = foldr (\f l -> (Not f) : l) [] fs
                        binarni = foldr (\(f1, f2) l -> (And f1 f2) : (Or f1 f2) : l) [] dvojceFormuli

gen :: [Formula]
gen = [f | hladina <- [0..], f <- generateFormulas hladina]