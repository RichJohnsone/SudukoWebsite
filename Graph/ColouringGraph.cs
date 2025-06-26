using System;
using System.Collections.Generic;
using System.Linq;

namespace Suduko.Graph
{
    public class ColouringGraph
    {
        private sbyte numVertices;
        private List<List<sbyte>> adjList;

        public ColouringGraph(sbyte vertices)
        {
            numVertices = vertices;
            adjList = new List<List<sbyte>>(vertices);
            for (sbyte i = 0; i < vertices; i++)
            {
                adjList.Add(new List<sbyte>());
            }
        }

        public void AddEdge(sbyte u, sbyte v)
        {
            adjList[u].Add(v);
            adjList[v].Add(u);
        }

        public void AddEdges(sbyte u, List<sbyte> v)
        {
            foreach (sbyte _v in v)
                AddEdge(u, _v);
        }

        public Dictionary<sbyte, sbyte> GreedyColouring()
        {
            Dictionary<sbyte, sbyte> result = new Dictionary<sbyte, sbyte>();
            // Assign the first color to the first vertex
            result[0] = 0;

            // A helper array to store the color of each vertex
            // Initially all vertices are uncolored.
            sbyte[] availableColors = new sbyte[numVertices];
            for (sbyte i = 0; i < numVertices; i++)
            {
                availableColors[i] = -1;
            }
            availableColors[0] = 0;

            // Assign colors to remaining V-1 vertices
            for (sbyte u = 1; u < numVertices; u++)
            {
                // Process all adjacent vertices and flag their colors as unavailable
                foreach (sbyte v in adjList[u])
                {
                    if (result.ContainsKey(v))
                    {
                        availableColors[result[v]] = u;
                    }
                }

                // Find the first available color
                sbyte color;
                for (color = 0; color < numVertices; color++)
                {
                    if (availableColors[color] != u)
                    {
                        break;
                    }
                }

                result[u] = color;

                // Reset the availableColors array
                foreach (sbyte v in adjList[u])
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
