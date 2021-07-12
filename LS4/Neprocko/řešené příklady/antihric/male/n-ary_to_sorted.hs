--http://forum.matfyz.info/viewtopic.php?f=169&t=11380&p=41048&hilit=strom+na#p41048

data NTree a = N2 a [NTree a]
data UspTree a = UT [UspTree a] a [UspTree a]

preved :: NTree (a, Int) -> UspTree a
preved (N2 (h, l) xs) = UT ls h ps
            where
                leveStromy = take l xs
                praveStromy = drop l xs
                ls = map preved leveStromy
                ps = map preved praveStromy
                