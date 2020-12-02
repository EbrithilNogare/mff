#!/bin/bash




mkdir -p ../test/output


if [[ $1 -eq 1 ]] ; then
	echo "MAKE cecko1"
	make cecko1

	echo -n "test1"
	./stud-main/cecko1 ../test/test1.c | diff ../test/test1.cecko1.gold - > ../test/output/test1.cecko1.txt
	if [ -s ../test/output/test1.cecko1.txt ]
	then
	echo " "
	else
	echo " ---> OK"
	fi

	echo -n "testla-chr-n"
	./stud-main/cecko1 ../test/testla-chr-n.c | diff ../test/testla-chr-n.cecko1.gold - > ../test/output/testla-chr-n.cecko1.txt
	if [ -s ../test/output/testla-chr-n.cecko1.txt ]
	then
	echo " "
	else
	echo " ---> OK"
	fi

	echo -n "testla-cmt1-n"
	./stud-main/cecko1 ../test/testla-cmt1-n.c | diff ../test/testla-cmt1-n.cecko1.gold - > ../test/output/testla-cmt1-n.cecko1.txt
	if [ -s ../test/output/testla-cmt1-n.cecko1.txt ]
	then
	echo " "
	else
	echo " ---> OK"
	fi

	echo -n "testla-cmt2-n"
	./stud-main/cecko1 ../test/testla-cmt2-n.c | diff ../test/testla-cmt2-n.cecko1.gold - > ../test/output/testla-cmt2-n.cecko1.txt
	if [ -s ../test/output/testla-cmt2-n.cecko1.txt ]
	then
	echo " "
	else
	echo " ---> OK"
	fi

	echo -n "testla-num-n"
	./stud-main/cecko1 ../test/testla-num-n.c | diff ../test/testla-num-n.cecko1.gold - > ../test/output/testla-num-n.cecko1.txt
	if [ -s ../test/output/testla-num-n.cecko1.txt ]
	then
	echo " "
	else
	echo " ---> OK"
	fi

	echo -n "testla-str-n"
	./stud-main/cecko1 ../test/testla-str-n.c | diff ../test/testla-str-n.cecko1.gold - > ../test/output/testla-str-n.cecko1.txt
	if [ -s ../test/output/testla-str-n.cecko1.txt ]
	then
	echo " "
	else
	echo " ---> OK"
	fi
fi



if [[ $1 -eq 2 ]] ; then
	echo "MAKE cecko2"
	make cecko2

	echo -n "Test 01"
	./stud-main/cecko2 ../test/test1.c | diff ../test/test1.cecko2.gold - > ../test/output/test1.cecko2.txt
	if [ -s ../test/output/test1.cecko2.txt ]
	then
	echo ""
	cat ../test/output/test1.cecko2.txt
	echo ""
	else
	echo " ---> OK"
	fi


	echo -n "Test sxa arr"
	./stud-main/cecko2 ../test/testsxa-arr.c | diff ../test/testsxa-arr.cecko2.gold - > ../test/output/testsxa-arr.cecko2.txt
	if [ -s ../test/output/testsxa-arr.cecko2.txt ]
	then
	echo ""
	cat ../test/output/testsxa-arr.cecko2.txt
	echo ""
	else
	echo " ---> OK"
	fi


	echo -n "Test sxa enum"
	./stud-main/cecko2 ../test/testsxa-enum.c | diff ../test/testsxa-enum.cecko2.gold - > ../test/output/testsxa-enum.cecko2.txt
	if [ -s ../test/output/testsxa-enum.cecko2.txt ]
	then
	echo ""
	cat ../test/output/testsxa-enum.cecko2.txt
	echo ""
	else
	echo " ---> OK"
	fi

	echo -n "Test sxa expr"
	./stud-main/cecko2 ../test/testsxa-expr.c | diff ../test/testsxa-expr.cecko2.gold - > ../test/output/testsxa-expr.cecko2.txt
	if [ -s ../test/output/testsxa-expr.cecko2.txt ]
	then
	echo ""
	cat ../test/output/testsxa-expr.cecko2.txt
	echo ""
	else
	echo " ---> OK"
	fi

	echo -n "Test sxa arr"
	./stud-main/cecko2 ../test/testsxa-arr.c | diff ../test/testsxa-arr.cecko2.gold - > ../test/output/testsxa-arr.cecko2.txt
	if [ -s ../test/output/testsxa-arr.cecko2.txt ]
	then
	echo ""
	cat ../test/output/testsxa-arr.cecko2.txt
	echo ""
	else
	echo " ---> OK"
	fi

	echo -n "Test sxa stmt"
	./stud-main/cecko2 ../test/testsxa-stmt.c | diff ../test/testsxa-stmt.cecko2.gold - > ../test/output/testsxa-stmt.cecko2.txt
	if [ -s ../test/output/testsxa-stmt.cecko2.txt ]
	then
	echo ""
	cat ../test/output/testsxa-stmt.cecko2.txt
	echo ""
	else
	echo " ---> OK"
	fi

	echo -n "Test sxa struct"
	./stud-main/cecko2 ../test/testsxa-struct.c | diff ../test/testsxa-struct.cecko2.gold - > ../test/output/testsxa-struct.cecko2.txt
	if [ -s ../test/output/testsxa-struct.cecko2.txt ]
	then
	echo ""
	cat ../test/output/testsxa-struct.cecko2.txt
	echo ""
	else
	echo " ---> OK"
	fi

	echo -n "Test sxa typedef"
	./stud-main/cecko2 ../test/testsxa-typedef.c | diff ../test/testsxa-typedef.cecko2.gold - > ../test/output/testsxa-typedef.cecko2.txt
	if [ -s ../test/output/testsxa-typedef.cecko2.txt ]
	then
	echo ""
	cat ../test/output/testsxa-typedef.cecko2.txt
	echo ""
	else
	echo " ---> OK"
	fi


fi






if [[ $1 -eq 3 ]] ; then
	echo "MAKE cecko3"
	make cecko3

	echo -n "Test 01"
	./stud-main/cecko3 ../test/test1.c | diff ../test/test1.cecko3.gold - > ../test/output/test1.cecko3.txt
	if [ -s ../test/output/test1.cecko3.txt ]; then
		echo ""
		cat ../test/output/test1.cecko3.txt
		echo ""
	else
		echo " ---> OK"
	fi


	echo -n "Test 02 typedef"
	./stud-main/cecko3 ../test/test2typedef.c | diff ../test/test2typedef.cecko3.gold - > ../test/output/test2typedef.cecko3.txt
	if [ -s ../test/output/test2typedef.cecko3.txt ]; then
		echo ""
		cat ../test/output/test2typedef.cecko3.txt
		echo ""
	else
		echo " ---> OK"
	fi


	echo -n "Test decl arr"
	./stud-main/cecko3 ../test/testdecl-arr.c | diff ../test/testdecl-arr.cecko3.gold - > ../test/output/testdecl-arr.cecko3.txt
	if [ -s ../test/output/testdecl-arr.cecko3.txt ]; then
		echo ""
		cat ../test/output/testdecl-arr.cecko3.txt
		echo ""
	else
		echo " ---> OK"
	fi

	echo -n "Test decl enum"
	./stud-main/cecko3 ../test/testdecl-enum.c | diff ../test/testdecl-enum.cecko3.gold - > ../test/output/testdecl-enum.cecko3.txt
	if [ -s ../test/output/testdecl-enum.cecko3.txt ]; then
		echo ""
		cat ../test/output/testdecl-enum.cecko3.txt
		echo ""
	else
		echo " ---> OK"
	fi

	echo -n "Test decl struct"
	./stud-main/cecko3 ../test/testdecl-struct.c | diff ../test/testdecl-struct.cecko3.gold - > ../test/output/testdecl-struct.cecko3.txt
	if [ -s ../test/output/testdecl-struct.cecko3.txt ]; then
		echo ""
		cat ../test/output/testdecl-struct.cecko3.txt
		echo ""
	else
		echo " ---> OK"
	fi

	echo -n "Test decl typedef"
	./stud-main/cecko3 ../test/testdecl-typedef.c | diff ../test/testdecl-typedef.cecko3.gold - > ../test/output/testdecl-typedef.cecko3.txt
	if [ -s ../test/output/testdecl-typedef.cecko3.txt ]; then
		echo ""
		cat ../test/output/testdecl-typedef.cecko3.txt
		echo ""
	else
		echo " ---> OK"
	fi
fi





echo "DONE"
