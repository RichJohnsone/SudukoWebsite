namespace Suduko.Graph
{
    public class CageBuilder
	{
		public CageBuilder()
		{

		}

        List<int> _availableCells = null!;
        List<Cage> _cages = null!;
        Cage _currentCage = null!;
        Random rnd = new Random();
        private List<int> _cageSizes = new() { 2, 2, 3, 3, 3, 3, 3, 3, 4, 4, 4, 5, 6, 7, 8 };
        int _boardSize = 0;
        sbyte cageIndex = 0;
        //public List<string> Colours = new() { "LightSkyBlue", "LightPink", "Aquamarine", "Gold", "GreenYellow" };
        public List<string> Colours = new() { "#daf0ee", "#fedc97", "#fcb9b2", "#bcd3f2", "#FDFCDC", "#00AFB9", "#FED9B7", "#F07167"};

        public List<Cage> Build(int size) // size of 1 side of board
		{
            _cages = new();
            _availableCells = Enumerable.Range(0, (size * size)).ToList();
            _boardSize = size;
            _cageSizes = _cageSizes.Where(c => c < size).ToList();

            bool built = BuildCages();

            for (sbyte i = 0; i < _cages.Count(); i++){
                _cages[i].Id = i;
            }

            AssignColours();
            SetFirstCells();

            return _cages;
        }

        private bool BuildCages()
        {
            int failureCount = 0;

            while (true)
            {
                // select random start position - not in a cage already
                sbyte origin = GetStartPosition();

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
                    failureCount++;
                    cageIndex--;
                }

                // check if there are any cells not in a cage
                if (_availableCells.Count == 0) return true;
                if (failureCount >= 400) break;
            }

            // so now we need to tidy up the grid a bit
            List<int> available = new();
            available.AddRange(_availableCells);
            int singleCellCageCount = 0;
            int singleCellCageMaxCount = rnd.Next(7, 11); // can be used to make solution easier

            foreach (sbyte cell in available)
            {
                // check cell is still available
                if (!_availableCells.Contains(cell)) continue;

                // does it neighbour any other available cells
                if (_availableCells.Any(c => c.IsNeighbour(cell, _boardSize)) == true)
                {
                    _currentCage = new Cage(cageIndex++, cell);
                    _currentCage.CellIds.Add((sbyte)_availableCells.First(c => c.IsNeighbour(cell, _boardSize)));
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
                    List<int> neighbours = GetNeighbours(cell);
                    // are any neighbour cells in _available Cells
                    List<int> availableCells = _availableCells.Where(x => neighbours.Contains(x)).ToList();
                    if (availableCells?.Any() == true)
                    {
                        _currentCage = new Cage(cageIndex++, cell);
                        _currentCage.CellIds.Add((sbyte)_availableCells.First());
                        CommitCage();
                    }
                    else
                    {
                        // pick a neighbour
                        // maybe try and join in same region
                        neighbours = neighbours.OrderBy(i => Guid.NewGuid()).ToList();

                        foreach (var n in neighbours)
                        {
                            var cages = _cages.Where(d => d.CellIds.Contains((sbyte)n));
                            if (cages?.Any() == true)
                            {
                                cages.First().CellIds.Add(cell);
                                _availableCells.Remove(cell);
                                break;
                            }
                            else
                            {

                            }
                        }
                    }
                }
            }

            return _availableCells.Count() == 0;
        }

        private void CommitCage()
        {
            _cages.Add(_currentCage);
            foreach (int c in _currentCage.CellIds)
                _availableCells.Remove(c);
        }

        private bool BuildCage(int targetSize)
        {
            if (targetSize == 1) return true;

            while (true)
            {
                // find available neighbours
                List<int> neighbours = GetAvailableNeighbours();

                // select one
                while (neighbours.Count > 0)
                {
                    _currentCage.CellIds.Add((sbyte)neighbours.First());
                    if (_currentCage.CellIds.Count == targetSize) return true;
                    break;
                }
                if (neighbours.Count == 0) return false;
            }
        }

        private List<int> GetAvailableNeighbours()
        {
            List<int> neighbours = new();
            int currentCell = _currentCage.CellIds.Last();

            // check above
            if (currentCell > _boardSize && _availableCells.Contains(currentCell - _boardSize))
                neighbours.Add(currentCell - _boardSize);
            // check below
            if (currentCell < (_boardSize * _boardSize - _boardSize) && _availableCells.Contains(currentCell + _boardSize))
                neighbours.Add(currentCell + _boardSize);
            // check left
            if (currentCell % _boardSize != 0 && _availableCells.Contains(currentCell - 1))
                neighbours.Add(currentCell - 1);
            // check right
            if (currentCell % _boardSize != _boardSize -1 && _availableCells.Contains(currentCell + 1))
                neighbours.Add(currentCell + 1);
            // remove any already in cell
            foreach (var c in _currentCage.CellIds)
            {
                if (neighbours.Contains(c))
                    neighbours.Remove(c);
            }

            return neighbours.OrderBy(i => Guid.NewGuid()).ToList();
        }

        private List<int> GetNeighbours(int cell)
        {
            List<int> neighbours = new();

            //// check above
            //if (cell > _boardSize)
            //    neighbours.Add(cell - _boardSize);
            //// check below
            //if (cell < (_boardSize * _boardSize - _boardSize))
            //    neighbours.Add(cell + _boardSize);
            //// check left
            //if (cell % _boardSize != 0)
            //    neighbours.Add(cell - 1);
            //// check right
            //if (cell % _boardSize != _boardSize - 1)
            //    neighbours.Add(cell + 1);

            neighbours.Add(cell - 1);
            neighbours.Add(cell + 1);
            neighbours.Add(cell - _boardSize);
            neighbours.Add(cell + _boardSize);

            return neighbours.Where(n => n > -1 && n < _boardSize * _boardSize).ToList();
        }

        // get a random position to try building a cage from
        private sbyte GetStartPosition()
        {
            int availableCells = _availableCells.Count;
            int cellIndex = rnd.Next(0, availableCells - 1);
            return (sbyte)_availableCells[cellIndex];
        }

        // for creating cages
        private int GetCageSize()
        {
            int index = rnd.Next(0, _cageSizes.Count - 1);
            int size = _cageSizes[index];
            return size;
        }

        private void SetFirstCells()
        {
            foreach (Cage cage in _cages)
            {
                sbyte first = cage.CellIds.First();
                int firstCol = cage.CellIds.First() % _boardSize;
                int firstRow = (cage.CellIds.First() - firstCol) / _boardSize;

                foreach (sbyte cell in cage.CellIds)
                {
                    //c.Cage = cage;
                    int col = cell % _boardSize;
                    int row = (cell - col) / _boardSize;
                    if (col < firstCol || row < firstRow)
                    {
                        first = cell;
                        firstRow = row;
                        firstCol = col;
                    }
                }
                cage.CageStart = first;
            }
        }

        private void AssignColours()
        {
            // work out neighbouring cages
            ColouringGraph graph = new((sbyte)_cages.Count);

            foreach (Cage cage in _cages)
            {
                SetNeighbouringCages(cage);
                graph.AddEdges((sbyte)cage.Id, cage.NeighbouringCages);
            }

            Dictionary<sbyte, sbyte> colouring = graph.GreedyColouring();

            foreach (Cage cage in _cages)
            {
                var colourIndex = colouring[(sbyte)cage.Id];
                cage.Colour = Colours[colourIndex];
            }
        }

        private void SetNeighbouringCages(Cage cage) 
        {
            HashSet<int> cells = new();

            foreach (sbyte cellId in cage.CellIds)
            {
                //var neighbouringCages =
                //    from n in GetNeighbours(cellId)
                //    from c in _cages
                //    where c.CellIds.Contains((sbyte)n)
                //    select c.Id;

                //foreach (var n in neighbouringCages)
                //    cages.Add(n);

                cells.Add(cellId - 1);
                cells.Add(cellId + 1);
                cells.Add(cellId - _boardSize);
                cells.Add(cellId + _boardSize);
            }

            var cages =
                from cell in cells
                from cg in _cages
                where cell > -1 && cell < _boardSize * _boardSize && cg.CellIds.Contains((sbyte)cell)
                select cg.Id;

            cage.NeighbouringCages = cages.Distinct().ToList();
        }
    }
}

