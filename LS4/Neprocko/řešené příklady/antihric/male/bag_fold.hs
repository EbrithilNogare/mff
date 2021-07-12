data Bag a = Item a | Items [Bag a] deriving Show

t = (Items [Item 1,Items [Item 2, Item 3], Items [Items [Item 4]]]) 

--http://forum.matfyz.info/viewtopic.php?f=169&t=11357&p=41017&hilit=bag#p41017

fold :: (a -> b) -> ([b] -> b) -> Bag a -> b
fold f g (Item x) = f x
fold f g (Items xs) = g prvky
        where prvky = map (\x -> fold f g x) xs

listy :: Bag a -> [a]
listy = fold (\x -> [x]) (foldr (++) [])