using System;
using System.Linq;
using System.Collections.Generic;
namespace aco{
    public class AntColony{
        //static int[] nij;
        int[] hormigas;
        static double[] inicializarNIJ(AdjacencyList g,List<CaminoOptimo> cs){
            bool[] visitados = new bool[g.tamanioGrafo()];
            char[] letras = new char[]{'A','B','C','D','E','F','G','H','I'};
            foreach(int i in Enumerable.Range(0,g.tamanioGrafo())){
                visitados[i] = false;
            }
            //OBTENER ARREGLO CON nij
            List<double> nij = new List<double>();
            List<int> hormiguita = new List<int>();
            for(int i = 0; i < g.tamanioGrafo() - 1; i++){
                visitados[i] = true;
                var vecinos = g[i];
                foreach(var j in vecinos){
                    if(!visitados[j.Item1]){
                        Console.WriteLine($"arco {letras[i]} - {letras[j.Item1]} ");
                        double x = (double)1/(double)j.Item2;
                        nij.Add(x);
                        
                    }
                }
            }
            double[] nijArr = nij.ToArray();
            //DESPUES DE ESTO, PARA QUE SEPAS EL LARGO DEL ARREGLO,,, 
            //O PUEDE SER ARRIBA CUANDO SACAS NIJ PORQUE YA ESTA EL ARCO QUE SE ESTA COMPARANDO
            //LLENAR MATRIZ DE ARCOS POR LOS QUE PASO UNA HORMIGA
                        //SI i HA Y EL NODO ACTUAL EXISTEN EN LOS ARCOS EXISTENTES EN ESTE CAMINO
                            //PON '1' EN EL ESPACIO i DEL ARREGLO
                        /* if(i == cs[0].arcos[0]){

                        } */
            //for(int i = 0; i < nijArr.Length; i++){
                var h = cs[0].arcos[0];
                foreach(var m in h){
                    Console.WriteLine( m.Key );
                    Console.WriteLine( m.Value );
                }
                
            
            return nijArr;
        }

        public static void Init(AdjacencyList grafo, List<CaminoOptimo> caminos){
            double[] nij = inicializarNIJ(grafo,caminos);
            
        }

    }
}