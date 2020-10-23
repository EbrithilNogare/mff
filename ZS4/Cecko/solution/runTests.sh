#!/bin/bash

echo "MAKE"
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


echo "DONE"
