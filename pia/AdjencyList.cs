using System;
using System.Collections.Generic;

//usa tuples
namespace pia
{
    public class Vertice{
        public int source;
        public int sink;
        public int capacidad;
        public int flujo;
        public Vertice(int s,int t, int c, int f){
            this.source = s;
            this.sink = t;
            this.capacidad = c;
            this.flujo = f;
        }
    }
    public class AdjacencyListCapacity
    {
        LinkedList<Vertice>[] adjList;
        public AdjacencyListCapacity(int vertices)
        {
            adjList = new LinkedList<Vertice>[vertices];

            for (int i = 0; i < adjList.Length; ++i)
            {
                adjList[i] = new LinkedList<Vertice>();
            }
        }

        public void agregaVertice(int inicial,int final,int capacidad, int flujo)
        {
            adjList[inicial].AddLast(new Vertice(inicial,final,capacidad,flujo));
        }

        public LinkedList<Vertice> this[int index]
        {
            get
            {
                LinkedList<Vertice> edgeList
                               = new LinkedList<Vertice>(adjList[index]);
 
                return edgeList;
            }
        }

        public void mostrarListaAdyacencia()
        {
            int i = 0;
            foreach (LinkedList<Vertice> list in adjList)
            {
                Console.Write("adjacencyList[" + i + "] -> ");
 
                foreach (Vertice vertice in list)
                {
                    Console.Write(vertice.sink + "(" + vertice.capacidad + ")");
                }
                ++i;
                Console.WriteLine();
            }
        }
    }

    //ADJACENCY LIST NodoInicial->[nodoFinal, peso] , ...
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
        public int tamanioGrafo(){
            return adjList.Length;
        }
    }
}