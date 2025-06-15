using System;
namespace Suduko.Models
{
	public class Cell
	{
        public int Value { get; set; }
        public int Solution { get; set; }
        public Cage Cage { get; set; } = null!;
        public List<string> Suggestions { get; set; } = new() { "&nbsp;", "&nbsp;", "&nbsp;", "&nbsp;", "&nbsp;", "&nbsp;", "&nbsp;", "&nbsp;", "&nbsp;" };
        //public List<string> Suggestions { get; set; } = new() { "1", "2", "3", "4", "5", "6", "7", "8", "9" };
        public bool IsValid { get; set; } = true;
        public int Row { get; set; }
        public int Column { get; set; }
		public bool IsCageStart{ get; set; }

		public Cell(int row, int column)
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

