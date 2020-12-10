#!/bin/bash

mkdir -p ../test/output


if [[ $1 -eq "custom" ]] ; then
	echo "MAKE cecko"$2
	make cecko$2

	echo "Test Custom"
	./stud-main/cecko$2 ../test/custom.c

fi


if [[ $1 -eq 1 ]] ; then
	echo "MAKE cecko1"
	make cecko1

	echo -n "test1"
	./stud-main/cecko1 ../test/test1.c | diff ../test/test1.cecko1.gold - > ../test/output/test1.cecko1.txt
	if [ -s ../test/output/test1.cecko1.txt ]; then
		echo " "
	else
		echo " ---> OK"
	fi

	echo -n "testla-chr-n"
	./stud-main/cecko1 ../test/testla-chr-n.c | diff ../test/testla-chr-n.cecko1.gold - > ../test/output/testla-chr-n.cecko1.txt
	if [ -s ../test/output/testla-chr-n.cecko1.txt ]; then
		echo " "
	else
		echo " ---> OK"
	fi

	echo -n "testla-cmt1-n"
	./stud-main/cecko1 ../test/testla-cmt1-n.c | diff ../test/testla-cmt1-n.cecko1.gold - > ../test/output/testla-cmt1-n.cecko1.txt
	if [ -s ../test/output/testla-cmt1-n.cecko1.txt ]; then
		echo " "
	else
		echo " ---> OK"
	fi

	echo -n "testla-cmt2-n"
	./stud-main/cecko1 ../test/testla-cmt2-n.c | diff ../test/testla-cmt2-n.cecko1.gold - > ../test/output/testla-cmt2-n.cecko1.txt
	if [ -s ../test/output/testla-cmt2-n.cecko1.txt ]; then
		echo " "
	else
		echo " ---> OK"
	fi

	echo -n "testla-num-n"
	./stud-main/cecko1 ../test/testla-num-n.c | diff ../test/testla-num-n.cecko1.gold - > ../test/output/testla-num-n.cecko1.txt
	if [ -s ../test/output/testla-num-n.cecko1.txt ]; then
		echo " "
	else
		echo " ---> OK"
	fi

	echo -n "testla-str-n"
	./stud-main/cecko1 ../test/testla-str-n.c | diff ../test/testla-str-n.cecko1.gold - > ../test/output/testla-str-n.cecko1.txt
	if [ -s ../test/output/testla-str-n.cecko1.txt ]; then
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
	if [ -s ../test/output/test1.cecko2.txt ]; then
		echo ""; cat ../test/output/test1.cecko2.txt; echo ""
	else
		echo " ---> OK"
	fi


	echo -n "Test sxa arr"
	./stud-main/cecko2 ../test/testsxa-arr.c | diff ../test/testsxa-arr.cecko2.gold - > ../test/output/testsxa-arr.cecko2.txt
	if [ -s ../test/output/testsxa-arr.cecko2.txt ]; then
		echo ""; cat ../test/output/testsxa-arr.cecko2.txt; echo ""
	else
		echo " ---> OK"
	fi


	echo -n "Test sxa enum"
	./stud-main/cecko2 ../test/testsxa-enum.c | diff ../test/testsxa-enum.cecko2.gold - > ../test/output/testsxa-enum.cecko2.txt
	if [ -s ../test/output/testsxa-enum.cecko2.txt ]; then
		echo ""; cat ../test/output/testsxa-enum.cecko2.txt; echo ""
	else
		echo " ---> OK"
	fi

	echo -n "Test sxa expr"
	./stud-main/cecko2 ../test/testsxa-expr.c | diff ../test/testsxa-expr.cecko2.gold - > ../test/output/testsxa-expr.cecko2.txt
	if [ -s ../test/output/testsxa-expr.cecko2.txt ]; then
		echo ""; cat ../test/output/testsxa-expr.cecko2.txt; echo ""
	else
		echo " ---> OK"
	fi

	echo -n "Test sxa arr"
	./stud-main/cecko2 ../test/testsxa-arr.c | diff ../test/testsxa-arr.cecko2.gold - > ../test/output/testsxa-arr.cecko2.txt
	if [ -s ../test/output/testsxa-arr.cecko2.txt ]; then
		echo ""; cat ../test/output/testsxa-arr.cecko2.txt; echo ""
	else
		echo " ---> OK"
	fi

	echo -n "Test sxa stmt"
	./stud-main/cecko2 ../test/testsxa-stmt.c | diff ../test/testsxa-stmt.cecko2.gold - > ../test/output/testsxa-stmt.cecko2.txt
	if [ -s ../test/output/testsxa-stmt.cecko2.txt ]; then
		echo ""; cat ../test/output/testsxa-stmt.cecko2.txt; echo ""
	else
		echo " ---> OK"
	fi

	echo -n "Test sxa struct"
	./stud-main/cecko2 ../test/testsxa-struct.c | diff ../test/testsxa-struct.cecko2.gold - > ../test/output/testsxa-struct.cecko2.txt
	if [ -s ../test/output/testsxa-struct.cecko2.txt ]; then
		echo ""; cat ../test/output/testsxa-struct.cecko2.txt; echo ""
	else
		echo " ---> OK"
	fi

	echo -n "Test sxa typedef"
	./stud-main/cecko2 ../test/testsxa-typedef.c | diff ../test/testsxa-typedef.cecko2.gold - > ../test/output/testsxa-typedef.cecko2.txt
	if [ -s ../test/output/testsxa-typedef.cecko2.txt ]; then
		echo ""; cat ../test/output/testsxa-typedef.cecko2.txt; echo ""
	else
		echo " ---> OK"
	fi


fi


if [[ $1 -eq 3 ]] ; then
	echo "MAKE cecko3"
	make cecko3

	echo "Test base constptr"
	./stud-main/cecko3 ../test/testdecl-constptr.c > ../test/output/testdecl-constptr.cecko3.txt
	diff ../test/testdecl-constptr.cecko3.gold ../test/output/testdecl-constptr.cecko3.txt
	echo ""

	echo "Test base elementary"
	./stud-main/cecko3 ../test/testdecl-elementary.c > ../test/output/testdecl-elementary.cecko3.txt
	diff ../test/testdecl-elementary.cecko3.gold ../test/output/testdecl-elementary.cecko3.txt
	echo ""
	
	echo "Test base function"
	./stud-main/cecko3 ../test/testdecl-function.c > ../test/output/testdecl-function.cecko3.txt
	diff ../test/testdecl-function.cecko3.gold ../test/output/testdecl-function.cecko3.txt
	echo ""
	
	echo "Test base funptr"
	./stud-main/cecko3 ../test/testdecl-funptr.c > ../test/output/testdecl-funptr.cecko3.txt
	diff ../test/testdecl-funptr.cecko3.gold ../test/output/testdecl-funptr.cecko3.txt
	echo ""
	
	echo "Test base local"
	./stud-main/cecko3 ../test/testdecl-local.c > ../test/output/testdecl-local.cecko3.txt
	diff ../test/testdecl-local.cecko3.gold ../test/output/testdecl-local.cecko3.txt
	echo ""
	
	echo "Test base pointer"
	./stud-main/cecko3 ../test/testdecl-pointer.c > ../test/output/testdecl-pointer.cecko3.txt
	diff ../test/testdecl-pointer.cecko3.gold ../test/output/testdecl-pointer.cecko3.txt
	echo ""
	
	echo "Test bonus arr"
	./stud-main/cecko3 ../test/testdecl-arr.c > ../test/output/testdecl-arr.cecko3.txt
	diff ../test/testdecl-arr.cecko3.gold ../test/output/testdecl-arr.cecko3.txt
	echo ""
	
	echo "Test bonus enum"
	./stud-main/cecko3 ../test/testdecl-enum.c > ../test/output/testdecl-enum.cecko3.txt
	diff ../test/testdecl-enum.cecko3.gold ../test/output/testdecl-enum.cecko3.txt
	echo ""
	
	echo "Test bonus struct"
	./stud-main/cecko3 ../test/testdecl-struct.c > ../test/output/testdecl-struct.cecko3.txt
	diff ../test/testdecl-struct.cecko3.gold ../test/output/testdecl-struct.cecko3.txt
	echo ""
	
	echo "Test bonus typedef"
	./stud-main/cecko3 ../test/testdecl-typedef.c > ../test/output/testdecl-typedef.cecko3.txt
	diff ../test/testdecl-typedef.cecko3.gold ../test/output/testdecl-typedef.cecko3.txt
	echo ""
	
fi

echo "DONE"