-- https://bigonotetaking.wordpress.com/2015/11/06/a-trie-in-haskell/
-- http://mchaver.com/posts/2018-12-27-tries-in-haskell.html
-- https://blog.jle.im/entry/tries-with-recursion-schemes.html

import qualified Data.Map as Map
import Data.Maybe (fromMaybe, isNothing)


data Trie k v = Trie (Map.Map k (Trie k v)) (Maybe v) deriving (Eq, Read, Show, Ord)


empty :: Trie k v
empty = Trie Map.empty Nothing


singleton :: Ord k => [k] -> v -> Trie k v
singleton k v = insert k v empty


insertWith :: Ord k => (v -> v -> v) -> [k] -> v -> Trie k v -> Trie k v
insertWith f []     v (Trie nodes val) = Trie nodes (Just v)
insertWith f (x:xs) v (Trie nodes val) = Trie (Map.alter (Just . insertWith f xs v . fromMaybe empty) x nodes) val


insert :: Ord k => [k] -> v -> Trie k v -> Trie k v
insert []     v (Trie nodes val) = Trie nodes (Just v)
insert (x:xs) v (Trie nodes val) = Trie (Map.alter (Just . insert xs v . fromMaybe empty) x nodes) val


find :: (Ord k) => [k] -> Trie k v -> Maybe v
find []     (Trie _ v)     = v
find (x:xs) (Trie nodes v) = fromMaybe v (find xs <$> Map.lookup x nodes)


member :: Ord k => [k] -> Trie k v -> Bool
member []     (Trie _ _)     = True
member (x:xs) (Trie nodes _) = fromMaybe False (member xs <$> Map.lookup x nodes)


fromList :: Ord k => [([k], v)] -> Trie k v
fromList [] = empty
fromList as = fromList' as empty
  where
    fromList' []          trie = trie
    fromList' ((k, v):xs) trie = fromList' xs $ insert k v trie


delete :: (Ord k) => [k] -> Trie k v -> Trie k v
delete = undefined

-- 'delete ks t' smaže klíč 'ks' (a odpovídající hodnotu) z trie 't', pokud
-- klíč 'ks' není v trii obsažený, 'delete' vrátí původní trii.
--
-- > delete "a" (fromList [("b","y")]) == fromList [("b","y")]
-- > delete "a" (fromList [("a","x")]) == fromList []


main = do
  print "Tests:"
  --print $ empty == fromList []
  print $ singleton "a" "b" == fromList [("a", "b")]
  print $ insertWith (++) "a" "x" empty == fromList [("a","x")]
  print $ insertWith (++) "a" "x" (fromList [("a","y")]) == fromList [("a","xy")]
  print $ insert "a" "x" (fromList [("a","y")]) == fromList [("a","x")]
  --print $ find "a" empty == Nothing
  print $ find "a" (fromList [("a","x")]) == Just "x"
  print $ member "a" empty                  == False
  print $ member "a" (fromList [("a","x")]) == True
