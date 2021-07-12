data BVS a = Nil | Node (BVS a) a (BVS a) deriving Show

--http://forum.matfyz.info/viewtopic.php?f=169&t=11043 (varianta preorder)

preorderToBVS :: Ord a => [a] -> BVS a
preorderToBVS [] = Nil
preorderToBVS (koren : xs) = Node (preorderToBVS vlevo) koren (preorderToBVS vpravo) 
                where
                    vlevo = takeWhile (\x -> x < koren) xs
                    vpravo = drop (length vlevo) xs

postorderToBVS :: Ord a => [a] -> BVS a
postorderToBVS xs = postorderToBVS' (reverse xs)
                    where 
                        postorderToBVS' :: Ord a => [a] -> BVS a
                        postorderToBVS' [] = Nil
                        postorderToBVS' (koren : xs) = Node (postorderToBVS' vlevo) koren (postorderToBVS' vpravo)
                            where
                                vpravo = takeWhile (\x -> x > koren) xs
                                vlevo = drop (length vpravo) xs

inorderToBVS :: Ord a => [a] -> BVS a
inorderToBVS [] = Nil
inorderToBVS xs = Node (inorderToBVS vlevo) (xs !! korenIdx) (inorderToBVS vpravo)
                where 
                    korenIdx = (length xs - 1) `div` 2
                    vlevo = take korenIdx xs
                    vpravo = drop (korenIdx + 1) xs
