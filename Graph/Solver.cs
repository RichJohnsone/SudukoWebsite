namespace Suduko.Graph
{
    public sealed class Solver
    {
        private enum Result { Solved, Unsolved, Busted }
        private readonly Graph graph;
        private readonly BitSet[] possibilities;
        private int colours; // these are the digits 1 to 9
        public List<List<sbyte>> _cages { get; set; } = null!;

        public Solver(Graph graph, int colours, List<List<sbyte>> cages = null!)
        {
            this.colours = colours;
            this.graph = graph;
            this.possibilities = new BitSet[graph.Size];
            this._cages = cages;

            BitSet b = BitSet.Empty;

            for (int i = 0; i < colours; ++i)
                b = b.Add(i);

            for (int p = 0; p < this.possibilities.Length; ++p)
                this.possibilities[p] = b;

            foreach (var p in this.possibilities)
            {
                if (p.Max() > this.colours - 1)
                {

                }
            }
        }

        private Solver(Graph graph, int colours, BitSet[] possibilities, List<List<sbyte>> cages = null!)
        {
            this.graph = graph;
            this.colours = colours;
            this.possibilities = possibilities;
            this._cages = cages;

            //foreach (var p in this.possibilities)
            //{
            //    if (p.Max() > this.colours - 1)
            //    {

            //    }
            //}
        }

        // Make a new solver with one of the colours known.
        public Solver SetColour(int node, int colour)
        {
            BitSet[] newPossibilities = (BitSet[])this.possibilities.Clone();
            newPossibilities[node] = BitSet.Empty.Add(colour);
            return new Solver(this.graph, this.colours, newPossibilities, this._cages);
        }

        // Make a new solver with several of the colours known.
        public Solver SetColours(Dictionary<int, int> nodeColours)
        {
            BitSet[] newPossibilities = (BitSet[])this.possibilities.Clone();

            foreach(var pair in nodeColours)
            {
                newPossibilities[pair.Key] = BitSet.Empty.Add(pair.Value);
            }
            
            return new Solver(this.graph, this.colours, newPossibilities, this._cages);
        }

        private Result Status
        {
            get
            {
                if (possibilities.Any(p => !p.Any()))
                    return Result.Busted;

                if (possibilities.All(p => p.Count() == 1))
                    return Result.Solved;

                return Result.Unsolved;
            }
        }

        private Solver Reduce()
        {
            BitSet[] newPossibilities = (BitSet[])this.possibilities.Clone();
            // The query answers the question "What colour possibilities should I remove?"
            var reductions =
                from node in Enumerable.Range(0, newPossibilities.Length)
                where newPossibilities[node].Count() == 1
                let colour = newPossibilities[node].Single()
                from neighbour in graph.Neighbours(node)
                where newPossibilities[neighbour].Contains(colour)
                select new { neighbour, colour };

            bool progress = false;

            while (true)
            {
                var list = reductions.ToList();
                if (list.Count == 0)
                    break;
                progress = true;
                foreach (var reduction in list)
                    newPossibilities[reduction.neighbour] = newPossibilities[reduction.neighbour].Remove(reduction.colour);
                // Doing so might have created a new node that has a single possibility,
                // which we can then use to make further reductions. Keep looping until
                // there are no more reductions to be made.
            }

            return progress ? new Solver(graph, colours, newPossibilities, this._cages) : null!;
        }

        public IEnumerable<IEnumerable<int>> Solve()
        {
            // Base case: we are already solved or busted.
            var status = this.Status;

            if (status == Result.Solved)
                //return this.possibilities.Select(x => x.Single());
                return new[] { this.possibilities.SelectMany(x => x) };

            if (status == Result.Busted)
                //return null;
                return Enumerable.Empty<IEnumerable<int>>();

            // reduce for cages - maybe move this after main Reduce ?
            if (_cages is not null)
            {
                var cageReduced = CageReduce();
                if (cageReduced != null)
                    return cageReduced.Solve();
            }

            // Easy inductive case: do simple reductions and then solve again.
            var reduced = Reduce();
            if (reduced != null)
                return reduced.Solve();

            // Difficult inductive case: there were no simple reductions.
            // Make a hypothesis about the colouring of a node and see if
            // that introduces a contradiction or a solution.
            
            foreach (var b in this.possibilities)
            {
                if (b.Max() > this.colours - 1)
                {

                }
            }

            int node = this.possibilities.FirstIndex(p => p.Count() > 1);
            var solutions =
                from colour in this.possibilities[node]

                //from cage in _cages
                //where cage.Contains((sbyte)node)

                //let singleValuesFromNodes = from n in cage
                //                            where this.possibilities[n].Count() == 1
                //                            select this.possibilities[n].Single()

                //where !singleValuesFromNodes.Contains(colour)

                let solution = this.SetColour(node, colour).Solve()
                where solution != null
                    //&& CagesValid(solution) == true
                select solution;

            //return solutions.FirstOrDefault();
            return solutions.SelectMany(x => x);
        }

        private bool CagesValid(IEnumerable<IEnumerable<int>> solution)
        {
            //if (solution is null) return false;

            if (_cages is null) return true;

            foreach (var cage in _cages)
            {
                var singleValuesFromNodes =
                    from node in cage
                    where this.possibilities[node].Count() == 1
                    select this.possibilities[node].Single();

                // check for dupes
                if (singleValuesFromNodes.Any() && singleValuesFromNodes.Distinct().Count() < singleValuesFromNodes.Count())
                    return false;

                //int cageTotal = singleValuesFromNodes.Sum();

                //if (cageTotal == 0) continue;

                //int cageSize = cage.Count();
                //// is cage total > max possible for cage size
                //if (cageTotal > MaximumCageValue(cageSize)) return false;
                //// if cage is populated is cage total < min possible for cage size
                //if (singleValuesFromNodes.Count() == cageSize && cageTotal < MinimumCageValue(cageSize)) return false;
            }

            return true;
        }

        private Solver CageReduce()
        {
            BitSet[] newPossibilities = (BitSet[])this.possibilities.Clone();
            // The query answers the question "What colour possibilities should I remove for the cages?"

            // are there any single cell cages with more than 1 possibility?
            //var reductions =
            //    from cage in _cages
            //    where cage.Count() == 1 && newPossibilities[cage.Single()].Count() > 1
            //    let colour = newPossibilities[cage.Single()].ToList().OrderBy(n => new Guid()).First()
            //    from neighbour in graph.Neighbours(cage.Single())
            //    where newPossibilities[neighbour].Contains(colour)
            //    select new { neighbour, colour };

            // are there any cages with cells with one possibility - remove these possibilities from other cells in the cage
            var reductions =
                from cage in _cages
                from node in cage
                where newPossibilities[node].Count() == 1
                let colour = newPossibilities[node].Single()
                from cell in cage
                where cell != node
                    && newPossibilities[cell].Contains(colour)
                select new { neighbour = (int)cell, colour };

            //if (reductions?.Any() == true)
            //{
            //    reductions.ToList().Concat(reductions2);
            //}
            //else
            //{
            //    reductions = reductions2;
            //}

            bool progress = false;

            while (true)
            {
                var list = reductions.ToList();
                if (list.Count == 0)
                    break;
                progress = true;
                foreach (var reduction in list)
                    newPossibilities[reduction.neighbour] = newPossibilities[reduction.neighbour].Remove(reduction.colour);
                // Doing so might have created a new node that has a single possibility,
                // which we can then use to make further reductions. Keep looping until
                // there are no more reductions to be made.
            }

            return progress ? new Solver(graph, colours, newPossibilities, this._cages) : null!;
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
    }
}

// give type to the edges