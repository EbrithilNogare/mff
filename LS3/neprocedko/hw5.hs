-- https://bigonotetaking.wordpress.com/2015/11/06/a-trie-in-haskell/
-- http://mchaver.com/posts/2018-12-27-tries-in-haskell.html
-- https://blog.jle.im/entry/tries-with-recursion-schemes.html

import qualified Data.Map as Map
import Data.Maybe (fromMaybe)

-- 5. úloha
--
-- 1) Definujte datový typ 'Trie k v' reprezentující trii, kde klíče (řetězce)
-- jsou typu '[k]' a hodnoty typu 'v'.

data Trie k v = Trie (Map.Map k (Trie k v)) (Maybe v) deriving (Eq, Read, Show)

empty :: Trie k v
empty = Trie Map.empty Nothing

--singleton :: [k] -> v -> Trie k v
--singleton k v = Trie { key = [(k, child)], value = Nothing }
--  where
--    child = Trie { key = [], value = (Just v) }



--insertWith :: (Ord k) => (v -> v -> v) -> [k] -> v -> Trie k v -> Trie k v
--insertWith f k v t
--  | t . key == k  = Trie { key = Just(k) , value = Just(v)}
--  | otherwise     = Trie { key = Just(k) , value = Just(v)}


insertWith :: Ord k => (v -> v -> v) -> [k] -> v -> Trie k v -> Trie k v
insertWith f []     v (Trie nodes val) = Trie nodes (f v val)
insertWith f (x:xs) v (Trie nodes val) = Trie (Map.alter (Just . insertWith f xs . fromMaybe empty) x nodes) (Just val)



-- 'insertWith f ks new t' vloží klíč 'ks' s hodnotou 'new' do trie 't'. Pokud
-- trie již klíč 'ks' (s hodnotou 'old') obsahuje, původní hodnota je nahrazena
-- hodnotou 'f new old'.
--
-- > insertWith (++) "a" "x" empty                  == fromList [("a","x")]
-- > insertWith (++) "a" "x" (fromList [("a","y")]) == fromList [("a","xy")]
--
{-
insert :: Ord k => [k] -> v -> Trie k v -> Trie k v
insert []     v (Trie nodes val) = Trie nodes v
insert (x:xs) v (Trie nodes val) = Trie (Map.alter (Just . insert xs . fromMaybe empty) x nodes) val
-}
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

{-
member :: Ord k => [k] -> Trie k v -> Bool
member []     (Trie end _) = end
member (x:xs) (Trie _ nodes) = fromMaybe False (member xs <$> Map.lookup x nodes)
-}
-- 'member k t' zjistí, jestli se klíč 'k' nalézá v trii 't'.
--
-- Hint: použijte 'find'
--
--
-- Funkce 'fromList' není nutná, ale může se vám hodit pro testování.

{-
fromList :: Ord k => [[(k, v)]] -> Trie k v
fromList as = fromList' as empty
  where
    fromList' []     trie = trie
    fromList' (x:xs) trie = fromList' xs $ insert x trie

-}

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