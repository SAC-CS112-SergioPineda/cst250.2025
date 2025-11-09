/*
sergio thomas pineda 
cst250
*/

//main method


using System;

namespace CountToOneRecursion
{
    internal class Program
    {
        // ================================
        // Start of the Main Method
        // ================================
        static void Main(string[] args)
        {
            // Declare and initialize
            int choice = 0, result = 0;
            string input = "";

            // Prompt the user for a number
            Console.Write("Enter a positive number: ");
            input = Console.ReadLine();

            // Validate user input
            while (!int.TryParse(input, out choice) || choice <= 0)
            {
                Console.WriteLine("Invalid number");
                Console.Write("Enter a positive number: ");
                input = Console.ReadLine();
            }

            // Call the CountToOne function
            result = Utility.CountToOne(choice);

            // Display the final result
            Console.WriteLine($"\nThe end number is {result}");

            Console.WriteLine("\nPress any key to exit...");
            Console.ReadKey();
        }
        // ================================
        // End of the Main Method
        // ================================
    }

    // ============================================================
    // Utility class that contains recursive logic for CountToOne
    // ============================================================
    internal static class Utility
    {
        /// <summary>
        /// Recursively reduces a positive number down to one.
        /// </summary>
        /// <param name="num">The number to reduce.</param>
        /// <returns>The final number after recursion (1).</returns>
        internal static int CountToOne(int num)
        {
            Console.WriteLine($"Current number: {num}");

            // Base Case: stop recursion when number is 1
            if (num == 1)
            {
                Console.WriteLine("Reached base case (1)");
                return 1;
            }

            // Recursive Case: even → divide by 2; odd → add 1
            if (num % 2 == 0)
            {
                Console.WriteLine($"{num} is even; dividing by 2");
                return CountToOne(num / 2);
            }
            else
            {
                Console.WriteLine($"{num} is odd; adding 1 to make it even");
                return CountToOne(num + 1);
            }
        }
    }
}
