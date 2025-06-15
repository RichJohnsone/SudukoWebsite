using System;
using System.Data.Common;
using System.Drawing;
using Suduko.Helpers;
using Suduko.Models;

namespace Suduko.Services
{
    public class CageBuilder : ICageBuilder
    {
        public Board _board { get; set; } = null!;
        private List<Cell> _availableCells = new();
        List<Cage> _cages = new();
        Random rnd = new Random();
        private List<int> _cageSizes = new() { 2, 2, 3, 3, 3, 3, 3, 4, 4, 4, 5, 5, 6, 7 };
        private Cage _currentCage = null!;
        int cageIndex = 0;
        public List<string> Colours = new() { "LightSkyBlue", "LightPink", "Aquamarine", "Gold", "GreenYellow" };

        public CageBuilder()
        {
        }

        public List<Cage> Build(Board board)
        {
            _board = board;
            _board.Cages = new();
            PopulateAvailableCells();
            BuildCages();
            AssignColours();
            return _cages;
        }

        private void AssignColours()
        {
            // work out neighbouring cages
            Graph graph = new(_cages.Count);

            foreach (Cage cage in _cages)
            {
                SetNeighbouringCages(cage);
                graph.AddEdges(cage.Id, cage.NeighbouringCages);
            }

            Dictionary<int, int> colouring = graph.GreedyColouring();

            foreach (Cage cage in _cages)
            {
                var colourIndex = colouring[cage.Id];
                cage.Colour = Colours[colourIndex];
            }
        }

        private void SetNeighbouringCages(Cage cage)
        {
            HashSet<int> neighbouringCages = new();

            foreach (Cell cell in cage.Cells)
            {
                if (cell.Column < _board.Size -1)
                    neighbouringCages.Add(_board.Cells[cell.Row, cell.Column + 1].Cage.Id);
                if (cell.Column > 1)
                    neighbouringCages.Add(_board.Cells[cell.Row, cell.Column - 1].Cage.Id);

                if (cell.Row < _board.Size -1)
                    neighbouringCages.Add(_board.Cells[cell.Row + 1, cell.Column].Cage.Id);
                if (cell.Row > 1)
                    neighbouringCages.Add(_board.Cells[cell.Row - 1, cell.Column].Cage.Id);
            }

            cage.NeighbouringCages = neighbouringCages.ToList();
        }

        private bool BuildCages()
        {
            int failureCount = 0;

            while (true)
            {
                // select random start position - not in a cage already
                Cell origin = GetStartPosition();

                // select random cage size
                int cageSize = GetCageSize();

                // build cage
                _currentCage = new Cage(cageIndex++, origin);

                if (BuildCage(cageSize))
                {
                    CommitCage();
                }
                else
                {
                    cageIndex--;
                    failureCount++;
                }

                // check if there are any cells not in a cage
                if (_availableCells.Count == 0) return true;
                if (failureCount >= 100) break;
            }

            // so now we need to tidy up the grid a bit=
            List<Cell> available = new();
            available.AddRange(_availableCells);
            int singleCellCageCount = 0;
            int singleCellCageMaxCount = rnd.Next(0, 3);
            foreach (Cell cell in available)
            {
                // check cell is still available
                if (!_availableCells.Contains(cell)) continue;
                // does it neighbour any other available cells
                if (_availableCells.Any(c => c.IsNeighbour(cell)) == true)
                {
                    _currentCage = new Cage(cageIndex++, cell);
                    _currentCage.Cells.Add(_availableCells.First(c => c.IsNeighbour(cell)));
                    CommitCage();
                }
                else if (singleCellCageCount < singleCellCageMaxCount)
                {
                    _currentCage = new Cage(cageIndex++, cell);
                    CommitCage();
                    singleCellCageCount++;
                }
                else // we try and join it to an existing cage
                {
                    List<Cell> neighbours = GetNeighbours(cell);
                    // are any neighbour cells in _available Cells
                    List<Cell> availableCells = _availableCells.Where(x => neighbours.Contains(x)).ToList();
                    if (availableCells?.Any() == true){
                        _currentCage = new Cage(cageIndex++, cell);
                        _currentCage.Cells.Add(availableCells.First(c => c.IsNeighbour(cell)));
                        CommitCage();
                    }
                    else
                    {
                        // pick a neighbour
                        neighbours = neighbours.OrderBy(i => Guid.NewGuid()).ToList();
                        _currentCage = neighbours.First().Cage;
                        _currentCage.Cells.Add(cell);
                        cell.Cage = _currentCage;
                    }
                }
            }
            return false;
        }

        private List<Cell> GetNeighbours(Cell cell)
        {
            List<Cell> neighbours = new();

            if (cell.Column < _board.Size - 1)
                neighbours.Add(_board.Cells[cell.Row, cell.Column + 1]);
            if (cell.Column > 1)
                neighbours.Add(_board.Cells[cell.Row, cell.Column - 1]);

            if (cell.Row < _board.Size - 1)
                neighbours.Add(_board.Cells[cell.Row + 1, cell.Column]);
            if (cell.Row > 1)
                neighbours.Add(_board.Cells[cell.Row - 1, cell.Column]);

            return neighbours;
        }

        private bool BuildCage(int targetSize)
        {
            if (targetSize == 1) return true;

            while (true)
            {
                // find available neighbours
                var neighbours = GetAvailableNeighbours();

                // select one
                while (neighbours.Count > 0)
                {
                    _currentCage.Cells.Add(neighbours.First());
                    if (CageIsValid(_currentCage, targetSize))
                    {
                        if (_currentCage.Cells.Count == targetSize) return true;
                        break;
                    }
                    else
                    {
                        _currentCage.Cells.RemoveAt(_currentCage.Cells.Count - 1);
                        neighbours.RemoveAt(0);
                    }
                }
                if (neighbours.Count == 0) return false;
            }
        }

        private bool CageIsValid(Cage cage, int targetSize)
        {
            // check dupes
            if (cage.Cells.Select(c => c.Value).Distinct().Count() != cage.Cells.Count) return false;

            // check min
            if (cage.Value < MinimumCageValue(targetSize)) return false;

            // check max
            if (cage.Value > MaximumCageValue(targetSize)) return false;

            return true;
        }

        private int MaximumCageValue(int size)
        {
            switch (size)
            {
                case 1:
                    return 9;
                case 2:
                    return 17;
                case 3:
                    return 24;
                case 4:
                    return 30;
                case 5:
                    return 35;
                case 6:
                    return 39;
                case 7:
                    return 42;
                case 8:
                    return 44;
                default:
                    return 0;
            }
        }

        private int MinimumCageValue(int size)
        {
            switch (size)
            {
                case 1:
                    return 1;
                case 2:
                    return 3;
                case 3:
                    return 6;
                case 4:
                    return 10;
                case 5:
                    return 15;
                case 6:
                    return 21;
                case 7:
                    return 28;
                case 8:
                    return 36;
                default:
                    return 0;
            }
        }

        private List<Cell> GetAvailableNeighbours()
        {
            Cell currentCell = _currentCage.Cells.Last();
            return _availableCells.Where(c => c.IsNeighbour(currentCell)).OrderBy(i => Guid.NewGuid()).ToList();
        }

        private void CommitCage()
        {
            _cages.Add(_currentCage);
            Cell first = _currentCage.Cells.First();

            foreach (Cell c in _currentCage.Cells)
            {
                c.Cage = _currentCage;
                _availableCells.Remove(c);
                if (c.Column < first.Column || c.Row < first.Row) first = c;
            }

            first.IsCageStart = true;
        }

        private string GetColour(Cage currentCage)
        {
            int index = rnd.Next(0, 3);
            return Colours[index];
        }

        private int GetCageSize()
        {
            int index = rnd.Next(0, _cageSizes.Count - 1);
            int size = _cageSizes[index];
            return size;
        }

        private void PopulateAvailableCells()
        {
            for (int x = 0; x < _board.Size; x++)
            {
                for (int y = 0; y < _board.Size; y++)
                {
                    _availableCells.Add(_board.Cells[x, y]);
                }
            }
        }

        private Cell GetStartPosition()
        {
            int availableCells = _availableCells.Count;
            int cellIndex = rnd.Next(0, availableCells - 1);
            return _availableCells[cellIndex];
        }
    }
}

