data Graf a = G [a] [(a,a)]

--http://forum.matfyz.info/viewtopic.php?f=169&t=11357&p=41017&hilit=troj#p41017

troj :: Eq a => Graf a -> Int
troj (G vertices edges) = foldr (\b a -> if b then a+1 else a) 0 [x == i && z == j && y == u | (x,y) <- edges, (z,u) <- edges, (i,j) <- edges]