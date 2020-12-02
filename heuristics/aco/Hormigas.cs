using System;
using System.Linq;
using System.Collections.Generic;
namespace aco{
    public class AntColony{
        //static int[] nij;
        static List<int[]> hormigas;
        static int q = 1;      //ESTE VALOR NO CAMBIA
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
                        double x = (double)1/(double)j.Item2;
                        nij.Add(x);
                        //Console.WriteLine($"arco {letras[i]} - {letras[j.Item1]} ");
                        //Console.WriteLine($"camino de hormiga : {cs[i].getId()}");
                        //Console.WriteLine(i);
                    }
                }
            }

            var h = cs[0].arcos[0];
            Console.WriteLine($"ARCOS");
            for(int i=0; i < nij.Count; i++){
                Console.WriteLine($"arcos de {cs[i].getId()}: {cs[i].arcos.Count}");
            }
                 /* //YO QUIERO SOLO EL PRIMER ARCO DE CADA CAMINO, Y SABER SI LO VISITO LA HORMIGA
                        
                        if(cs[i].arcos[i].Item1 == i && cs[i].arcos[i].Item2 == j.Item1){
                            Console.WriteLine("son el mismo arco que emocion");
                            Console.WriteLine($"arco {letras[i]} - {letras[j.Item1]} ");
                            Console.WriteLine(cs[i].arcos[i].Item1);
                        }         */
            //double[] nijArr = nij.ToArray();
            for(int i = 0; i < nij.Count; i++){
                
                //Console.WriteLine($"{cs[i].arcos[0].Item1} - {cs[i].arcos[0].Item2}");
            }
            //RECORRER LA LISTA DE CAMINOS
                //GUARDAR EN MATRIZ DE HORMIGAS
                //SI UNA HORMIGA PASA POR EL ARCO VISITADO
                    //GUARDAS UN '1'
                //SI NO
                    //GUARDAS UN '0'
            //IMPRIMIR COSTO

            //DESPUES DE ESTO, PARA QUE SEPAS EL LARGO DEL ARREGLO,,, 
            //O PUEDE SER ARRIBA CUANDO SACAS NIJ PORQUE YA ESTA EL ARCO QUE SE ESTA COMPARANDO
            //LLENAR MATRIZ DE ARCOS POR LOS QUE PASO UNA HORMIGA
                        //SI i HA Y EL NODO ACTUAL EXISTEN EN LOS ARCOS EXISTENTES EN ESTE CAMINO
                            //PON '1' EN EL ESPACIO i DEL ARREGLO
                        /* if(i == cs[0].arcos[0]){

                        } */
            //for(int i = 0; i < nijArr.Length; i++){
                
            
            return nij.ToArray();
        }

        public static void Init(AdjacencyList grafo, List<CaminoOptimo> caminos){
            double[] nij = inicializarNIJ(grafo,caminos);
            
        }

    }
}