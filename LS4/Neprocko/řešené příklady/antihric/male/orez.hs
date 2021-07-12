data BVS a = Nil | Node (BVS a) a (BVS a) deriving (Show, Eq)

--http://forum.matfyz.info/viewtopic.php?f=169&t=8308

testStrom = Node (Node (Node (Nil) 10 (Nil)) 25 (Node (Node (Nil) 30 (Nil)) 35 (Node (Nil) 37 (Nil)))) 50 (Node (Nil) 75 (Node (Node (Nil) 80 (Nil)) 100 (Nil)))

orez :: Ord a => a -> a -> BVS a -> BVS a
orez _ _ Nil = Nil
orez x y (Node l v r) | x <= v && v <= y        = Node (orez x y l) v (orez x y r)
                      | v < x                   = orez x y r
                      | v > y                   = orez x y l