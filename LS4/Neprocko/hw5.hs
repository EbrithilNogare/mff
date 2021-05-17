import qualified Data.Map as Map
import Data.Maybe (fromMaybe, isNothing)


data Trie k v = Trie (Map.Map k (Trie k v)) (Maybe v) deriving (Eq, Read, Show)

empty :: Ord k => Trie k v
empty = Trie Map.empty Nothing

singleton :: Ord k => [k] -> v -> Trie k v
singleton k v = insert k v empty

insertWith :: Ord k => (v -> v -> v) -> [k] -> v -> Trie k v -> Trie k v
insertWith f []     v (Trie nodes Nothing) = Trie nodes (Just v)
insertWith f []     v (Trie nodes (Just val)) = Trie nodes (Just (f v val))
insertWith f (x:xs) v (Trie nodes val) = Trie (Map.alter (Just . insertWith f xs v . fromMaybe empty) x nodes) val

insert :: Ord k => [k] -> v -> Trie k v -> Trie k v
insert []     v (Trie nodes val) = Trie nodes (Just v)
insert (x:xs) v (Trie nodes val) = Trie (Map.alter (Just . insert xs v . fromMaybe empty) x nodes) val

find :: Ord k => [k] -> Trie k v -> Maybe v
find []     (Trie _ v)     = v
find (x:xs) (Trie nodes v) = fromMaybe Nothing (find xs <$> Map.lookup x nodes)

member :: Ord k => [k] -> Trie k v -> Bool
member k (Trie nodes v) = case find k (Trie nodes v) of
	Nothing -> False
	Just _  -> True

fromList :: Ord k => [([k], v)] -> Trie k v
fromList [] = empty
fromList as = fromList' as empty
	where
		fromList' []          trie = trie
		fromList' ((k, v):xs) trie = fromList' xs $ insert k v trie

main = do
	print "Tests:"
	print $ (empty:: Trie Char Int) == fromList []
	print $ singleton "a" "b" == fromList [("a", "b")]
	print $ insertWith (++) "a" "x" empty == fromList [("a","x")]
	print $ insertWith (++) "a" "x" (fromList [("a","y")]) == fromList [("a","xy")]
	print $ insert "a" "x" (fromList [("a","y")]) == fromList [("a","x")]
	print $ find "a" (empty:: Trie Char Int) == Nothing
	print $ find "a" (fromList [("a","x")]) == Just "x"
	print $ member "a" empty == False
	print $ member "a" (fromList [("ab","x")]) == False
	print $ member "he" (singleton "hello" 3) == False
	print $ find "hello" (singleton "he" 3) == Nothing