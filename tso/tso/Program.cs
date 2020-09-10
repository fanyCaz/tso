using System;
using System.Collections.Generic;
using System.Linq;

namespace tso
{
    class Program
    {
        static void MostrarAdyacencia(int[][] matrix)
        {
            for (int i = 0; i < matrix.Length; i++)
            {
                for (int j = 0; j < matrix[i].Length; j++)
                {
                    Console.Write("\t{0}", matrix[i][j]);
                }
                Console.Write("\n");
            }
        }

        static void MostrarLista(List<int> lista)
        {

            String nodos = "ABCDEFGH";
            Console.WriteLine("=====LISTA=====");
            for(int i=0; i< lista.Count; i++)
            {
                Console.Write("\t{0}", nodos[lista[i]]);
            }
            Console.Write("\n====FIN LISTA====\n");
        }

        static int[] NodoCosto(int[][] matriz, int nodoInicial, List<int> nodosRecorridos)
        {
            List<int> nodosVisitados = new List<int>();
            int min = int.MaxValue;
            Console.WriteLine("nodo inicial: {0} ",nodoInicial);
            MostrarLista(nodosRecorridos);
            for (int i = 0; i < matriz.Length; i++)
            {
                //Console.WriteLine(nodosRecorridos.Any(x => x == matriz[nodoInicial][i]));
                if (!nodosRecorridos.Contains(i)){
                    
                    if (matriz[nodoInicial][i] > 0 && matriz[nodoInicial][i] < min)
                    {
                        nodosVisitados.Add(i);
                        min = matriz[nodoInicial][i];
                    }
                }

            }

            MostrarLista(nodosRecorridos);
            var ultNodo = (nodosVisitados.Count > 0) ? nodosVisitados.Last() : -1;
            return new int[] { min, ultNodo };    //regresa costo, siguiente nodo
        }
        static void Main(string[] args)
        {
            int[][] matrix = new int[][] {  new int[] {0,3,5,2,0,0,0,10},
                                        new int[] {3,0,5,8,4,0,6,6},
                                        new int[] {5,5,0,0,1,7,9,0},
                                        new int[] {2,8,0,0,12,0,0,14},
                                        new int[] {0,4,1,12,0,0,15,0},
                                        new int[] {0,0,7,0,0,0,0,9},
                                        new int[] {0,6,9,0,15,0,0,3},
                                        new int[] {10,6,0,14,0,9,3,0} };

            List<int> nodosRecorridos = new List<int>();
            int nodoInicial = 3, distancia = 0 ,nodoFinal=int.MinValue;
            int costo = 0, i = 0, auxiliar=nodoInicial;
            int[] res;
            nodosRecorridos.Add(nodoInicial);
            while (i < 8)
            {
                Console.WriteLine("PRIMER PASO\n");
                res = NodoCosto(matrix, auxiliar, nodosRecorridos);
                nodoFinal = res[1];
                costo += res[0];
                nodosRecorridos.Add(nodoFinal);
                i++;
                Console.WriteLine("Resultados costoTotal: {0} - nodoFinal: {1} - min: {2}\n", costo, nodoFinal, res[0]);
            }
            //Console.WriteLine("SEGUNDO PASO\n");
            //res = NodoCosto(matrix, nodoFinal, nodosRecorridos);
            //nodoFinal = res[1];
            //nodosRecorridos.Add(nodoFinal);
            //costo += res[0];
            //Console.WriteLine("Resultados {0} - {1}\n", costo, nodoFinal);
            //Console.WriteLine("TERCER PASO\n");
            //res = NodoCosto(matrix, nodoFinal, nodosRecorridos);
            //nodoFinal = res[1];
            //nodosRecorridos.Add(nodoFinal);
            //costo += res[0];
            //Console.WriteLine("Resultados {0} - {1}\n", costo, nodoFinal);

            //for (int j = 0; j < nodosRecorridos.Count(); j++)
            //{
            //    Console.Write("\theyy {0}", nodosRecorridos[j]);
            //}
        }
    }
}
