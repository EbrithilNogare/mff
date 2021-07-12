data Tree a = Leaf a | Node (Tree a) a (Tree a) deriving Show

--http://forum.matfyz.info/viewtopic.php?f=169&t=10961&p=40530&hilit=fold#p40530


testStrom = Node (Node (Leaf 10) 30 (Leaf 20)) 50 (Node (Leaf 30) 40 (Leaf 60))

         --akumulator nalevo
fold :: (b -> a -> b) -> b -> Tree a -> b
fold f a (Leaf v) = f a v
fold f a (Node l v r) = f (fold f (fold f a l) r) v
                        --slozim nejdrive levou, pak pravou vetev a nakonec jeste hodnotu


fold2 :: (a -> b) -> (b -> b -> b) -> Tree a -> b
fold2 f g (Leaf v) = f v
fold2 f g (Node l v r) = g (f v) (g (fold2 f g l) (fold2 f g r))
