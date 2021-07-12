data NTree a = N a [NTree a]

--http://forum.matfyz.info/viewtopic.php?f=169&t=10104&p=39368&hilit=html#p39368

testStrom = N "html" [N "head" [], N "body" [N "a" [], N "h2" []]]

vypis :: NTree String -> String
vypis (N s []) = "<" ++ s ++ ">" ++ "<" ++ s ++ "/>"
vypis (N s xs) = btag ++ vnitrek ++ etag
        where
            btag = "<" ++ s ++ ">"
            etag = "</" ++ s ++ ">"
            vnitrek = concat $ map vypis xs