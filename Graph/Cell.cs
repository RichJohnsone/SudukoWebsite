namespace Suduko.Graph
{
    public class Cell
	{
        public sbyte Value { get; set; }
        public sbyte Solution { get; set; }
        public Cage Cage { get; set; } = null!;
        public List<string> Suggestions { get; set; } = new() { "&nbsp;", "&nbsp;", "&nbsp;", "&nbsp;", "&nbsp;", "&nbsp;", "&nbsp;", "&nbsp;", "&nbsp;" };
        //public List<string> Suggestions { get; set; } = new() { "1", "2", "3", "4", "5", "6", "7", "8", "9" };
        public bool IsValid { get; set; } = true;
        public sbyte Row { get; set; }
        public sbyte Column { get; set; }
		public bool IsCageStart{ get; set; }

		public Cell(sbyte row, sbyte column)
		{
			Row = row;
			Column = column;
		}

        internal bool IsNeighbour(Cell cell)
        {
			return (cell.Column == Column && (cell.Row == Row + 1 || cell.Row == Row - 1))
				|| (cell.Row == Row && (cell.Column == Column + 1 || cell.Column == Column - 1));
        }
    }
}

