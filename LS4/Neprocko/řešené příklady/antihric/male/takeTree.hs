data NT a = N a [NT a] deriving Show

--http://forum.matfyz.info/viewtopic.php?f=169&t=11466&p=41155&hilit=takeTree#p41155

testStrom = N 10 [N 5 [N 3 [], N 8 []], N 12 [], N 40 [N 35 [], N 37 [], N 42 [], N 43 []]]

takeTree :: Int -> Int -> NT a -> NT a
takeTree _ 0 (N v xs) = N v []
takeTree n m (N v xs) = N v ys
        where
            arita = min (length xs) n
            children = take arita xs
            ys = map (\t -> takeTree n (m-1) t) children