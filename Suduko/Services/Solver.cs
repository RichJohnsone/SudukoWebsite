using System;
using Suduko.Models;

namespace Suduko.Services
{
    // adapted from https://www.c-sharpcorner.com/blogs/sudoku-solver

    //public class Solver : ISolver
    //{
    //    public Board _board { get; set; } = null!;
    //    private List<int> numbers = new() { 0, 1, 2, 3, 4, 5, 6, 7, 8 };

    //    public Solver()
    //    {
    //    }

    //    public void SolveSudoku(Board board)
    //    {
    //        if (board == null || board.Cells.Length == 0)
    //            return;
    //        _board = board;

    //        //solutionCount = 0;
    //        //solutions = new List<int[,]>();

    //        Solve(board!.Cells);

    //        //for (int i = 0; i < board.Cells.GetLength(0); i++)
    //        //{
    //        //    for (int j = 0; j < board.Cells.GetLength(1); j++)
    //        //    {
    //        //        board.Cells[i, j].Value = solutions.First()[1, j];
    //        //    }
    //        //}
    //    }

    //    //int solutionCount = 0;
    //    //List<int[,]> solutions = new List<int[,]>();

    //    private bool Solve(Cell[,] cells)
    //    {
    //        int i, j;
    //        numbers = numbers.OrderBy(i => Guid.NewGuid()).ToList();
    //        for (int a = 0; a < cells.GetLength(0); a++)
    //        {
    //            i = a;// numbers[a];
    //            for (int b = 0; b < cells.GetLength(1); b++)
    //            {
    //                j = b;// numbers[b];
    //                if (cells[i, j].Value.Equals(0))
    //                {
    //                    for (int c = 1; c <= 9; c++)
    //                    {
    //                        sbyte candidate = _board.RandomOrder[c - 1];

    //                        if (IsValid(cells, i, j, candidate))
    //                        {
    //                            cells[i, j].Value = candidate;

    //                            if (Solve(cells))
    //                            {
    //                                //solutions.Add(CellsToIntArray(cells));
    //                                //solutionCount++;
    //                                //if (solutionCount < 1)
    //                                    return true;
    //                            }
    //                            else
    //                                cells[i, j].Value = 0;
    //                        }
    //                    }
    //                    return false;
    //                }
    //            }
    //        }
    //        return true;
    //    }

    //    private int[,] CellsToIntArray(Cell[,] cells)
    //    {
    //        int[,] solution = new int[cells.GetLength(0), cells.GetLength(1)];
    //        for (int i = 0; i < cells.GetLength(0); i++)
    //        {
    //            for (int j = 0; j < cells.GetLength(1); j++)
    //            {
    //                solution[i, j] = cells[i, j].Value;
    //            }
    //        }
    //        return solution;
    //    }

    //    private bool IsValid(Cell[,] board, int row, int col, int c)
    //    {
    //        for (int i = 0; i < 9; i++)
    //        {
    //            //check row  
    //            if (board[i, col].Value != 0 && board[i, col].Value == c)
    //                return false;
    //            //check column  
    //            if (board[row, i].Value != 0 && board[row, i].Value == c)
    //                return false;
    //            //check 3*3 block  
    //            if (board[3 * (row / 3) + i / 3, 3 * (col / 3) + i % 3].Value != 0 && board[3 * (row / 3) + i / 3, 3 * (col / 3) + i % 3].Value == c)
    //                return false;
    //        }
    //        return true;
    //    }
    //}
}

