using Minesweeper.Models;
using Minesweeper.Services;
using Minesweeper.UI;

// start
Console.Clear();
Console.WriteLine("=== Minesweeper (C# Console) ===");
int size = ReadInt("Board size (5-30)", 10, 5, 30);

// pick
Console.WriteLine("\nSelect difficulty:");
Console.WriteLine("  1) Easy (~10%)");
Console.WriteLine("  2) Medium (~15%)");
Console.WriteLine("  3) Hard (~20%)");
Console.WriteLine("  4) Custom (%)");
int choice = ReadInt("Choose 1-4", 2, 1, 4);

float diff = choice switch
{
    1 => 0.10f,
    2 => 0.15f,
    3 => 0.20f,
    _ => ReadPercent("Enter custom %", 12f)
};

var board = new BoardModel(size);
board.SetDifficultyPercentage(diff);
IBoardService svc = new BoardService();
svc.SetupBombs(board);
svc.CountBombsNearby(board);

Console.WriteLine($"\n{size}x{size} Minesweeper ({diff:P0}). Type 'help'.");

// play loop
while (board.GameState == GameState.InProgress)
{
    Console.WriteLine($"Tokens: {board.RewardTokens}");
    ConsolePrinter.PrintVisible(board, false);
    Console.Write("> ");
    string? line = Console.ReadLine();
    if (line == null) break;
    line = line.Trim();
    if (line.Length == 0) continue;

    var parts = line.Split(' ', StringSplitOptions.RemoveEmptyEntries);
    string cmd = parts[0].ToLowerInvariant();

    try
    {
        switch (cmd)
        {
            case "help":
                PrintHelp();
                break;
            case "reveal":
            case "r":
                RequireArgs(parts, 3);
                svc.Reveal(board, int.Parse(parts[1]), int.Parse(parts[2]));
                break;
            case "flag":
            case "f":
                RequireArgs(parts, 3);
                int fr = int.Parse(parts[1]), fc = int.Parse(parts[2]);
                if (!svc.InBounds(board, fr, fc)) { Console.WriteLine("Out of bounds."); break; }
                board.Grid[fr, fc].IsFlagged = !board.Grid[fr, fc].IsFlagged;
                break;
            case "peek":
                if (parts.Length == 3)
                {
                    if (board.RewardTokens <= 0) { Console.WriteLine("No tokens."); break; }
                    int pr = int.Parse(parts[1]), pc = int.Parse(parts[2]);
                    if (!svc.InBounds(board, pr, pc)) { Console.WriteLine("Out of bounds."); break; }
                    var cell = board.Grid[pr, pc];
                    Console.WriteLine(cell.HasBomb ? "Peek: 💣 BOMB" : "Peek: ✅ SAFE");
                    board.RewardTokens--;
                }
                else
                {
                    Console.WriteLine("Use: peek row col");
                }
                break;
            case "quit":
            case "exit":
                Console.WriteLine("Goodbye!");
                Environment.Exit(0);
                break;
            default:
                Console.WriteLine("Unknown. Type 'help'.");
                break;
        }
    }
    catch (Exception ex)
    {
        Console.WriteLine($"Error: {ex.Message}");
    }
}

ConsolePrinter.PrintVisible(board, true);
Console.WriteLine(board.GameState == GameState.Won ? "🎉 You win!" : "💥 Boom! Lost.");

// show help
static void PrintHelp()
{
    Console.WriteLine("""
Commands:
  reveal r c — open cell
  flag r c   — mark cell
  peek r c   — use 1 token to check
  help       — show help
  quit       — exit
""");
}

// read int
static int ReadInt(string msg, int def, int min, int max)
{
    while (true)
    {
        Console.Write($"{msg} [{def}]: ");
        var s = Console.ReadLine();
        if (string.IsNullOrWhiteSpace(s)) return def;
        if (int.TryParse(s, out int v) && v >= min && v <= max) return v;
        Console.WriteLine($"Enter {min}-{max}.");
    }
}

// read %
static float ReadPercent(string msg, float def)
{
    while (true)
    {
        Console.Write($"{msg} [{def}%]: ");
        var s = Console.ReadLine();
        if (string.IsNullOrWhiteSpace(s)) return def / 100f;
        if (float.TryParse(s, out float v) && v > 0 && v < 100) return v / 100f;
        Console.WriteLine("Enter 1-99.");
    }
}

// need args
static void RequireArgs(string[] parts, int n)
{
    if (parts.Length < n) throw new ArgumentException("Need more numbers.");
}
