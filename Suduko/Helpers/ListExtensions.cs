using System;
namespace Suduko.Helpers
{
	public static class ListExtensions
	{
        /// <summary>
        /// Shuffles the element order of the specified list.
        /// </summary>
        public static void Shuffle<T>(this IList<T> listOfThings)
        {
            listOfThings = listOfThings.OrderBy(i => Guid.NewGuid()).ToList();
        }
    }
}