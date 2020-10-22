using System;
using System.Collections.Generic;

//usa tuples
namespace flujomaximo
{
    public class AdjacencyList
    {
        LinkedList<Tuple<int, int>>[] adjList;
        public AdjacencyList(int vertices)
        {
            adjList = new LinkedList<Tuple<int, int>>[vertices];

            for (int i = 0; i < adjList.Length; ++i)
            {
                adjList[i] = new LinkedList<Tuple<int, int>>();
            }
        }

        public void agregaVertice(int startVertex, int endVertex, int weight)
        {
            adjList[startVertex].AddLast(new Tuple<int, int>(endVertex, weight));
        }

        public LinkedList<Tuple<int, int>> this[int index]
        {
            get
            {
                LinkedList<Tuple<int, int>> edgeList
                               = new LinkedList<Tuple<int, int>>(adjList[index]);
 
                return edgeList;
            }
        }

        public void mostrarListaAdyacencia()
        {
            int i = 0;
            foreach (LinkedList<Tuple<int, int>> list in adjList)
            {
                Console.Write("adjacencyList[" + i + "] -> ");
 
                foreach (Tuple<int, int> vertice in list)
                {
                    Console.Write(vertice.Item1 + "(" + vertice.Item2 + ")");
                }
                ++i;
                Console.WriteLine();
            }
        }
    }
}