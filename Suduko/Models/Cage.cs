using System;
namespace Suduko.Models
{
	public class Cage
	{
        public int Id { get; set; }
		public int Value => Cells.Sum(c => c.Value); // sum of cell values
		public int Size => Cells.Count; // number of cells
        public List<Cell> Cells { get; set; } = new();
		public string Colour = string.Empty;
		public Cell Origin { get; private set; }
        public List<int> NeighbouringCages { get; set; } = new ();

        public Cage(int id, Cell origin)
		{
			Id = id;
			Origin = origin;
			Cells.Add(origin);
		}
	}
}

