--vsechny mozne varianty ocislovani vrcholu stromu (casto se recykluje)

--n-arni stromy

data NT a = N a [NT a] deriving Show

testStrom = N 10 [N 5 [N 3 [], N 8 []], N 12 [], N 40 [N 35 [], N 37 [], N 42 [], N 43 []]]
testStrom2 = N 50 [ N 10 [], N 20 [], N 30 [], N 40 [N 45 []]] 

postorderOcisluj :: NT a -> NT (a, Int)
postorderOcisluj strom = s
    where
        (s, _) = postorderOcisluj' strom 0

        postorderOcisluj' :: NT a -> Int -> (NT (a, Int), Int)
        postorderOcisluj' (N h xs) p = (N (h, q) ys, q+1)
            where
                (ys, q) = postorderOcislujSeznam xs p

        postorderOcislujSeznam :: [NT a] -> Int -> ([NT (a, Int)],Int)
        postorderOcislujSeznam [] p = ([],p)
        postorderOcislujSeznam (x : xs) p = ((y : ys), r)
            where
                (y, q) = postorderOcisluj' x p
                (ys, r) = postorderOcislujSeznam xs q



preorderOcisluj :: NT a -> NT (a, Int)
preorderOcisluj strom = s
    where
        (s,_) = preorderOcisluj' strom 0

        preorderOcisluj' :: NT a -> Int -> (NT (a, Int),Int)
        preorderOcisluj' (N h xs) p = (N (h, p) ys, q)
            where
                (ys, q) = preorderOcislujSeznam xs (p+1)
        
        preorderOcislujSeznam :: [NT a] -> Int -> ([NT (a, Int)], Int)
        preorderOcislujSeznam [] q = ([],q)
        preorderOcislujSeznam (x : xs) q = ((y : ys), t)
            where
                (y, r) = preorderOcisluj' x q
                (ys, t) = preorderOcislujSeznam xs r


--binarni stromy
data BST a = Nil | Tree (BST a) a (BST a) deriving Show

bstTest = Tree (Tree (Tree Nil 10 Nil) 25 (Tree Nil 35 Nil)) 50 (Tree (Tree Nil 60 Nil) 75 Nil)

preorderOcislujb :: BST a -> BST (a, Int)
preorderOcislujb strom = s
    where
        (s, _) = preorderOcislujb' strom 0

        preorderOcislujb' :: BST a -> Int -> (BST (a, Int), Int)
        preorderOcislujb' Nil p = (Nil, p)
        preorderOcislujb' (Tree l h r) p = (Tree ll (h,p) rr, s)
            where
                (ll, q) = preorderOcislujb' l (p+1)
                (rr, s) = preorderOcislujb' r q

postorderOcislujb :: BST a -> BST (a, Int)
postorderOcislujb strom = s
    where
        (s, _) = postorderOcislujb' strom 0

        postorderOcislujb' :: BST a -> Int -> (BST (a, Int), Int)
        postorderOcislujb' Nil p = (Nil, p)
        postorderOcislujb' (Tree l h r) p = (Tree ll (h,s) rr, s+1)
            where
                (ll, q) = postorderOcislujb' l p
                (rr, s) = postorderOcislujb' r q


inorderOcislujb :: BST a -> BST (a, Int)
inorderOcislujb strom = s
    where
        (s, _) = inorderOcislujb' strom 0

        inorderOcislujb' :: BST a -> Int -> (BST (a, Int), Int)
        inorderOcislujb' Nil p = (Nil, p)
        inorderOcislujb' (Tree l h r) p = ((Tree ll (h,q) rr),s)
            where
                (ll, q) = inorderOcislujb' l p 
                (rr, s) = inorderOcislujb' r (q + 1)