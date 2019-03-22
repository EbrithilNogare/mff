using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Cuni.Arithmetics.FixedPoint;

using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Running;

namespace Benchmarky
{
    class Benchmarks
    {
         static void Main(string[] args)
        {
            List<BenchmarkDotNet.Reports.Summary> reports = new List<BenchmarkDotNet.Reports.Summary>();
            reports.Add(BenchmarkRunner.Run<AddTestsWithoutDeclarationForCycle>());
            //reports.Add(BenchmarkRunner.Run<AddTestsWithoutDeclaration>());
            //reports.Add(BenchmarkRunner.Run<AddTests>());
            //reports.Add(BenchmarkRunner.Run<SubtractTests>());
            //reports.Add(BenchmarkRunner.Run<MultiplyTests>());
            //reports.Add(BenchmarkRunner.Run<DivideTests>());
            //reports.Add(BenchmarkRunner.Run<GauseEliminationTests>());

            //Console.Clear();

            foreach (var report in reports)
            {
                Console.WriteLine(report.Title);
                foreach (var row in report.Table.FullContent)
                    Console.WriteLine(String.Format("    {0, -20}:  {1}", row[0], row[41]));                
            }
            
        }
    }


    public class AddTestsWithoutDeclarationForCycle
    {
        Fixed<Q24_8> f1 = new Fixed<Q24_8>(512);
        Fixed<Q24_8> f2 = new Fixed<Q24_8>(42);
        Fixed<Q24_8> f3;
        Fixed<Q16_16> f4 = new Fixed<Q16_16>(512);
        Fixed<Q16_16> f5 = new Fixed<Q16_16>(42);
        Fixed<Q16_16> f6;
        Fixed<Q8_24> f7 = new Fixed<Q8_24>(512);
        Fixed<Q8_24> f8 = new Fixed<Q8_24>(42);
        Fixed<Q8_24> f9;
        float f10 = 512;
        float f11 = 42;
        float f12 = 512;
        double f13 = 42;
        double f14 = 512;
        double f15 = 42;



        [Benchmark]
        public void Q24_8Test()
        {
            for (int i = 0; i < 10000; i++)
            {
                f3 = f1.Add(f2);
            }
        }
        [Benchmark]
        public void Q16_16Test()
        {
            for (int i = 0; i < 10000; i++)
            {
                f6 = f4.Add(f5);
            }
        }
        [Benchmark]
        public void Q8_24Test()
        {
            for (int i = 0; i < 10000; i++)
            {
                f9 = f7.Add(f8);
            }
        }
        [Benchmark]
        public void FloatTest()
        {
            for (int i = 0; i < 10000; i++)
            {
                f12 = f10 + f11;
            }
        }
        [Benchmark]
        public void DoubleTest()
        {
            for (int i = 0; i < 10000; i++)
            {
                f15 = f13 + f14;
            }
        }
    }
    public class AddTestsWithoutDeclaration
    {
        Fixed<Q24_8> f1 = new Fixed<Q24_8>(512);
        Fixed<Q24_8> f2 = new Fixed<Q24_8>(42);
        Fixed<Q24_8> f3;
        Fixed<Q16_16> f4 = new Fixed<Q16_16>(512);
        Fixed<Q16_16> f5 = new Fixed<Q16_16>(42);
        Fixed<Q16_16> f6;
        Fixed<Q8_24> f7 = new Fixed<Q8_24>(512);
        Fixed<Q8_24> f8 = new Fixed<Q8_24>(42);
        Fixed<Q8_24> f9;
        float f10 = 512;
        float f11 = 42;
        float f12 = 512;
        double f13 = 42;
        double f14 = 512;
        double f15 = 42;



        [Benchmark]
        public void Q24_8Test()
        {
            f3 = f1.Add(f2);
        }
        [Benchmark]
        public void Q16_16Test()
        {
            f6 = f4.Add(f5);
        }
        [Benchmark]
        public void Q8_24Test()
        {
            f9 = f7.Add(f8);
        }
        [Benchmark]
        public void FloatTest()
        {
            f12 = f10 + f11;
        }
        [Benchmark]
        public void DoubleTest()
        {
            f15 = f13 + f14;
        }
    }
    public class AddTests
    {
        int a = 512;
        int b = 42;

        [Benchmark]
        public void Q24_8Test()
        {
            var f1 = new Fixed<Q24_8>(a);
            var f2 = new Fixed<Q24_8>(b);
            var f3 = f1.Add(f2);
        }
        [Benchmark]
        public void Q16_16Test()
        {
            var f1 = new Fixed<Q16_16>(a);
            var f2 = new Fixed<Q16_16>(b);
            var f3 = f1.Add(f2);
        }
        [Benchmark]
        public void Q8_24Test()
        {
            var f1 = new Fixed<Q8_24>(a);
            var f2 = new Fixed<Q8_24>(b);
            var f3 = f1.Add(f2);
        }
        [Benchmark]
        public void FloatTest()
        {
            var f1 = (float)a;
            var f2 = (float)b;
            var f3 = f1 + f2;
        }
        [Benchmark]
        public void DoubleTest()
        {
            var f1 = (double)a;
            var f2 = (double)b;
            var f3 = f1 + f2;
        }
    }
    public class SubtractTests
    {
        int a = 512;
        int b = 42;

        [Benchmark]
        public void Q24_8Test()
        {
            var f1 = new Fixed<Q24_8>(a);
            var f2 = new Fixed<Q24_8>(b);
            var f3 = f1.Subtract(f2);
        }
        [Benchmark]
        public void Q16_16Test()
        {
            var f1 = new Fixed<Q16_16>(a);
            var f2 = new Fixed<Q16_16>(b);
            var f3 = f1.Subtract(f2);
        }
        [Benchmark]
        public void Q8_24Test()
        {
            var f1 = new Fixed<Q8_24>(a);
            var f2 = new Fixed<Q8_24>(b);
            var f3 = f1.Subtract(f2);
        }
        [Benchmark]
        public void FloatTest()
        {
            var f1 = (float)a;
            var f2 = (float)b;
            var f3 = f1 - f2;
        }
        [Benchmark]
        public void DoubleTest()
        {
            var f1 = (double)a;
            var f2 = (double)b;
            var f3 = f1 - f2;
        }
    }
    public class MultiplyTests
    {
        int a = 512;
        int b = 42;

        [Benchmark]
        public void Q24_8Test()
        {
            var f1 = new Fixed<Q24_8>(a);
            var f2 = new Fixed<Q24_8>(b);
            var f3 = f1.Multiply(f2);
        }
        [Benchmark]
        public void Q16_16Test()
        {
            var f1 = new Fixed<Q16_16>(a);
            var f2 = new Fixed<Q16_16>(b);
            var f3 = f1.Multiply(f2);
        }
        [Benchmark]
        public void Q8_24Test()
        {
            var f1 = new Fixed<Q8_24>(a);
            var f2 = new Fixed<Q8_24>(b);
            var f3 = f1.Multiply(f2);
        }
        [Benchmark]
        public void FloatTest()
        {
            var f1 = (float)a;
            var f2 = (float)b;
            var f3 = f1 * f2;
        }
        [Benchmark]
        public void DoubleTest()
        {
            var f1 = (double)a;
            var f2 = (double)b;
            var f3 = f1 * f2;
        }
    }
    public class DivideTests
    {
        int a = 512;
        int b = 42;

        [Benchmark]
        public void Q24_8Test()
        {
            var f1 = new Fixed<Q24_8>(a);
            var f2 = new Fixed<Q24_8>(b);
            var f3 = f1.Divide(f2);
        }
        [Benchmark]
        public void Q16_16Test()
        {
            var f1 = new Fixed<Q16_16>(a);
            var f2 = new Fixed<Q16_16>(b);
            var f3 = f1.Divide(f2);
        }
        [Benchmark]
        public void Q8_24Test()
        {
            var f1 = new Fixed<Q8_24>(a);
            var f2 = new Fixed<Q8_24>(b);
            var f3 = f1.Divide(f2);
        }
        [Benchmark]
        public void FloatTest()
        {
            var f1 = (float)a;
            var f2 = (float)b;
            var f3 = f1 / f2;
        }
        [Benchmark]
        public void DoubleTest()
        {
            var f1 = (double)a;
            var f2 = (double)b;
            var f3 = f1 / f2;
        }
    }
    public class GauseEliminationTests
    {
        [Benchmark]
        public void Q24_8Test()
        {
            var input = new Fixed<Q24_8>[,] {
                {552,70,741,530,313,55,140,64,314,113},
                {55,518,575,229,114,127,283,369,1005,260},
                {65,871,242,526,673,235,971,161,214,507},
                {021,753,774,172,221,757,216,414,419,823},
                {66,328,833,372,23,265,976,656,292,65},
                {21,442,1003,473,614,354,977,471,122,662},
                {08,97,849,813,942,472,115,61,480,132},
                {5,134,261,253,263,783,566,215,385,516},
                {024,19,517,837,773,435,519,377,219,499},
                };
            var gause = new GauseElimination<Fixed<Q24_8>, Q24_8>();
            bool succeded = gause.SolveLinearEquations(input);
        }
        [Benchmark]
        public void Q16_16Test()
        {
            var input = new Fixed<Q16_16>[,] {
                {552,70,741,530,313,55,140,64,314,113},
                {55,518,575,229,114,127,283,369,1005,260},
                {65,871,242,526,673,235,971,161,214,507},
                {021,753,774,172,221,757,216,414,419,823},
                {66,328,833,372,23,265,976,656,292,65},
                {21,442,1003,473,614,354,977,471,122,662},
                {08,97,849,813,942,472,115,61,480,132},
                {5,134,261,253,263,783,566,215,385,516},
                {024,19,517,837,773,435,519,377,219,499},
                };
            var gause = new GauseElimination<Fixed<Q16_16>, Q16_16>();
            bool succeded = gause.SolveLinearEquations(input);
        }
        [Benchmark]
        public void Q8_24Test()
        {
            var input = new Fixed<Q8_24>[,] {
                {552,70,741,530,313,55,140,64,314,113},
                {55,518,575,229,114,127,283,369,1005,260},
                {65,871,242,526,673,235,971,161,214,507},
                {021,753,774,172,221,757,216,414,419,823},
                {66,328,833,372,23,265,976,656,292,65},
                {21,442,1003,473,614,354,977,471,122,662},
                {08,97,849,813,942,472,115,61,480,132},
                {5,134,261,253,263,783,566,215,385,516},
                {024,19,517,837,773,435,519,377,219,499},
                };
            var gause = new GauseElimination<Fixed<Q8_24>, Q8_24>();
            bool succeded = gause.SolveLinearEquations(input);
        }
        [Benchmark]
        public void DoubleTest()
        {
            var input = new double[,] {
                {552,70,741,530,313,55,140,64,314,113},
                {55,518,575,229,114,127,283,369,1005,260},
                {65,871,242,526,673,235,971,161,214,507},
                {021,753,774,172,221,757,216,414,419,823},
                {66,328,833,372,23,265,976,656,292,65},
                {21,442,1003,473,614,354,977,471,122,662},
                {08,97,849,813,942,472,115,61,480,132},
                {5,134,261,253,263,783,566,215,385,516},
                {024,19,517,837,773,435,519,377,219,499},
                };
            var gause = new GauseElimination();
            bool succeded = gause.SolveLinearEquations(input);
        }
    }

    public class GauseElimination
    {
        public bool SolveLinearEquations(double[,] M)
        {
            // input checks
            int rowCount = M.GetUpperBound(0) + 1;
            if (M == null || M.Length != rowCount * (rowCount + 1))
                throw new ArgumentException("The algorithm must be provided with a (n x n+1) matrix.");
            if (rowCount < 1)
                throw new ArgumentException("The matrix must at least have one row.");

            // pivoting
            for (int col = 0; col + 1 < rowCount; col++) if (M[col, col].Equals(0))
                // check for zero coefficients
                {
                    // find non-zero coefficient
                    int swapRow = col + 1;
                    for (; swapRow < rowCount; swapRow++) if (! M[swapRow, col].Equals(0)) break;

                    if (! M[swapRow, col].Equals(0)) // found a non-zero coefficient?
                    {
                        // yes, then swap it with the above
                        double[] tmp = new double[rowCount + 1];
                        for (int i = 0; i < rowCount + 1; i++)
                        { tmp[i] = M[swapRow, i]; M[swapRow, i] = M[col, i]; M[col, i] = tmp[i]; }
                    }
                    else return false; // no, then the matrix has no unique solution
                }

            // elimination
            for (int sourceRow = 0; sourceRow + 1 < rowCount; sourceRow++)
            {
                for (int destRow = sourceRow + 1; destRow < rowCount; destRow++)
                {
                    double df = M[sourceRow, sourceRow];
                    double sf = M[destRow, sourceRow];
                    for (int i = 0; i < rowCount + 1; i++)
                    {
                        double a = M[destRow, i] * df;
                        var b = M[sourceRow, i] * sf;
                        M[destRow, i] = (double)(a - b);
                    }
                }
            }

            // back-insertion
            for (int row = rowCount - 1; row >= 0; row--)
            {
                double f = M[row, row];
                if (f.Equals(0)) return false;

                for (int i = 0; i < rowCount + 1; i++) M[row, i] = (M[row, i] / f);
                for (int destRow = 0; destRow < row; destRow++)
                {
                    M[destRow, rowCount] = (M[destRow, rowCount] - M[destRow, row] * M[row, rowCount]);
                    M[destRow, row] = 0;
                }
            }
            return true;
        }
    }
    public class GauseElimination<T, U> where T : Fixed<U>, new() where U : Q, new()
    {
        public bool SolveLinearEquations(T[,] M)
        {
            // input checks
            int rowCount = M.GetUpperBound(0) + 1;
            if (M == null || M.Length != rowCount * (rowCount + 1))
                throw new ArgumentException("The algorithm must be provided with a (n x n+1) matrix.");
            if (rowCount < 1)
                throw new ArgumentException("The matrix must at least have one row.");

            // pivoting
            for (int col = 0; col + 1 < rowCount; col++) if (M[col, col] == 0)
                // check for zero coefficients
                {
                    // find non-zero coefficient
                    int swapRow = col + 1;
                    for (; swapRow < rowCount; swapRow++) if (M[swapRow, col] != 0) break;

                    if (M[swapRow, col] != 0) // found a non-zero coefficient?
                    {
                        // yes, then swap it with the above
                        T[] tmp = new T[rowCount + 1];
                        for (int i = 0; i < rowCount + 1; i++)
                        { tmp[i] = M[swapRow, i]; M[swapRow, i] = M[col, i]; M[col, i] = tmp[i]; }
                    }
                    else return false; // no, then the matrix has no unique solution
                }

            // elimination
            for (int sourceRow = 0; sourceRow + 1 < rowCount; sourceRow++)
            {
                for (int destRow = sourceRow + 1; destRow < rowCount; destRow++)
                {
                    T df = M[sourceRow, sourceRow];
                    T sf = M[destRow, sourceRow];
                    for (int i = 0; i < rowCount + 1; i++)
                    {
                        var a = M[destRow, i] * df;
                        var b = M[sourceRow, i] * sf;
                        M[destRow, i] = (T)(a - b);
                    }
                }
            }

            // back-insertion
            for (int row = rowCount - 1; row >= 0; row--)
            {
                T f = M[row, row];
                if (f == 0) return false;

                for (int i = 0; i < rowCount + 1; i++) M[row, i] = (T)(M[row, i] / f);
                for (int destRow = 0; destRow < row; destRow++)
                {
                    M[destRow, rowCount] = (T)(M[destRow, rowCount] - M[destRow, row] * M[row, rowCount]);
                    M[destRow, row] = (T)0;
                }
            }
            return true;
        }
    }
}

