namespace Suduko.Graph
{
    public static class ExtensionMethods
    {
        public static int FirstIndex<T>(this IEnumerable<T> items, Func<T, bool> predicate)
        {
            int i = 0;
            foreach (T item in items)
            {
                if (predicate(item))
                    return i;
                i++;
            }
            throw new Exception();
        }

        public static bool IsNeighbour(this int thisCell, int cellToCheck, int boardSize)
        {
            // check above
            if (thisCell > boardSize && cellToCheck == thisCell - boardSize) return true;
            // check below
            if (thisCell < (boardSize * boardSize - boardSize) && cellToCheck == thisCell + boardSize) return true;
            // check left
            if (thisCell % boardSize != 0 && cellToCheck == thisCell - 1) return true;
            // check right
            if (thisCell % boardSize != boardSize - 1 && cellToCheck == thisCell + boardSize) return true;

            return false;
        }
    }
}