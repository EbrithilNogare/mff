-- https://bigonotetaking.wordpress.com/2015/11/06/a-trie-in-haskell/
-- http://mchaver.com/posts/2018-12-27-tries-in-haskell.html
-- https://blog.jle.im/entry/tries-with-recursion-schemes.html

-- 5. úloha
--
-- 1) Definujte datový typ 'Trie k v' reprezentující trii, kde klíče (řetězce)
-- jsou typu '[k]' a hodnoty typu 'v'.

data NodeType = Top|Word|NonWord deriving (Show,Eq)
data Trie k v = Trie { nodeType :: NodeType, key :: Maybe [k], value :: Maybe v, childs :: [(Trie k v)] } deriving (Show)

empty :: Trie k v
empty = Trie Top Nothing Nothing []

singleton :: [k] -> v -> Trie k v
singleton k v = Trie { nodeType = Top, key = Nothing, value = Nothing, childs = [child] }
  where
    child = Trie { nodeType = Word, key = Just(k), value = Just(v), childs = [] } 



insertWith :: (Ord k) => (v -> v -> v) -> [k] -> v -> Trie k v -> Trie k v
insertWith f [] v (Trie _ v)     = Trie Word m v []
insertWith (x:xs) v (Trie k v) = Trie { nodeType = Word, key = Just(k), value = Just(v), childs = [] }

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