using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace deleni
{
    class Program
    {
        static void Main(string[] args)
        {
            string[] tempStr = Console.ReadLine().Split(' ');
            double a, b, c, d;
            string v;
            List<double> seznamA = new List<double>();
            List<double> seznamB = new List<double>();


            a = double.Parse(tempStr[0]);
            b = double.Parse(tempStr[1]);
            c = Math.Floor(a / b);
            v = c.ToString()+".";
            d = a % b;

            while (d != 0)
            {
                a = a - c*b;
                a *= 10;
                c = Math.Floor(a / b);
                d = a % b;


                for (int i = 0; i < seznamA.Count; i++)
                {
                    if (seznamA[i] == a && seznamB[i] == b)
                    {


                        Console.Write(v.Split('.')[0]);
                        if(v.Split('.').Length == 1)
                        {
                            return;
                        }
                        v = v.Split('.')[1];
                        Console.Write(".");
                        Console.Write(v.Substring(0,i));
                        v=v.Remove(0, i);
                        Console.Write("(");
                        Console.Write(v);
                        Console.Write(")");
                        Console.ReadKey();
                        return;
                    }
                }



                v += c.ToString();


                seznamA.Add(a);
                seznamB.Add(b);





                //Console.WriteLine("a " + a + "\n" + "c " + c + "\n" + "d " + d + "\n" + "v " + v);

            }
            
            if(double.Parse(v.ToString())%1!=0)
                Console.Write(v.Substring(0, v.Length));
            else
                Console.Write(v.Substring(0, v.Length-1));


            Console.ReadKey();
            
        }
    }
}
