// ============================================================
// sergio thomas pineda
//  CST-250
// ============================================================

using System;

namespace FloodFillRecursion
{
    internal class Program
    {
        static void Main(string[] args)
        {
            // '.' = open cell, '#' = wall
            char[,] grid = new char[,]
            {
                {'.','.','.','#','.','.','.','.','.','.'},
                {'.','#','.','#','.','#','#','#','.','.'},
                {'.','#','.','.','.','.','.','#','.','.'},
                {'.','#','#','#','#','.','.','#','.','.'},
                {'.','.','.','.','#','.','#','#','.','.'},
                {'.','#','#','.','#','.','.','.','.','.'},
                {'.','.','.','.','.','#','.','#','#','.'},
                {'.','#','#','#','.','.','.','.','#','.'},
                {'.','.','.','#','#','#','.','.','.','.'},
                {'.','.','.','.','.','.','.','#','.','.'}
            };

            bool[,] visited = new bool[grid.GetLength(0), grid.GetLength(1)];

            Console.Write("Enter start row (0-9): ");
            int sr = ReadInRange(0, 9);
            Console.Write("Enter start col (0-9): ");
            int sc = ReadInRange(0, 9);

            Console.WriteLine("\nBefore Flood Fill:");
            Print(grid, visited, showVisited: false);

            Flood.Fill(grid, visited, sr, sc, target: '.', mark: '~');

            Console.WriteLine("\nAfter Flood Fill:");
            Print(grid, visited, showVisited: true);

            Console.WriteLine("\nPress any key to exit...");
            Console.ReadKey();
        }

        // Input guard
        static int ReadInRange(int min, int max)
        {
            string s = Console.ReadLine();
            int v;
            while (!int.TryParse(s, out v) || v < min || v > max)
            {
                Console.WriteLine("Invalid number");
                Console.Write($"Enter a number {min}-{max}: ");
                s = Console.ReadLine();
            }
            return v;
        }

        // Draw board; optionally show visited with 'v'
        static void Print(char[,] grid, bool[,] visited, bool showVisited)
        {
            int rows = grid.GetLength(0), cols = grid.GetLength(1);
            Console.Write("   ");
            for (int c = 0; c < cols; c++) Console.Write($"{c,2}");
            Console.WriteLine();
            Console.WriteLine("   " + new string('-', cols * 2));

            for (int r = 0; r < rows; r++)
            {
                Console.Write($"{r,2}|");
                for (int c = 0; c < cols; c++)
                {
                    char ch = grid[r, c];
                    if (showVisited && visited[r, c] && ch == '~') ch = 'v';
                    Console.Write($" {ch}");
                }
                Console.WriteLine();
            }
        }
    }

    internal static class Flood
    {
        // Recursive flood fill: expands on target cells and marks them with 'mark'
        internal static void Fill(char[,] grid, bool[,] visited, int r, int c, char target, char mark)
        {
            int rows = grid.GetLength(0), cols = grid.GetLength(1);

            // bounds
            if (r < 0 || r >= rows || c < 0 || c >= cols) return;

            // already processed
            if (visited[r, c]) return;

            // only fill the target type (e.g., '.')
            if (grid[r, c] != target) return;

            // mark visited + convert to mark
            visited[r, c] = true;
            grid[r, c] = mark;

            // recurse to neighbors (8-directional or 4-directional; here use 4-dir)
            Fill(grid, visited, r - 1, c, target, mark); // up
            Fill(grid, visited, r + 1, c, target, mark); // down
            Fill(grid, visited, r, c - 1, target, mark); // left
            Fill(grid, visited, r, c + 1, target, mark); // right
        }
    }
}
