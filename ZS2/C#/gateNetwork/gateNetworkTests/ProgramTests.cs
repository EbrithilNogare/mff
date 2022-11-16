using Microsoft.VisualStudio.TestTools.UnitTesting;
using gateNetwork;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace gateNetwork.Tests
{
    [TestClass()]
    public class ProgramTests
    {

        [TestMethod(), Timeout(5000)]
        public void MainTestArgumentError()
        {
            using (StringWriter sw = new StringWriter())
            {
                Console.SetOut(sw);


                string[] args = new string[0];
                Program.Main(args);


                string expected = string.Format("Argument error.{0}", Environment.NewLine);
                Assert.AreEqual<string>(expected, sw.ToString());
            }
        }

        [TestMethod(), Timeout(5000)]
        public void MainTestFileError()
        {
            using (StringWriter sw = new StringWriter())
            {
                Console.SetOut(sw);


                string[] args = new string[] { "notExistingFileAtAllIHope.in" };
                Program.Main(args);


                string expected = string.Format("File error.{0}", Environment.NewLine);
                Assert.AreEqual<string>(expected, sw.ToString());
            }
        }

        //test folder is in ...\gateNetwork\gateNetworkTests\bin\Debug
        [TestMethod(), Timeout(5000)]
        public void MainTestFile01()
        {
            string[] args = new string[] { "testFiles/01/hradla.in" }; // file with network
            string consoleInputFile = "testFiles/01/std.in";
            string fileWithSolution = "testFiles/01/std.out";

            using (StringWriter sw = new StringWriter())
            using (StreamReader srIn = new StreamReader(consoleInputFile))
            using (StreamReader srOut = new StreamReader(fileWithSolution))
            {
                Console.SetOut(sw);
                Console.SetIn(srIn);


                Program.Main(args);


                StringBuilder expected = new StringBuilder();
                string input;
                while ((input = srOut.ReadLine()) != null)
                {
                    expected.Append(input);
                    expected.Append(Environment.NewLine);
                }
                Assert.AreEqual<string>(expected.ToString(), sw.ToString());
            }
        }//input diferences checking
        [TestMethod(), Timeout(5000)]
        public void MainTestFile04()
        {
            string[] args = new string[] { "testFiles/04/hradla.in" }; // file with network
            string consoleInputFile = "testFiles/04/std.in";
            string fileWithSolution = "testFiles/04/std.out";

            using (StringWriter sw = new StringWriter())
            using (StreamReader srIn = new StreamReader(consoleInputFile))
            using (StreamReader srOut = new StreamReader(fileWithSolution))
            {
                Console.SetOut(sw);
                Console.SetIn(srIn);


                Program.Main(args);


                StringBuilder expected = new StringBuilder();
                string input;
                while ((input = srOut.ReadLine()) != null)
                {
                    expected.Append(input);
                    expected.Append(Environment.NewLine);
                }
                Assert.AreEqual<string>(expected.ToString(), sw.ToString());
            }
        }
        [TestMethod(), Timeout(5000)]
        public void MainTestFile05()
        {
            string[] args = new string[] { "testFiles/05/hradla.in" }; // file with network
            string consoleInputFile = "testFiles/05/std.in";
            string fileWithSolution = "testFiles/05/std.out";

            using (StringWriter sw = new StringWriter())
            using (StreamReader srIn = new StreamReader(consoleInputFile))
            using (StreamReader srOut = new StreamReader(fileWithSolution))
            {
                Console.SetOut(sw);
                Console.SetIn(srIn);


                Program.Main(args);


                StringBuilder expected = new StringBuilder();
                string input;
                while ((input = srOut.ReadLine()) != null)
                {
                    expected.Append(input);
                    expected.Append(Environment.NewLine);
                }
                Assert.AreEqual<string>(expected.ToString(), sw.ToString());
            }
        }
        [TestMethod(), Timeout(5000)]
        public void MainTestFile07()
        {
            string[] args = new string[] { "testFiles/07/hradla.in" }; // file with network
            string consoleInputFile = "testFiles/07/std.in";
            string fileWithSolution = "testFiles/07/std.out";

            using (StringWriter sw = new StringWriter())
            using (StreamReader srIn = new StreamReader(consoleInputFile))
            using (StreamReader srOut = new StreamReader(fileWithSolution))
            {
                Console.SetOut(sw);
                Console.SetIn(srIn);


                Program.Main(args);


                StringBuilder expected = new StringBuilder();
                string input;
                while ((input = srOut.ReadLine()) != null)
                {
                    expected.Append(input);
                    expected.Append(Environment.NewLine);
                }
                Assert.AreEqual<string>(expected.ToString(), sw.ToString());
            }
        }
        [TestMethod(), Timeout(5000)]
        public void MainTestFile08()
        {
            string[] args = new string[] { "testFiles/08/hradla.in" }; // file with network
            string consoleInputFile = "testFiles/08/std.in";
            string fileWithSolution = "testFiles/08/std.out";

            using (StringWriter sw = new StringWriter())
            using (StreamReader srIn = new StreamReader(consoleInputFile))
            using (StreamReader srOut = new StreamReader(fileWithSolution))
            {
                Console.SetOut(sw);
                Console.SetIn(srIn);


                Program.Main(args);


                StringBuilder expected = new StringBuilder();
                string input;
                while ((input = srOut.ReadLine()) != null)
                {
                    expected.Append(input);
                    expected.Append(Environment.NewLine);
                }
                Assert.AreEqual<string>(expected.ToString(), sw.ToString());
            }
        }
        [TestMethod(), Timeout(5000)]
        public void MainTestFile09()
        {
            string[] args = new string[] { "testFiles/09/hradla.in" }; // file with network
            string consoleInputFile = "testFiles/09/std.in";
            string fileWithSolution = "testFiles/09/std.out";

            using (StringWriter sw = new StringWriter())
            using (StreamReader srIn = new StreamReader(consoleInputFile))
            using (StreamReader srOut = new StreamReader(fileWithSolution))
            {
                Console.SetOut(sw);
                Console.SetIn(srIn);


                Program.Main(args);


                StringBuilder expected = new StringBuilder();
                string input;
                while ((input = srOut.ReadLine()) != null)
                {
                    expected.Append(input);
                    expected.Append(Environment.NewLine);
                }
                Assert.AreEqual<string>(expected.ToString(), sw.ToString());
            }
        }
        [TestMethod(), Timeout(5000)]
        public void MainTestFile16()
        {
            string[] args = new string[] { "testFiles/16/hradla.in" }; // file with network
            string consoleInputFile = "testFiles/16/std.in";
            string fileWithSolution = "testFiles/16/std.out";

            using (StringWriter sw = new StringWriter())
            using (StreamReader srIn = new StreamReader(consoleInputFile))
            using (StreamReader srOut = new StreamReader(fileWithSolution))
            {
                Console.SetOut(sw);
                Console.SetIn(srIn);


                Program.Main(args);


                StringBuilder expected = new StringBuilder();
                string input;
                while ((input = srOut.ReadLine()) != null)
                {
                    expected.Append(input);
                    expected.Append(Environment.NewLine);
                }
                Assert.AreEqual<string>(expected.ToString(), sw.ToString());
            }
        }
        [TestMethod(), Timeout(5000)]
        public void MainTestFile17()
        {
            string[] args = new string[] { "testFiles/17/hradla.in" }; // file with network
            string consoleInputFile = "testFiles/17/std.in";
            string fileWithSolution = "testFiles/17/std.out";

            using (StringWriter sw = new StringWriter())
            using (StreamReader srIn = new StreamReader(consoleInputFile))
            using (StreamReader srOut = new StreamReader(fileWithSolution))
            {
                Console.SetOut(sw);
                Console.SetIn(srIn);


                Program.Main(args);


                StringBuilder expected = new StringBuilder();
                string input;
                while ((input = srOut.ReadLine()) != null)
                {
                    expected.Append(input);
                    expected.Append(Environment.NewLine);
                }
                Assert.AreEqual<string>(expected.ToString(), sw.ToString());
            }
        }//Line 10833:CHYBA
        [TestMethod(), Timeout(5000)]
        public void MainTestFile20()
        {
            string[] args = new string[] { "testFiles/20/hradla.in" }; // file with network
            string fileWithSolution = "testFiles/20/std.out";

            using (StringWriter sw = new StringWriter())
            using (StreamReader srOut = new StreamReader(fileWithSolution))
            {
                var srIn = new StringReader("");
                Console.SetOut(sw);
                Console.SetIn(srIn);


                Program.Main(args);


                StringBuilder expected = new StringBuilder();
                string input;
                while ((input = srOut.ReadLine()) != null)
                {
                    expected.Append(input);
                    expected.Append(Environment.NewLine);
                }
                Assert.AreEqual<string>(expected.ToString(), sw.ToString());
            }
        }//no input file


        [TestMethod(), Timeout(5000)]
        public void MainTestDuplicate1()//opakování některé kombinace vstupů v definici přechodové funkce u hradla
        {
            string[] args = new string[] { "testFiles/extra/hradlaDuplicate1.in" }; // file with network
            string consoleInputFile = "testFiles/extra/std.in";
            string expected = "Line 16: Duplicate.";

            using (StringWriter sw = new StringWriter())
            using (StreamReader srIn = new StreamReader(consoleInputFile))
            {
                Console.SetOut(sw);
                Console.SetIn(srIn);

                Program.Main(args);

                Assert.AreEqual<string>(expected, sw.ToString());
            }
        }
        [TestMethod(), Timeout(5000)]
        public void MainTestDuplicate2()//definice již definovaného typu hradla
        {
            string[] args = new string[] { "testFiles/extra/hradlaDuplicate2.in" }; // file with network
            string consoleInputFile = "testFiles/extra/std.in";
            string expected = "Line 18: Duplicate.";

            using (StringWriter sw = new StringWriter())
            using (StreamReader srIn = new StreamReader(consoleInputFile))
            {
                Console.SetOut(sw);
                Console.SetIn(srIn);

                Program.Main(args);

                Assert.AreEqual<string>(expected, sw.ToString());
            }
        }
        [TestMethod(), Timeout(5000)]
        public void MainTestDuplicate3()//definice již definovaného typu instance,
        {
            string[] args = new string[] { "testFiles/extra/hradlaDuplicate3.in" }; // file with network
            string consoleInputFile = "testFiles/extra/std.in";
            string expected = "Line 23: Duplicate.";

            using (StringWriter sw = new StringWriter())
            using (StreamReader srIn = new StreamReader(consoleInputFile))
            {
                Console.SetOut(sw);
                Console.SetIn(srIn);

                Program.Main(args);

                Assert.AreEqual<string>(expected, sw.ToString());
            }
        }
        [TestMethod(), Timeout(5000)]
        public void MainTestDuplicate4()//definice již definovaného jména vstupu hradlové sítě
        {
            string[] args = new string[] { "testFiles/extra/hradlaDuplicate4.in" }; // file with network
            string consoleInputFile = "testFiles/extra/std.in";
            string expected = "Line 2: Duplicate.";

            using (StringWriter sw = new StringWriter())
            using (StreamReader srIn = new StreamReader(consoleInputFile))
            {
                Console.SetOut(sw);
                Console.SetIn(srIn);

                Program.Main(args);

                Assert.AreEqual<string>(expected, sw.ToString());
            }
        }
        [TestMethod(), Timeout(5000)]
        public void MainTestDuplicate5()//definice již definovaného jména výstupu hradlové sítě
        {
            string[] args = new string[] { "testFiles/extra/hradlaDuplicate5.in" }; // file with network
            string consoleInputFile = "testFiles/extra/std.in";
            string expected = "Line 3: Duplicate.";

            using (StringWriter sw = new StringWriter())
            using (StreamReader srIn = new StreamReader(consoleInputFile))
            {
                Console.SetOut(sw);
                Console.SetIn(srIn);

                Program.Main(args);

                Assert.AreEqual<string>(expected, sw.ToString());
            }
        }
        [TestMethod(), Timeout(5000)]
        public void MainTestDuplicate6()//definice již definovaného definice propojení hradlové sítě
        {
            string[] args = new string[] { "testFiles/extra/hradlaDuplicate6.in" }; // file with network
            string consoleInputFile = "testFiles/extra/std.in";
            string expected = "Line 27: Duplicate.";

            using (StringWriter sw = new StringWriter())
            using (StreamReader srIn = new StreamReader(consoleInputFile))
            {
                Console.SetOut(sw);
                Console.SetIn(srIn);

                Program.Main(args);

                Assert.AreEqual<string>(expected, sw.ToString());
            }
        }


        [TestMethod(), Timeout(5000)]
        public void MainTestMissingKeyword1()//Pokud v programu chybí nějaká povinná část -> inputs
        {
            string[] args = new string[] { "testFiles/extra/hradlaMissK1.in" }; // file with network
            string consoleInputFile = "testFiles/extra/std.in";
            string expected = "Line 8: Missing keyword.";

            using (StringWriter sw = new StringWriter())
            using (StreamReader srIn = new StreamReader(consoleInputFile))
            {
                Console.SetOut(sw);
                Console.SetIn(srIn);

                Program.Main(args);

                Assert.AreEqual<string>(expected, sw.ToString());
            }
        }
        [TestMethod(), Timeout(5000)]
        public void MainTestMissingKeyword2()//Pokud v programu chybí nějaká povinná část -> outputs
        {
            string[] args = new string[] { "testFiles/extra/hradlaMissK2.in" }; // file with network
            string consoleInputFile = "testFiles/extra/std.in";
            string expected = "Line 8: Missing keyword.";

            using (StringWriter sw = new StringWriter())
            using (StreamReader srIn = new StreamReader(consoleInputFile))
            {
                Console.SetOut(sw);
                Console.SetIn(srIn);

                Program.Main(args);

                Assert.AreEqual<string>(expected, sw.ToString());
            }
        }


        [TestMethod(), Timeout(5000)]
        public void MainTestBindingRule1()//propojení vstupu jednoho hradla na vstup jiného
        {
            string[] args = new string[] { "testFiles/extra/hradlaBindR1.in" }; // file with network
            string consoleInputFile = "testFiles/extra/std.in";
            string expected = "Line 31: Binding rule.";

            using (StringWriter sw = new StringWriter())
            using (StreamReader srIn = new StreamReader(consoleInputFile))
            {
                Console.SetOut(sw);
                Console.SetIn(srIn);

                Program.Main(args);

                Assert.AreEqual<string>(expected, sw.ToString());
            }
        }
        [TestMethod(), Timeout(5000)]
        public void MainTestBindingRule2()//nepřipojený výstup
        {
            string[] args = new string[] { "testFiles/extra/hradlaBindR2.in" }; // file with network
            string consoleInputFile = "testFiles/extra/std.in";
            string expected = "Line 29: Binding rule.";

            using (StringWriter sw = new StringWriter())
            using (StreamReader srIn = new StreamReader(consoleInputFile))
            {
                Console.SetOut(sw);
                Console.SetIn(srIn);

                Program.Main(args);

                Assert.AreEqual<string>(expected, sw.ToString());
            }
        }


        [TestMethod(), Timeout(5000)]
        public void MainTestSyntaxError()//odkaz na neexistující výstup
        {
            string[] args = new string[] { "testFiles/extra/hradlaSyntaxError.in" }; // file with network
            string consoleInputFile = "testFiles/extra/std.in";
            string expected = "Line 28: Syntax error.";

            using (StringWriter sw = new StringWriter())
            using (StreamReader srIn = new StreamReader(consoleInputFile))
            {
                Console.SetOut(sw);
                Console.SetIn(srIn);

                Program.Main(args);

                Assert.AreEqual<string>(expected, sw.ToString());
            }
        }
    }
}