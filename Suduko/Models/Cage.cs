using System;
using Newtonsoft.Json;

namespace Suduko.Models
{
	public class Cage
	{
        public int Id { get; set; }
        public int Value => Cells.Sum(c => c.Value); // sum of cell values
        public int Solution => Cells.Sum(c => c.Solution); // sum of cell solutions
        public int Size => Cells.Count; // number of cells
		[JsonIgnore]
        public List<Cell> Cells { get; set; } = new();
		public string Colour = string.Empty;
        [JsonIgnore]
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

