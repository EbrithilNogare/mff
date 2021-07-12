%http://forum.matfyz.info/viewtopic.php?f=169&t=11747

gen([X], _, X).
gen([A, B | Atoms], Operators, V) :-
    member(Op, Operators),
    X =.. [Op, A, B],
    gen([X | Atoms], Operators, V).