/*
sergio thomas pineda 
cst250
minesweeper milestone2
*/

namespace Minesweeper.Models;

// Square info
public class CellModel
{
    public bool HasBomb { get; set; }     // danger
    public int BombsNearby { get; set; }  // count
    public bool IsRevealed { get; set; }  // shown
    public bool IsFlagged { get; set; }   // marked
    public bool HasReward { get; set; }   // gift
}
