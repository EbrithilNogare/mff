// unit testy pro vicesouborove zarovnani
// Author: David Napravnik

/*
using Microsoft.VisualStudio.TestTools.UnitTesting;
using alignToBlock;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using alignToBlockTests;

namespace alignToBlock.Tests
{
    [TestClass()]
    public class AligmentorTests
    {
        [TestMethod()]
        public void AlignFilesTest01()
        {
            Aligmentor a = new Aligmentor();
            string[] args = "plain.txt format.out 17".Split(' ');

            a.AlignFiles(args);

            FileAssert.AreEqual("format.txt", "format.out");
        }
        [TestMethod()]
        public void AlignFilesTestEx01()
        {
            Aligmentor a = new Aligmentor();
            string[] args = "--highlight-spaces 01.in 01.out 17".Split(' ');

            a.AlignFiles(args);

            FileAssert.AreEqual("01.out", "ex01.out");
        }

        [TestMethod()]
        public void AlignFilesTestEx02()
        {
            Aligmentor a = new Aligmentor();
            string[] args = "--highlight-spaces 01.in 01.in 01.in 02.out 17".Split(' ');

            a.AlignFiles(args);

            FileAssert.AreEqual("02.out", "ex02.out");
        }

        [TestMethod()]
        public void AlignFilesTestEx08()
        {
            Aligmentor a = new Aligmentor();
            string[] args = "--highlight-spaces xx.in xx.in xx.in 01.in xx.in xx.in 08.out 80".Split(' ');

            a.AlignFiles(args);

            FileAssert.AreEqual("08.out", "ex08.out");
        }

        [TestMethod()]
        public void AlignFilesTestEx12()
        {
            Aligmentor a = new Aligmentor();
            string[] args = "01.in 01.in 01.in 12.out 17".Split(' ');

            a.AlignFiles(args);

            FileAssert.AreEqual("12.out", "ex12.out");
        }
    }
}
*/