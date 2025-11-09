// ============================================================
// sergio thomas pineda
// CST-250
// Description: Compute the GCD of two positive integers using recursion.
// ============================================================

using System;

namespace GcdRecursion
{
    internal class Program
    {
        static void Main(string[] args)
        {
            int a = 0, b = 0;
            string s1 = "", s2 = "";

            Console.Write("Enter first positive integer: ");
            s1 = Console.ReadLine();
            while (!int.TryParse(s1, out a) || a <= 0)
            {
                Console.WriteLine("Invalid number");
                Console.Write("Enter first positive integer: ");
                s1 = Console.ReadLine();
            }

            Console.Write("Enter second positive integer: ");
            s2 = Console.ReadLine();
            while (!int.TryParse(s2, out b) || b <= 0)
            {
                Console.WriteLine("Invalid number");
                Console.Write("Enter second positive integer: ");
                s2 = Console.ReadLine();
            }

            int g = NumberUtil.Gcd(a, b);
            Console.WriteLine($"\nGCD({a}, {b}) = {g}");
            Console.WriteLine("\nPress any key to exit...");
            Console.ReadKey();
        }
    }

    internal static class NumberUtil
    {
        // Euclid's algorithm (recursive)
        internal static int Gcd(int a, int b)
        {
            if (b == 0) return a;       // base case
            return Gcd(b, a % b);       // recurse with remainder
        }
    }
}
