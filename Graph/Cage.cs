namespace Suduko.Graph
{
    public class Cage
	{
        public sbyte Id { get; set; }
        public sbyte Value { get; set; } // sum of cell values
        public sbyte Size => (sbyte)CellIds.Count; // number of cells
		public string Colour = string.Empty;
        public List<sbyte> NeighbouringCages { get; set; } = new ();
        public List<sbyte> CellIds { get; set; } = new();
        public sbyte CageStart { get; set; }

        public Cage(sbyte id, List<sbyte> cellIds)
        {
            Id = id;
            CellIds = cellIds;
        }

        public Cage(sbyte id, sbyte origin)
        {
            Id = id;
            CellIds.Add(origin);
        }
    }
}