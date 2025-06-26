namespace Suduko.Graph

{ // An immutable directed graph
    public sealed class Graph
    {
        // If there is an edge from A to B then neighbours[A,B] is true.
        readonly public bool[,] neighbours;
        readonly public int nodes;

        public Graph(int nodes, IEnumerable<Tuple<sbyte, sbyte>> edges)
        {
            this.nodes = nodes;
            this.neighbours = new bool[nodes, nodes];
            foreach (var edge in edges)
                this.neighbours[edge.Item1, edge.Item2] = true;
        }

        public IEnumerable<int> Neighbours(int node)
        {
            return
              from i in Enumerable.Range(0, Size)
              where this.neighbours[node, i]
              select i;
        }

        public int Size { get { return this.nodes; } }

        public static IEnumerable<Tuple<sbyte, sbyte>> CliquesToEdges(IEnumerable<IEnumerable<int>> cliques)
        {
            return
              from clique in cliques
              from item1 in clique
              from item2 in clique
              where item1 != item2
              select Tuple.Create((sbyte)item1, (sbyte)item2);
        }

        public static IEnumerable<Tuple<sbyte, sbyte>> CliquesToEdges(IEnumerable<IEnumerable<sbyte>> cliques)
        {
            return
              from clique in cliques
              from item1 in clique
              from item2 in clique
              where item1 != item2
              select Tuple.Create(item1, item2);
        }

        public static IEnumerable<Tuple<sbyte, sbyte>> OffsetsToEdges(int[,][] offsets)
        {
            //var offsets = new[,] {
            //    /*rows*/    {new[] {0, 4, 8, 12}, new[] {0, 1, 2, 3 }},
            //    /*columns*/ {new[] {0, 1, 2, 3}, new[] {0, 4, 8, 12}},
            //    /*squares */{new[] {0, 2, 8, 10}, new[] {0, 1, 4, 5}}};
            var cliques =
                from r in Enumerable.Range(0, 3)
                from i in offsets[r, 0]
                select (from j in offsets[r, 1] select i + j);
            var edges = CliquesToEdges(cliques);
            return edges;
        }
    }
}