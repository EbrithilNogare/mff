using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace excel
{
    class Program
    {
        static void Main(string[] args)
        {
            if (!correctArguments(args))
            {
                Console.WriteLine("Argument Error");
                Console.ReadKey();
                return;
            }

            try
            {
                using (var sr = new StreamReader(args[0]))
                using (var sw = new StreamWriter(args[1]))
                {


                    var excel = new Excel();
                    excel.createTable(sr);
                    excel.computeTable();
                    excel.returnTable(sw);


                }
            }
            catch (IOException)
            {
                Console.WriteLine("File Error");
                Console.ReadKey();
                return;
            }
        }

        /// <summary>
        /// return true if arguments are correct
        /// </summary>
        /// <param name="args"></param>
        /// <returns></returns>
        static bool correctArguments(string[] args)
        {
            return (args.Length == 2);
        }
    }

    public class Excel
    {
        public List<List<string>> table;
        public Excel()
        {
            this.table = new List<List<string>>();
            return;
        }

        /// <summary>
        /// read from stream and creates table
        /// </summary>
        /// <param name="sr"></param>
        public void createTable(StreamReader sr)
        { 
            char[] charSeparators = new char[] { ' ' };
            while (sr.Peek() != -1)
            {
                string[] line = sr.ReadLine().Split(charSeparators, StringSplitOptions.RemoveEmptyEntries);
                var row = new List<string>();
                foreach (string cell in line)
                {
                    row.Add(cell);
                }
                table.Add(row);
            }
            return;
        }

        /// <summary>
        /// take already generated table and compute it
        /// </summary>
        public void computeTable()
        {
            for (int i = 0; i < table.Count; i++)
            {
                for (int j = 0; j < table[i].Count; j++)
                {
                    computeCell(i, j, new List<int[]>());
                }
            }
        }

        /// <summary>
        /// take separate cell and solve its formula or leave default constant
        /// </summary>
        /// <param name="i"></param>
        /// <param name="j"></param>
        /// <param name="cycleControl"></param>
        private void computeCell(int i, int j, List<int[]> cycleControl)
        {
            //i, j out of range check
            if (!checkRange(i, j, table))
            {
                return;
            }

            //cycle detecting
            if (checkCycle(i, j, cycleControl))
            {
                return;
            }

            string cell = table[i][j];

            //default values check
            if (checkConstants(cell))
            {
                return;
            }

            //cell with formula
            if (cell[0] == '=') 
            {
                string formula = cell.Substring(1);
                cycleControl.Add(new int[] { i, j });
                if(i==0&&j==1)
                    Console.WriteLine("a");
                table[i][j] = parseFormula(formula, cycleControl);
                cycleControl.RemoveAt(cycleControl.Count - 1);
                return;
            }

            //cell with number
            if (int.TryParse(cell, out int bin))
            {
                return;
            }

            table[i][j] = "#INVVAL";

        }

        /// <summary>
        /// check if cell contain one of the constant names (like #ERROR)
        /// </summary>
        /// <param name="cell"></param>
        /// <returns>true if there is constant</returns>
        private bool checkConstants(string cell)
        {
            return (cell == "#ERROR" || cell == "#DIV0" || cell == "#CYCLE" || cell == "#MISSOP" || cell == "#FORMULA" || cell == "#INVVAL" || cell == "[]") ;
        }

        /// <summary>
        /// check for cycle and resolve if there is
        /// </summary>
        /// <param name="i"></param>
        /// <param name="j"></param>
        /// <param name="cycleControl"></param>
        /// <returns>true if there is cycle</returns>
        private bool checkCycle(int i, int j, List<int[]> cycleControl)
        {
            bool cycle = false;
            foreach (int[] item in cycleControl)
            {
                if ((item[0] == i && item[1] == j) || cycle)
                {
                    table[item[0]][item[1]] = "#CYCLE";
                    cycle = true;
                }
            }
            return cycle;
        }

        /// <summary>
        /// check if cell coordinates are range
        /// </summary>
        /// <param name="i"></param>
        /// <param name="j"></param>
        /// <param name="table"></param>
        /// <returns>true if coordinates link to valid cell</returns>
        private bool checkRange(int i, int j, List<List<string>> table)
        {
            return !(i >= table.Count || i < 0 || j >= table[i].Count || j < 0);
        }

        /// <summary>
        /// takes formula and apply some magic
        /// </summary>
        /// <param name="formula"></param>
        /// <param name="cycleControl"></param>
        /// <returns>returns constant error or solved formula</returns>
        private string parseFormula(string formula, List<int[]> cycleControl)
        {
            string[] cuttedFormula;
            char action;
            char[] actionMarks = { '+', '-', '*', '/' };
            int a = 0,b = 0;

            int actionMarkPosition = formula.IndexOfAny(actionMarks);

            if (actionMarkPosition == -1)
            {
                return "#MISSOP";
            }
            else
            {
                action = formula[actionMarkPosition];
            }

            cuttedFormula = formula.Split(actionMarks);

            if (cuttedFormula.Length != 2)
            {
                return "#FORMULA";
            }


            if (!isCellName(cuttedFormula[0]) || !isCellName(cuttedFormula[1]))
            {
                return "#FORMULA";
            }
            
            getCellname(cuttedFormula[0], out int firstRow, out int firstColumn);
            getCellname(cuttedFormula[1], out int secondRow, out int secondColumn);

            computeCell(firstRow, firstColumn, cycleControl);
            computeCell(secondRow, secondColumn, cycleControl);

            //check self for cycle
            if (table[cycleControl[cycleControl.Count-1][0]][cycleControl[cycleControl.Count - 1][1]] == "#CYCLE")
                return "#CYCLE";

            
            //check for number
            if (!checkNumber(firstRow, firstColumn, secondRow, secondColumn, out a, out b))
            {
                return "#ERROR";
            }
            if (a < 0 || b < 0)
            {
                return "#INVVAL";

            }
            
            return computeFormula(action, a, b);
        }

        /// <summary>
        /// takes cell names and return its values
        /// </summary>
        /// <param name="firstRow"></param>
        /// <param name="firstColumn"></param>
        /// <param name="secondRow"></param>
        /// <param name="secondColumn"></param>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <returns>true if there are two numbers</returns>
        private bool checkNumber(int firstRow, int firstColumn, int secondRow, int secondColumn, out int a, out int b)
        {
            bool succes = true;
            if (!checkRange(firstRow, firstColumn, table))
            {
                a = 0;
            }
            else
            if (!(int.TryParse(table[firstRow][firstColumn], out a) || table[firstRow][firstColumn] == "[]"))
            {
                succes = false;
            }

            if (!checkRange(secondRow, secondColumn, table))
            {
                b = 0;
            }
            else
            if (!(int.TryParse(table[secondRow][secondColumn], out b) || table[secondRow][secondColumn] == "[]"))
            {
                succes = false;
            }

            return succes;
        }

        /// <summary>
        /// do binary operation
        /// </summary>
        /// <param name="action"></param>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <returns></returns>
        public string computeFormula(char action, int a, int b)
        {
            int result = 0;
            switch (action)
            {
                case '+':
                    result = (a + b);
                    break;
                case '-':
                    result = (a - b);
                    break;
                case '*':
                    result = (a * b);
                    break;
                case '/':
                    if (b == 0)
                        return "#DIV0";
                    else
                        result = (a/b);
                    break;
                default:
                    return "#ERROR";
            }
            return result.ToString();
        }

        /// <summary>
        /// parse cell name from string
        /// </summary>
        /// <param name="name"></param>
        /// <param name="row"></param>
        /// <param name="column"></param>
        public void getCellname(string name, out int row, out int column)
        {
            bool number = false;
            string rowString = "";
            string columnString = "";

            foreach (char c in name)
            {
                if (number)
                {
                    rowString += c;
                }
                else
                {
                        if (c > 47 && c < 58) //number
                    {
                        rowString += c;
                        number = true;
                        }
                        else
                        {
                            columnString += c;
                        }
                }
            }
            row = int.Parse(rowString)-1;
            column = AlphabetToInt(columnString);
        }

        /// <summary>
        /// transfer letters to numbers
        /// </summary>
        /// <param name="rowString"></param>
        /// <returns></returns>
        public int AlphabetToInt(string rowString)
        {
            int output = 0;
            char c;
            for (int i = 0; i < rowString.Length; i++)
            {
                c = rowString[rowString.Length-i-1];
                output += (c - 64)* (int)Math.Pow(26, i); //math.pow is faster than array of precalculated values, interesting (about three time faster)
            }
            return output-1;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="cell"></param>
        /// <returns>true if it's cell name</returns>
        public bool isCellName(string cell)
        {
            bool number = false;
            for (int i = 0; i < cell.Length; i++)
            {
                char c = (char)cell[i];
                if (number)
                {

                    if (!(c > 47 && c < 58)) //number
                    {
                        return false;
                    }
                }
                else
                {
                    if (!(c > 64 && c < 91)) //char
                        if (c > 47 && c < 58 && i != 0) //number
                        {
                            number = true;
                        }
                        else
                        {
                            return false;
                        }
                }
            }
            return number;
        }

        /// <summary>
        /// send table to stream
        /// </summary>
        /// <param name="sw"></param>
        public void returnTable(StreamWriter sw)
        {
            bool firstLine = true;
            foreach (List<string> column in table)
            {
                if (!firstLine)
                    sw.WriteLine();
                else
                    firstLine = false;
                for (int i = 0; i < column.Count; i++)
                {
                    if(i!=0)
                        sw.Write(" ");
                    sw.Write(column[i]);
                }
            }
        }
    }
}
