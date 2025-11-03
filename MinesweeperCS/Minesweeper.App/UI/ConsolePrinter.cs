using Minesweeper.Models;

namespace Minesweeper.UI;

// Draw
public static class ConsolePrinter
{
    public static void PrintVisible(BoardModel board, bool revealAll = false)
    {
        Console.Write("    ");
        for (int c = 0; c < board.Size; c++) Console.Write($"{c,2} ");
        Console.WriteLine();
        Console.WriteLine("   " + new string('-', board.Size * 3));

        for (int r = 0; r < board.Size; r++)
        {
            Console.Write($"{r,2}| ");
            for (int c = 0; c < board.Size; c++)
            {
                var cell = board.Grid[r, c];
                char ch;
                if (revealAll || cell.IsRevealed)
                {
                    if (cell.HasBomb) ch = '*';
                    else if (cell.HasReward) ch = 'r';
                    else ch = cell.BombsNearby == 0 ? '·' : cell.BombsNearby.ToString()[0];
                }
                else
                {
                    ch = cell.IsFlagged ? 'F' : '#';
                }
                Console.Write($"{ch}  ");
            }
            Console.WriteLine();
        }
    }
}
