/*/
using Microsoft.VisualStudio.TestTools.UnitTesting;
using excel;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace excel.Tests
{
    [TestClass()]
    public class ExcelTests
    {
        [TestMethod()]
        public void AlphabetToIntTest()
        {
            var excel = new Excel();
            for (int i = 0; i < 1; i++)
            {
                if (!(
                    excel.AlphabetToInt("A") == 0 &&
                    excel.AlphabetToInt("B") == 1 &&
                    excel.AlphabetToInt("AA") == 26 &&
                    excel.AlphabetToInt("Z") == 25 &&
                    excel.AlphabetToInt("AZ") == 51 &&
                    excel.AlphabetToInt("BA") == 52
                    ))
                    Assert.Fail();
            }
        }

        [TestMethod()]
        public void isCellNameTest()
        {
            var excel = new Excel();

            Assert.AreEqual(excel.isCellName("A1"), true);
            Assert.AreEqual(excel.isCellName("AA1"), true);
            Assert.AreEqual(excel.isCellName("A11"), true);
            Assert.AreEqual(excel.isCellName("AA11"), true);
            Assert.AreEqual(excel.isCellName("Z9"), true);
            Assert.AreEqual(excel.isCellName("A" + int.MaxValue.ToString()), true);
            Assert.AreEqual(excel.isCellName(""), false);
            Assert.AreEqual(excel.isCellName("a1"), false);
            Assert.AreEqual(excel.isCellName("aa1"), false);
            Assert.AreEqual(excel.isCellName("A"), false);
            Assert.AreEqual(excel.isCellName("1"), false);
            Assert.AreEqual(excel.isCellName("@1"), false);
            Assert.AreEqual(excel.isCellName("[1"), false);
            Assert.AreEqual(excel.isCellName("1A1"), false);
            Assert.AreEqual(excel.isCellName("{0}"), false);
            Assert.AreEqual(excel.isCellName("1A"), false);
            Assert.AreEqual(excel.isCellName("AAAAA11111_"), false);
            Assert.AreEqual(excel.isCellName("AAAAA_11111"), false);
        }

        [TestMethod()]
        public void getCellnameTest()
        {
            var excel = new Excel();
            excel.getCellname("AZ27", out int row, out int column);
            if (!(
                 row == 26 &&
                 column == 51
                ))
                Assert.Fail(row + " " + column);
        }

        public static Stream GenerateStreamFromString(string s)
        {
            var stream = new MemoryStream();
            var writer = new StreamWriter(stream);
            writer.Write(s);
            writer.Flush();
            stream.Position = 0;
            return stream;
        }

        [TestMethod()]
        public void createTableTest()
        {
            var excel = new Excel();

            var result = new List<List<string>>();
            result.Add(new List<string>());
            result[0].Add("0");
            result[0].Add("1");
            result[0].Add("=A1+B2");
            result.Add(new List<string>());
            result[1].Add("6");




            using (StreamReader sr = new StreamReader(GenerateStreamFromString("0 1 =A1+B2\n6")))
            {
                excel.createTable(sr);
                Assert.AreEqual(excel.table.Count, result.Count);
                for (int i = 0; i < excel.table.Count; i++)
                {
                    Assert.AreEqual(excel.table[i].Count, result[i].Count);
                    for (int j = 0; j < excel.table[i].Count; j++)
                    {
                        Assert.AreEqual(excel.table[i][j], result[i][j]);
                    }
                }
            }
        }

        [TestMethod()]
        public void computeFormulaTest()
        {
            var excel = new Excel();
            Assert.AreEqual(excel.computeFormula('+', 1, 2), "3");
            Assert.AreEqual(excel.computeFormula('-', 1, 2), "-1");
            Assert.AreEqual(excel.computeFormula('-', 3, 2), "1");
            Assert.AreEqual(excel.computeFormula('*', 3, 2), "6");
            Assert.AreEqual(excel.computeFormula('/', 10, 2), "5");
            Assert.AreEqual(excel.computeFormula('/', 10, 3), "3");
            Assert.AreEqual(excel.computeFormula('/', 1, 2), "0"); // ???
            Assert.AreEqual(excel.computeFormula('/', 1, 0), "#DIV0");
        }
    }
}

/*/