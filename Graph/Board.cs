namespace Suduko.Graph
{
    public class Board
    {
        public sbyte Size { get; set; } = 9; // the length of one edge
        public List<Cell> Cells { get; set; } = null!;
        public List<Cage> Cages { get; set; } = null!;
        public List<sbyte> RandomOrder = new();
        public Graph SolverGraph { get; set; } = null!;
        public CageBuilder _cageBuilder = new();

        public Board(sbyte size = 9)
        {
            Size = size;
            Initialise();
        }

        public void Initialise()
        {
            // initialise cells
            Cells = new List<Cell>();
            for (sbyte x = 0; x < Size; x++)
            {
                for (sbyte y = 0; y < Size; y++)
                {
                    Cells.Add(new Cell(x, y));
                }
            }

            // initialise random order
            for (sbyte s = 0; s < Size; s++) RandomOrder.Add(s);
            RandomOrder = RandomOrder.OrderBy(i => Guid.NewGuid()).ToList();

            // initialise edges
            var offsets = GetOffsets(Size);

            var edges = Graph.OffsetsToEdges(offsets);

            // initialise cages
            var cages = _cageBuilder.Build(Size);

            // add edges for cages
            List<List<sbyte>> cageMemberLists = new();
            foreach (var c in cages)
            {
                cageMemberLists.Add(c.CellIds);
            }

            // add cages to cells
            foreach (var cg in cages)
            {
                Cells[cg.CageStart].IsCageStart = true;
                foreach (sbyte c in cg.CellIds)
                {
                    Cells[c].Cage = cg;
                }
            }

            // check cells all have a cage
            foreach (var c in Cells)
            {
                if (c.Cage is null)
                {

                }
            }

            edges.ToList().AddRange(Graph.CliquesToEdges(cageMemberLists));
            this.Cages = cages;

            // initilise graph
            SolverGraph = new Graph(Size * Size, edges);
            Solver solver;

            // get solution
            IEnumerable<IEnumerable<int>> results = null!;
            int iterationCount = 0;

            List<int> result = null!;

            while (true)
            {
                iterationCount++;

                solver = new Solver(SolverGraph, Size, cageMemberLists);

                RandomOrder = RandomOrder.OrderBy(i => Guid.NewGuid()).ToList();

                Dictionary<int, int> nodeColours = new();

                nodeColours.Add(0, RandomOrder[3]);

                if (Size > 4)
                {
                    //nodeColours.Add(8, RandomOrder[8]);
                    nodeColours.TryAdd(RandomOrder[3], RandomOrder[6]);
                    //nodeColours.TryAdd(10 + RandomOrder[2], RandomOrder[5]);
                    nodeColours.TryAdd(20 + RandomOrder[5], RandomOrder[2]);
                    //nodeColours.TryAdd(40 + RandomOrder[8], RandomOrder[1]);
                    nodeColours.TryAdd(50 + RandomOrder[7], RandomOrder[3]);
                    //nodeColours.TryAdd(60 + RandomOrder[1], RandomOrder[7]);
                    nodeColours.TryAdd(71 + RandomOrder[3], RandomOrder[6]);
                }

                solver = solver.SetColours(nodeColours);

                results = solver.Solve();
                if (results?.Any() == true)
                {
                    result = results.First().ToList();
                    // check results
                    bool ok = true;
                    foreach (int r in result)
                    {
                        if (r > Size - 1)
                        {
                            ok = false;
                        }
                    }

                    // store solution in Cells
                    for (int x = 0; x < Size * Size; x++)
                    {
                        Cells[x].Value = (sbyte)(result[x] + 1);
                        Cells[x].Solution = Cells[x].Value;
                    }

                    // check cages don't have dupes
                    foreach (var cage in Cages)
                    {
                        List<sbyte> cageValues = new();
                        foreach (var cell in cage.CellIds)
                        {
                            cageValues.Add(Cells[cell].Value);
                        }
                        if (cageValues.Distinct().Count() != cage.CellIds.Count())
                        {
                            ok = false;
                        }
                    }
                    if (ok)
                        break;
                }
            }

            //// store solution in Cells
            //for (int x = 0; x < Size * Size; x++)
            //{
            //    Cells[x].Value = (sbyte)(result[x] + 1);
            //    Cells[x].Solution = Cells[x].Value;
            //}

            // set cage totals
            List<sbyte> cageCells = new();
            foreach (var cg in cages)
            {
                sbyte total = 0;
                foreach (sbyte c in cg.CellIds)
                {
                    total += Cells[c].Value;
                    cageCells.Add(c);
                }
                cg.Value = total;
            }

            // reset cell values
            for (int x = 0; x < Size * Size; x++)
            {
                if (Cells[x].Cage.Size > 1)
                    Cells[x].Value = 0;
            }
        }

        private int[,][] GetOffsets(sbyte size)
        {
            switch (size)
            {
                case 4:
                    return new[,] {
                        /*rows*/    {new[] {0, 4, 8, 12}, new[] {0, 1, 2, 3 }},
                        /*columns*/ {new[] {0, 1, 2, 3},  new[] {0, 4, 8, 12}},
                        /*squares */{new[] {0, 2, 8, 10}, new[] {0, 1, 4, 5 }}
                    };
                case 9:
                    return new[,] {
                        /*rows*/   {new[] {0, 9, 18, 27, 36, 45, 54, 63, 72}, new[] {0, 1, 2, 3, 4, 5, 6, 7, 8}},
                        /*columns*/{new[] {0, 1, 2, 3, 4, 5, 6, 7, 8},        new[] {0, 9, 18, 27, 36, 45, 54, 63, 72}},
                        /*boxes */ {new[] {0, 3, 6, 27, 30, 33, 54, 57, 60},  new[] {0, 1, 2, 9, 10, 11, 18, 19, 20}}
                    };
                default:
                    throw new ApplicationException("Invalid grid size");
            }
        }
    }
}