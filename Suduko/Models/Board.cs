using System;
using Newtonsoft.Json;
using Suduko.Helpers;
using Suduko.Services;

namespace Suduko.Models
{
    //public class Board
    //{
    //    public sbyte Size { get; set; } = 9; // the length of one edge
    //    public Cell[,] Cells { get; set; } = null!;
    //    public List<Cage> Cages { get; set; } = null!;
    //    public List<sbyte> RandomOrder = null!;

    //    [JsonIgnore]
    //    public ISolver _solver;
    //    [JsonIgnore]
    //    public ICageBuilder _cageBuilder;

    //    public Board(sbyte size = 9)
    //    {
    //        Size = size;
    //        _solver = new Solver();
    //        _cageBuilder = new CageBuilder();
    //        Initialise();
    //    }

    //    public void Initialise()
    //    {
    //        // initialise cells
    //        Cells = new Cell[Size, Size];
    //        for (sbyte x = 0; x < Size; x++)
    //        {
    //            for (sbyte y = 0; y < Size; y++)
    //            {
    //                Cells[x, y] = new Cell(x, y);
    //            }
    //        }

    //        // initialis random order
    //        RandomOrder = new List<sbyte>() { 1, 2, 3, 4, 5, 6, 7, 8, 9 };
    //        RandomOrder = RandomOrder.OrderBy(i => Guid.NewGuid()).ToList();

    //        // get solution
    //        _solver.SolveSudoku(this);

    //        // build cages
    //        _cageBuilder.Build(this);

    //        // store solution
    //        for (int x = 0; x < Size; x++)
    //        {
    //            for (int y = 0; y < Size; y++)
    //            {
    //                Cells[x, y].Solution = Cells[x, y].Value;
    //            }
    //        }

    //        // reset cell values
    //        for (int x = 0; x < Size; x++)
    //        {
    //            for (int y = 0; y < Size; y++)
    //            {
    //                if (Cells[x, y].Cage.Size > 1)
    //                    Cells[x, y].Value = 0;
    //            }
    //        }
    //    }
    //}
}