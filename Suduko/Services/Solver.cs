using System;
using Suduko.Models;

namespace Suduko.Services
{
    // adapted from https://www.c-sharpcorner.com/blogs/sudoku-solver

    public class Solver : ISolver
    {
        public Board _board { get; set; } = null!;

        public Solver()
        {
        }

        public void SolveSudoku(Board board)
        {
            if (board == null || board.Cells.Length == 0)
                return;
            _board = board;
            Solve(board!.Cells);
        }

        private bool Solve(Cell[,] cells)
        {
            for (int i = 0; i < cells.GetLength(0); i++)
            {
                for (int j = 0; j < cells.GetLength(1); j++)
                {
                    if (cells[i, j].Value.Equals(0))
                    {
                        for (int c = 1; c <= 9; c++)
                        {
                            int candidate = _board.RandomOrder[c - 1];

                            if (IsValid(cells, i, j, candidate))
                            {
                                cells[i, j].Value = candidate;

                                if (Solve(cells))
                                    return true;
                                else
                                    cells[i, j].Value = 0;
                            }
                        }
                        return false;
                    }
                }
            }
            return true;
        }

        private bool IsValid(Cell[,] board, int row, int col, int c)
        {
            for (int i = 0; i < 9; i++)
            {
                //check row  
                if (board[i, col].Value != 0 && board[i, col].Value == c)
                    return false;
                //check column  
                if (board[row, i].Value != 0 && board[row, i].Value == c)
                    return false;
                //check 3*3 block  
                if (board[3 * (row / 3) + i / 3, 3 * (col / 3) + i % 3].Value != 0 && board[3 * (row / 3) + i / 3, 3 * (col / 3) + i % 3].Value == c)
                    return false;
            }
            return true;
        }
    }
}

