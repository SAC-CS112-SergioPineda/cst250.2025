// ============================================================
// Title:sergio thomas pineda
// CST-250
// ============================================================

using System;

namespace FactorialRecursion
{
    internal class Program
    {
        static void Main(string[] args)
        {
            int n = 0;
            string input = "";

            Console.Write("Enter a non-negative integer: ");
            input = Console.ReadLine();

            while (!int.TryParse(input, out n) || n < 0)
            {
                Console.WriteLine("Invalid number");
                Console.Write("Enter a non-negative integer: ");
                input = Console.ReadLine();
            }

            long ans = MathUtil.Factorial(n);
            Console.WriteLine($"\n{n}! = {ans}");
            Console.WriteLine("\nPress any key to exit...");
            Console.ReadKey();
        }
    }

    internal static class MathUtil
    {
        // Recursive factorial
        internal static long Factorial(int n)
        {
            if (n == 0 || n == 1)  // base case
                return 1;
            return n * Factorial(n - 1);
        }
    }
}
