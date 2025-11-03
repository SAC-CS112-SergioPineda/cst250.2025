/*
sergio thomas pineda 
cst250
minesweeper milestone2
*/
using Minesweeper.Models;

namespace Minesweeper.Services;

// Rules
public interface IBoardService
{
    void SetupBombs(BoardModel board);      // bombs
    void CountBombsNearby(BoardModel board);// numbers
    void Reveal(BoardModel board, int row, int col); // open
    bool InBounds(BoardModel board, int r, int c);   // check
}
