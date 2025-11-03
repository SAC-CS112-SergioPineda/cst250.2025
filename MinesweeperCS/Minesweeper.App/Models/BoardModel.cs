/*
sergio thomas pineda 
cst250
minesweeper milestone2
*/
using System;

namespace Minesweeper.Models;

// Whole board
public class BoardModel
{
    public int Size { get; }                     // width
    public float DifficultyPercentage { get; private set; }  // level
    public GameState GameState { get; set; } = GameState.InProgress; // status
    public CellModel[,] Grid { get; }            // cells
    public int RewardTokens { get; set; } = 0;   // peeks

    // make board
    public BoardModel(int size)
    {
        if (size < 2 || size > 40) throw new ArgumentOutOfRangeException(nameof(size));
        Size = size;
        Grid = new CellModel[size, size];
        for (int r = 0; r < size; r++)
        for (int c = 0; c < size; c++)
            Grid[r, c] = new CellModel();
    }

    // set level
    public void SetDifficultyPercentage(float pct)
    {
        if (pct <= 0 || pct >= 1) throw new ArgumentOutOfRangeException(nameof(pct));
        DifficultyPercentage = pct;
    }
}
