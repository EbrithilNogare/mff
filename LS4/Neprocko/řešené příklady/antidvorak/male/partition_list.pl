%mate danou mnozinu, vypiste vsechny jeji rozklady
%cesky to znamena partition set
%http://forum.matfyz.info/viewtopic.php?f=169&t=11944

splitList([],[],[]).
splitList([X | Xs], [X | Ys], Zs) :- splitList(Xs, Ys, Zs).
splitList([X | Xs], Ys, [X | Zs]) :- splitList(Xs, Ys, Zs).

partition_list([],[]).
partition_list([X | Xs], [ [X | Ys] | Rest ]) :-
    splitList(Xs, Ys, Zs),
    partition_list(Zs, Rest).