import qualified Control.Monad as M
import           Data.Char
import           Data.Maybe
import           Data.List

-- 6. úloha
--
-- 1) Jednoduchý backtrackovací parser lze reprezentovat jako funkci, která
-- na vstup dostane text, který cheme parsovat, a vrátí zpět buď 'Nothing'
-- (pokud parser neuspěl) nebo 'Just (a,s)', kde 'a' je naparsovaná hodnota
-- a 's' je zbytek vstupu (tj. to, co parser nezkonzumoval).
--
-- Konkrétně budeme používat následující typ:

newtype Parser a = Parser { runParser :: String -> Maybe (a, String) }

-- Definujte:

satisfy :: (Char -> Bool) -> Parser Char
satisfy f = Parser $ \s -> satisfyBody f s

satisfyBody :: (Char -> Bool) -> String -> Maybe (Char, String)
satisfyBody f (s:xs) = if f s then Just(s,xs) else Nothing
satisfyBody f ""     = Nothing



-- 'satisfy p' je základní parser. Funguje takto: podívá se na vstupní text,
-- pokud je text prázdný, tak selže (tj. vrátí 'Nothing'), jinak se podívá na
-- první znak. Pokud pro tento znak platí podmínka daná funkcí 'p', tak uspěje,
-- vrátí zpět rozpoznaný znak a zbytek textu. V opačném případě selže.
--
-- > runParser (satisfy (== 'x')) "ab"
-- Nothing
--
-- > runParser (satisfy (== 'x')) ""
-- Nothing
--
-- > runParser (satisfy (== 'x')) "xabc"
-- Just ('x',"abc")

failure :: Parser a
failure = Parser $ \s -> Nothing

-- 'failure' je parser, který vždy selže.
--
-- > runParser failure "abc"
-- Nothing

orElse :: Parser a -> Parser a -> Parser a
orElse p1 p2 = Parser $ \s -> 
  if isJust(runParser p1 s) then
    runParser p1 s 
  else
    runParser p2 s
  
  

-- 'orElse p1 p2' (doporučuji používat jako 'p1 `orElse` p2') je parser, který
-- nejprve zkusí provést 'p1'. Pokud 'p1' uspěje s hodnotou 'a', vrátí zpátky
-- 'a' a skončí. Pokud 'p1' neuspěje, zkusí parser 'p2'.
--
-- > runParser (satisfy (== 'a') `orElse` satisfy (== 'b')) "abc"
-- Just ('a',"bc")
--
-- > runParser (satisfy (== 'a') `orElse` satisfy (== 'b')) "bc"
-- Just ('b',"c")
--
-- > runParser (satisfy (== 'a') `orElse` satisfy (== 'b')) "c"
-- Nothing
--
--
-- Máme základní parser a výběr mezi dvěma (a více) parsery. Zbývá implementovat
-- spojování parserů.

parserReturn :: a -> Parser a
parserReturn a = Parser $ \s -> Just (a, s)

-- 'parserReturn a' je parser, který vždy uspěje a vrátí hodnotu 'a'. Vstupní
-- text ponechá beze změny.
--
-- runParser (parserReturn 1) "abc" == Just (1,"abc")


parserBind :: Parser a -> (a -> Parser b) -> Parser b
parserBind p f = Parser $ \s ->
  let val = runParser p s in
  if isNothing(val) then
    Nothing
  else
    let (valA, valB) = fromJust(val) in
    runParser (f valA) valB
  

-- 'parserBind m f' nejprve spustí parser m. Pokud tenhle parser uspěje s hodnotou
-- 'a', tak pustí parser 'f a'.

instance Functor Parser where
    fmap = M.liftM

instance Applicative Parser where
    pure  = return
    (<*>) = M.ap

instance Monad Parser where
    return = parserReturn
    (>>=)  = parserBind

-- Správnost implementace 'satisfy', 'parserReturn' a 'parserBind' pomůže
-- ověřit následující definice. Pokud se v GHCi zeptáte na hodnotu 'correct',
-- měli byste dostat zpět 'True'.
correct :: Bool
correct = and
    [ p ""    == Nothing
    , p "a"   == Nothing
    , p " "   == Nothing
    , p " a"  == Nothing
    , p "  "  == Nothing
    , p "aa"  == Nothing
    , p "a "  == Just ("a ", "")
    , p "a b" == Just ("a ", "b")
    ]
  where
    p = runParser $ do
        a <- satisfy (not . isSpace)
        b <- satisfy isSpace
        return [a,b]

-- 2) V této části nepoužívejte konstruktor 'Parser'. Místo toho parsery
-- sestavte pomocí dříve definovaných funkcí.

string :: String -> Parser String
string prefix = Parser $ \s ->
  let val = (stripPrefix prefix s) in
  if isNothing val then
    Nothing
  else
    Just(prefix, fromJust val)


-- 'string s' je parser, který parsuje přesně řetězec 's'. Parser vrací
-- naparsovaný řetězec.
--
-- > runParser (string "hello") "hello there"
-- Just ("hello"," there")
--
-- > runParser (string "hello") "hell"
-- Nothing

many :: Parser a -> Parser [a]
many p = Parser $ \s ->
  let val = runParser p s in
  if isNothing val then
    Just([], s)
  else
    let (valA, valB) = fromJust(val) in
    let (recValA, recValB) = fromJust(runParser (many p) valB) in
    Just(concat [[valA], recValA], recValB)



some :: Parser a -> Parser [a]
some p = Parser $ \s ->
  let val = runParser p s in
  if isNothing val then
    Nothing
  else
    let (valA, valB) = fromJust(val) in
    let (recValA, recValB) = fromJust(runParser (many p) valB) in
    Just(concat [[valA], recValA], recValB)


-- Funkce 'many' a 'some' dostanou jako vstup parser 'p' a aplikují jej,
-- dokud 'p' neselže. Výsledky všech použití parseru 'p' se nashromáždí
-- do seznamu.
--
-- 'many' zkusí najít 0 a více výskutů. 'some' zkusí najít 1 a více výskytů.
--
-- Hint: Bude se vám hodit funkce 'orElse'.
--
-- Hint: 'some' a 'many' se dají jednoduše definovat mutuální rekurzí,
-- tj. 'some' je definovaný pomocí 'many', 'many' pomocí 'some'.
--
-- > runParser (some $ string "ab") "aabab"
-- Nothing
--
-- > runParser (some $ string "ab") "ababababbb"
-- Just (["ab","ab","ab","ab"],"bb")
--
-- > runParser (many $ string "ab") "aaaaaaa"
-- Just ([],"aaaaaaa")

-- BONUS) Implementuje následující parser. Stejně jako v předchozí části
-- nepoužívejte 'Parser' explicitně.

whitespace :: Parser ()
whitespace = Parser $ \s -> 
  let spaceParser = string " " in
  let newLineParser = string "\n" in
  let comment = string "//" >>= \s -> many (satisfy (/= '\n')) >>= \s -> string "\n" in
  let multiLineComment = string "/*" >>= \s -> many (many (satisfy (/= '*')) >>= \s -> satisfy (/= '/')) >>= \s -> string "/" in
  let myParser = orElse (orElse (orElse comment multiLineComment) newLineParser) spaceParser  in
  let val = runParser myParser s in
  if val == Nothing then
    Just((), s)
  else
    let (_, valB) = fromJust(val) in
    let (_, rest) = fromJust(runParser whitespace valB) in
    Just((), rest)



-- Pokud chcete parsovat nějaký zdrojový kód, velice často potřebujete přeskakovat
-- bílé znaky (mezery, tabulátory, konce řádků). Normálně by tohle šlo vyřešit
-- snadno pomocí:
--
--   spaces = many $ satisfy isSpace
--
-- Funkce 'isSpace' je z modulu Data.Char.
--
-- Ale ve většině zdrojových kódu se také objevují komentáře, které se
-- také musejí přeskočit.
--
-- Definujte tedy parser 'whitespace', který přeskočí všechny bílé znaky
-- (včetně komentářů). Komentáře máme jednodřádkové, začínající řetězcem
-- "//", nebo víceřádkové, začínající "/*" a končící "*/".
--
-- Použijte přitom dříve definované funkce, bude se vám hodit 'string',
-- 'satisfy', 'some' a 'many'.
--
-- > runParser whitespace " x += 2;"
-- Just ((),"x += 2;")
--
-- > runParser whitespace "  // This adds 2 to x\n  x += 2;"
-- Just ((),"x += 2;")
--
-- > runParser whitespace "/*\n  This doesn't work:\n  x *= 2;\n*/\n  x += 2;"
-- Just ((),"x += 2;")







main = do
  print("tests pass:")
  print correct
  print $ runParser (string "hello") "hello there" == Just ("hello"," there")
  print $ runParser (string "hello") "hell" == Nothing
  print $ runParser (some $ string "ab") "aabab" == Nothing
  print $ runParser (some $ string "ab") "ababababbb" == Just (["ab","ab","ab","ab"],"bb")
  print $ runParser (many $ string "ab") "aaaaaaa" == Just ([],"aaaaaaa")
  print $ runParser whitespace " x += 2;" == Just ((),"x += 2;")
  print $ runParser whitespace "  // This adds 2 to x\n  x += 2;" == Just ((),"x += 2;")
  print $ runParser whitespace "/*\n  This doesn't work:\n  x *= 2;\n*/\n  x += 2;" == Just ((),"x += 2;")