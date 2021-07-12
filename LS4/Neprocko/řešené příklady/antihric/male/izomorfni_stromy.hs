import Kombinace

data Bt a = Void | Node (Bt a) a (Bt a) deriving (Eq,Show)

--http://forum.matfyz.info/viewtopic.php?f=169&t=8368&p=34206&hilit=stromy#p34206

testStrom = Node (Node (Node Void 10 Void) 25 (Node (Node Void 13 Void) 15 (Node Void 18 Void))) 50 (Node Void 75 (Node (Node Void 80 Void) 100 Void))
testStrom2 = Node Void 75 (Node (Node Void 80 Void) 100 Void)

testStrom3 = Node (Node (Node (Void) 10 (Void)) 25 (Node (Void) 15 (Void))) 50 (Node (Void) 75 (Node Void 100 Void))

pocetMoznychProhozeni :: Eq a => Bt a -> Int
pocetMoznychProhozeni Void = 0
pocetMoznychProhozeni (Node l v r) | l /= Void && r /= Void     = 1 + pocetMoznychProhozeni l + pocetMoznychProhozeni r
                                   | otherwise                  = pocetMoznychProhozeni l + pocetMoznychProhozeni r

izo :: Eq a => Int -> Bt a -> [Bt a]
izo n bt | n > c        = error $ "Maximální počet prohození je " ++ show c ++ ", požadovaný však " ++ show n ++ "."
         | otherwise    = map snd ts
    where
        c = pocetMoznychProhozeni bt
        ks = kombinace n [0..c-1]
        ts = map (\k -> izo' n k 0 bt) ks

izo' :: Eq a => Int -> [Int] -> Int -> Bt a -> (Int, Bt a)
izo' n k a (Node l v r) | l /= Void && r /= Void        = case elem a k of 
                                                                True  -> (acc', Node nr v nl)
                                                                False -> (acc', Node nl v nr)

                        | r /= Void                     = (bcc', Node Void v nr')
                        | l /= Void                     = (bcc, Node nl' v Void)
                        | l == Void && r == Void        = (a, Node Void v Void)

        where
           (acc, nl) = izo' n k (a+1) l
           (acc', nr) = izo' n k acc r

           (bcc, nl') = izo' n k a l
           (bcc', nr') = izo' n k a r
