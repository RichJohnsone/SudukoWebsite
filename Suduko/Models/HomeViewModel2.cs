using System;
namespace Suduko.Models
{
	public class HomeViewModel2
	{
		public Graph.Board Board { get; set; }
		public string BoardJson { get; set; } = "";

		public HomeViewModel2(Graph.Board board)
		{
			Board = board;
		}
	}
}