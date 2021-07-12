import Kombinace
import MnozinoveOp
import Functional

--3. Zadán řetězec s, číslo n. Vypsat řetězce, které mají stejnou délku jako vstupní řetězec
--s a liší se max o n znaků. Seznam znaků, se kterými se pracuje vydá nulární funkce abeceda :: [Char]. 
--Navíc měl být na výstupu seznam řetězců uspořádaný. 
--Takže něco jako: (uvažujeme-li abeceda = [a,b,c]) "aaa" 1 -> ["aaa","aab","aac","aba", ...] 

--http://forum.matfyz.info/viewtopic.php?f=169&t=8317&p=34069&hilit=Vypsat+%C5%99et%C4%9Bzce#p34069

change :: Int -> [Char] -> String -> [String]
change n abc txt = qsort $ txt : [c | i <- [1..n], c <- changed i abc txt]

changed :: Int -> [Char] -> String -> [String]
changed i abc txt = [ putAt j t txt | k <- ks, j <- k, t <- mozneNahrady j abc txt]
    where ks = kombinace i [0..length txt-1]

mozneNahrady :: Int -> [Char] -> String -> [Char]
mozneNahrady i abc txt = difference abc [txt!!i]