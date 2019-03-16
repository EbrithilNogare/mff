#define STUDENT_VERSION
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Cuni.Arithmetics.FixedPoint;

namespace FixedPointApiTest
{
#if STUDENT_VERSION
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Q24_8:");
            {
                var f1 = new Fixed<Q24_8>(3);
                Console.WriteLine($"3: {f1}");

                var f2 = new Fixed<Q24_8>(2);
                var f3 = f1.Add(f2);
                Console.WriteLine($"3 + 2: {f3}");

                f3 = f1.Multiply(f2);
                Console.WriteLine($"3 * 2: {f3}");

                f1 = new Fixed<Q24_8>(19);
                f2 = new Fixed<Q24_8>(13);
                f3 = f1.Multiply(f2);
                Console.WriteLine($"19 * 13: {f3}");

                f1 = new Fixed<Q24_8>(3);
                f2 = new Fixed<Q24_8>(2);
                f3 = f1.Divide(f2);
                Console.WriteLine($"3 / 2: {f3}");

                f1 = new Fixed<Q24_8>(248);
                f2 = new Fixed<Q24_8>(10);
                f3 = f1.Divide(f2);
                Console.WriteLine($"248 / 10: {f3}");

                f1 = new Fixed<Q24_8>(625);
                f2 = new Fixed<Q24_8>(1000);
                f3 = f1.Divide(f2);
                Console.WriteLine($"625 / 1000: {f3}");
            }

            //

            Console.WriteLine();
            Console.WriteLine("Q16_16:");
            {
                var f1 = new Fixed<Q16_16>(3);
                Console.WriteLine($"3: {f1}");

                var f2 = new Fixed<Q16_16>(2);
                var f3 = f1.Add(f2);
                Console.WriteLine($"3 + 2: {f3}");

                f3 = f1.Multiply(f2);
                Console.WriteLine($"3 * 2: {f3}");

                f1 = new Fixed<Q16_16>(19);
                f2 = new Fixed<Q16_16>(13);
                f3 = f1.Multiply(f2);
                Console.WriteLine($"19 * 13: {f3}");

                f1 = new Fixed<Q16_16>(248);
                f2 = new Fixed<Q16_16>(10);
                f3 = f1.Divide(f2);
                Console.WriteLine($"248 / 10: {f3}");

                f1 = new Fixed<Q16_16>(625);
                f2 = new Fixed<Q16_16>(1000);
                f3 = f1.Divide(f2);
                Console.WriteLine($"625 / 1000: {f3}");
            }

            //

            Console.WriteLine();
            Console.WriteLine("Q8_24:");
            {
                var f1 = new Fixed<Q8_24>(3);
                var f2 = new Fixed<Q8_24>(2);
                var f3 = f1.Add(f2);
                Console.WriteLine($"3 + 2: {f3}");

                f3 = f1.Multiply(f2);
                Console.WriteLine($"3 * 2: {f3}");

                f1 = new Fixed<Q8_24>(19);
                f2 = new Fixed<Q8_24>(13);
                f3 = f1.Multiply(f2);
                Console.WriteLine($"19 * 13: {f3}");

                f1 = new Fixed<Q8_24>(248);
                f2 = new Fixed<Q8_24>(10);
                f3 = f1.Divide(f2);
                Console.WriteLine($"248 / 10: {f3}");

                f1 = new Fixed<Q8_24>(625);
                f2 = new Fixed<Q8_24>(1000);
                f3 = f1.Divide(f2);
                Console.WriteLine($"625 / 1000: {f3}");
            }
        }
    }
#endif
}
