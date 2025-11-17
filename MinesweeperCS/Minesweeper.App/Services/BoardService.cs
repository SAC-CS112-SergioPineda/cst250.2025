/*
sergio thomas pineda 
cst250
minesweeper milestone2
*/
using Minesweeper.Models;

namespace Minesweeper.Services;

// Game logic
public class BoardService : IBoardService
{
    private readonly Random _rng = new();

    // place bombs + reward
    public void SetupBombs(BoardModel board)
    {
        int n = board.Size;
        int total = n * n;
        int bombs = Math.Max(1, (int)Math.Round(total * board.DifficultyPercentage));

        // reset
        for (int r = 0; r < n; r++)
        for (int c = 0; c < n; c++)
        {
            var cell = board.Grid[r, c];
            cell.HasBomb = false;
            cell.IsFlagged = false;
            cell.IsRevealed = false;
            cell.BombsNearby = 0;
            cell.HasReward = false;
        }

        var spots = new List<(int r, int c)>(total);
        for (int r = 0; r < n; r++)
        for (int c = 0; c < n; c++)
        {
            if (!(r == 1 && c == 1))
                spots.Add((r, c));
        }

        // random bombs
        for (int i = 0; i < bombs; i++)
        {
            int idx = _rng.Next(spots.Count);
            var (r, c) = spots[idx];
            board.Grid[r, c].HasBomb = true;
            spots.RemoveAt(idx);
        }

        // reward spot
        if (n > 1)
        {
            var reward = board.Grid[1, 1];
            reward.HasReward = true;
            reward.HasBomb = false;
        }
    }

    // count bombs
    public void CountBombsNearby(BoardModel board)
    {
        int n = board.Size;
        for (int r = 0; r < n; r++)
        for (int c = 0; c < n; c++)
        {
            if (board.Grid[r, c].HasBomb)
            {
                board.Grid[r, c].BombsNearby = -1;
                continue;
            }
            int count = 0;
            for (int dr = -1; dr <= 1; dr++)
            for (int dc = -1; dc <= 1; dc++)
            {
                if (dr == 0 && dc == 0) continue;
                int nr = r + dr, nc = c + dc;
                if (InBounds(board, nr, nc) && board.Grid[nr, nc].HasBomb)
                    count++;
            }
            board.Grid[r, c].BombsNearby = count;
        }
    }

    // open tile
    public void Reveal(BoardModel board, int row, int col)
    {
        if (!InBounds(board, row, col)) return;
        var cell = board.Grid[row, col];
        if (cell.IsRevealed || cell.IsFlagged) return;

        cell.IsRevealed = true;

        // reward pickup
        if (cell.HasReward)
        {
            cell.HasReward = false;
            board.RewardTokens++;
            Console.WriteLine("🎁 Found reward! +1 peek token.");
        }

        if (cell.HasBomb)
        {
            board.GameState = GameState.Lost;
            return;
        }

        if (cell.BombsNearby == 0)
        {
            for (int dr = -1; dr <= 1; dr++)
            for (int dc = -1; dc <= 1; dc++)
            {
                if (dr == 0 && dc == 0) continue;
                Reveal(board, row + dr, col + dc);
            }
        }

        // win check
        int n = board.Size;
        int revealed = 0, bombs = 0;
        for (int r = 0; r < n; r++)
        for (int c = 0; c < n; c++)
        {
            if (board.Grid[r, c].HasBomb) bombs++;
            else if (board.Grid[r, c].IsRevealed) revealed++;
        }
        if (revealed == n * n - bombs) board.GameState = GameState.Won;
    }

    // check bounds
    public bool InBounds(BoardModel board, int r, int c)
        => r >= 0 && r < board.Size && c >= 0 && c < board.Size;

    private void CheckWin(BoardModel board)
    {
        int n = board.Size;
        int bombs = 0;
        int flaggedBombs = 0;
        int unrevealedSafe = 0;

        for (int r = 0; r < n; r++)
        for (int c = 0; c < n; c++)
        {
            var cell = board.Grid[r, c];
            if (cell.HasBomb)
            {
                bombs++;
                if (cell.IsFlagged) flaggedBombs++;
            }
            else
            {
                if (!cell.IsRevealed) unrevealedSafe++;
            }
        }

        // New win rule: every bomb flagged AND every safe cell revealed
        if (bombs > 0 && flaggedBombs == bombs && unrevealedSafe == 0)
            board.GameState = GameState.Won;
    }

}
