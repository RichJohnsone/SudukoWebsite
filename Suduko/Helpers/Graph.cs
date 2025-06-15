using System;
using System.Collections.Generic;
using System.Linq;

namespace Suduko.Helpers
{
    public class Graph
    {
        private int numVertices;
        private List<List<int>> adjList;

        public Graph(int vertices)
        {
            numVertices = vertices;
            adjList = new List<List<int>>(vertices);
            for (int i = 0; i < vertices; i++)
            {
                adjList.Add(new List<int>());
            }
        }

        public void AddEdge(int u, int v)
        {
            adjList[u].Add(v);
            adjList[v].Add(u);
        }

        public void AddEdges(int u, List<int> v)
        {
            foreach (int _v in v)
                AddEdge(u, _v);
        }

        public Dictionary<int, int> GreedyColouring()
        {
            Dictionary<int, int> result = new Dictionary<int, int>();
            // Assign the first color to the first vertex
            result[0] = 0;

            // A helper array to store the color of each vertex
            // Initially all vertices are uncolored.
            int[] availableColors = new int[numVertices];
            for (int i = 0; i < numVertices; i++)
            {
                availableColors[i] = -1;
            }
            availableColors[0] = 0;

            // Assign colors to remaining V-1 vertices
            for (int u = 1; u < numVertices; u++)
            {
                // Process all adjacent vertices and flag their colors as unavailable
                foreach (int v in adjList[u])
                {
                    if (result.ContainsKey(v))
                    {
                        availableColors[result[v]] = u;
                    }
                }

                // Find the first available color
                int color;
                for (color = 0; color < numVertices; color++)
                {
                    if (availableColors[color] != u)
                    {
                        break;
                    }
                }

                result[u] = color;

                // Reset the availableColors array
                foreach (int v in adjList[u])
                {
                    if (result.ContainsKey(v))
                    {
                        availableColors[result[v]] = -1;
                    }
                }
            }

            return result;
        }
    }
}
