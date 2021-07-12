module DfsCasy(dfsCasy) where
import Functional

--http://forum.matfyz.info/viewtopic.php?f=169&t=11756&p=41531&hilit=dfs#p41531 (akorat zadano v prologu)

data Graf a = G [a] [(a,a)] deriving Show
testGraf = G ['a','b','c','d','e','s'] [('a','s'),('a','b'), ('a','c'), ('b','d'), ('b','e'), ('c','e'), ('d','e')]

sousedi :: Eq a => Graf a -> a -> [a]
sousedi (G v e) u = foldr (\(x,y) a -> if x == u then y:a else if y == u then x:a else a) [] e

dfsCasy :: Eq a => Graf a -> a -> [(a, Int, Int)]
dfsCasy g a = v
        where
            (_,_, v) = dfsCasy' g [a] 0

dfsCasy' :: Eq a => Graf a -> [a] -> Int -> ([a] ,Int, [(a, Int, Int)])
dfsCasy' g xs@(x : _) c | null pripustniSousedi = (xs,c+2, [(x,c,c+1)])
                        | otherwise             = (nvisited,cislo+1, (x,c,cislo):vs)                     
        where
            d = c + 1
            pripustniSousedi = filter (\y -> not $ elem y xs) (sousedi g x)
            (nvisited, cislo, vs) = zavolej pripustniSousedi xs g d []


zavolej :: Eq a => [a] -> [a] -> Graf a -> Int -> [(a, Int, Int)] -> ([a], Int, [(a, Int, Int)])
zavolej [] visited g d vd = (visited, d, vd)
zavolej (p : ps) visited g d vd  | elem p visited  = zavolej ps visited g d vd
                                 | otherwise       = zavolej ps nvisited g e (vd++vs)
        where (nvisited, e, vs) = dfsCasy' g (p:visited) d