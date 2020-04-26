-- https://bigonotetaking.wordpress.com/2015/11/06/a-trie-in-haskell/
-- http://mchaver.com/posts/2018-12-27-tries-in-haskell.html
-- https://blog.jle.im/entry/tries-with-recursion-schemes.html

import qualified Data.Map as M

-- 5. úloha
--
-- 1) Definujte datový typ 'Trie k v' reprezentující trii, kde klíče (řetězce)
-- jsou typu '[k]' a hodnoty typu 'v'.

data Trie k v = Trie { key :: [(k, Trie k v)], value :: Maybe v } deriving (Show)

empty :: Trie k v
empty = Trie { key = [], value = Nothing }

--singleton :: [k] -> v -> Trie k v
--singleton k v = Trie { key = [(k, child)], value = Nothing }
--  where
--    child = Trie { key = [], value = (Just v) }


search :: (Eq a) => a -> [(a,b)] -> Maybe b
search _ [] = Nothing
search x ((a,b):xs) = if x == a then Just b else search x xs


insertWith :: (Ord k) => (v -> v -> v) -> [k] -> v -> Trie k v -> Trie k v
insertWith f []     v (Trie tk tv) = Trie { key = tk, value = tv }
insertWith f (x:[]) v (Trie tk tv) = insertWith f [] v (search x tk)

{-insertWith f (x:xs) v (Trie tk tv) = 
  if xs == [] then
  Trie { key = tk, value = (Just (f tv v)) }
  else
  insertWith f xs v (Trie tk tv)
-}

--insertWith :: (Ord k) => (v -> v -> v) -> [k] -> v -> Trie k v -> Trie k v
--insertWith f k v t
--  | t . key == k  = Trie { key = Just(k) , value = Just(v)}
--  | otherwise     = Trie { key = Just(k) , value = Just(v)}


-- 'insertWith f ks new t' vloží klíč 'ks' s hodnotou 'new' do trie 't'. Pokud
-- trie již klíč 'ks' (s hodnotou 'old') obsahuje, původní hodnota je nahrazena
-- hodnotou 'f new old'.
--
-- > insertWith (++) "a" "x" empty                  == fromList [("a","x")]
-- > insertWith (++) "a" "x" (fromList [("a","y")]) == fromList [("a","xy")]
--

--insert :: (Ord k) => [k] -> v -> Trie k v -> Trie k v
--insert [] v (Trie _ v)     = Trie Word m v []
--insert (x:xs) v (Trie k v) = Trie { nodeType = Word, key = Just(k), value = Just(v), childs = [] }


-- 'insert ks new t' vloží klíč 'ks' s hodnotou 'new' do trie 't'. Pokud trie
-- již klíč 'ks' obsahuje, původní hodnota je nahrazena hodnotou 'new'
--
-- Hint: použijte 'insertWith'
--
-- > insert "a" "x" (fromList [("a","y")]) == fromList [("a","x")]
--

find :: (Ord k) => [k] -> Trie k v -> Maybe v
find = undefined

-- 'find k t' vrátí hodnotu odpovídající klíči 'k' (jako 'Just v'), pokud
-- existuje, jinak 'Nothing'.
--
-- > find "a" empty                  == Nothing
-- > find "a" (fromList [("a","x")]) == Just "x"
--

member :: (Ord k) => [k] -> Trie k v -> Bool
member = undefined

-- 'member k t' zjistí, jestli se klíč 'k' nalézá v trii 't'.
--
-- Hint: použijte 'find'
--
--
-- Funkce 'fromList' není nutná, ale může se vám hodit pro testování.

fromList :: (Ord k) => [([k], v)] -> Trie k v
fromList = undefined

-- BONUS) Implementujte funkci

delete :: (Ord k) => [k] -> Trie k v -> Trie k v
delete = undefined

-- 'delete ks t' smaže klíč 'ks' (a odpovídající hodnotu) z trie 't', pokud
-- klíč 'ks' není v trii obsažený, 'delete' vrátí původní trii.
--
-- > delete "a" (fromList [("b","y")]) == fromList [("b","y")]
-- > delete "a" (fromList [("a","x")]) == fromList []


main = do
  print ""