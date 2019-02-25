using System;
namespace dvojcisli
{
    class Program
    {
        static void Main(
            string[]
            args)
        {
            string a = Console.ReadLine()
                ;
            string b = Console.ReadLine()
                ;
            int vysledek = 1;
            if (
                a.Length < 2)
                a = "00" + a;
            if (
                b.Length < 1)
                b = "0" + b;
            int aa = int.Parse(
                a.Substring(
                    a.Length - 2, 2))
                    ;
            int mocnina = aa;

            for (
                int i = b.Length - 1; i >= 0; i--)
            {
                if (
                    b[
                        i]
                        == '1')
                {
                    vysledek = (
                        vysledek * mocnina)
                        % 100;
                }
                mocnina = (
                    mocnina * mocnina)
                    % 100;
            }
            if (
                vysledek < 10)
                Console.Write(
                    0)
                    ;
            Console.Write(
                Math.Abs(
                    vysledek))
                    ;
        }
    }
}