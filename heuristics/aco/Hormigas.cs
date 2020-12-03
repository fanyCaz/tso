using System;
using System.Linq;
using System.Collections.Generic;
namespace aco{
    public class AntColony{
        //static int[] nij;
        static List<Tuple<int,int>> primerosArcos = new List<Tuple<int, int>>();
        static int q = 1;      //ESTE VALOR NO CAMBIA
        static double[] inicializarNIJ(AdjacencyList g,List<CaminoOptimo> cs){
            bool[] visitados = new bool[g.tamanioGrafo()];
            char[] letras = new char[]{'A','B','C','D','E','F','G','H','I'};
            foreach(int i in Enumerable.Range(0,g.tamanioGrafo())){
                visitados[i] = false;
            }
            //OBTENER ARREGLO CON nij
            
            List<double> nij = new List<double>();
            for(int i = 0; i < g.tamanioGrafo() - 1; i++){
                visitados[i] = true;
                var vecinos = g[i];
                foreach(var j in vecinos){
                    if(!visitados[j.Item1]){
                        double x = (double)1/(double)j.Item2;
                        nij.Add(x);
                        primerosArcos.Add(new Tuple<int, int>(i,j.Item1) );
                    }
                }
            }
            
            return nij.ToArray();
        }

        /*OBTENER MATRIZ DE HORMIGAS*/
        //RECORRER LA LISTA DE CAMINOS
            //GUARDAR EN MATRIZ DE HORMIGAS
            //SI UNA HORMIGA PASA POR EL ARCO VISITADO
                //GUARDAS UN '1'
            //SI NO
                //GUARDAS UN '0'
        static List<int[]> getHormigas(int cantidadArcos, List<CaminoOptimo> cs){
            List<int[]> hormigas = new List<int[]>();
            Console.WriteLine($"ARCOS");
            for(int i=0; i < cs.Count; i++){ //cs.Count
                Console.WriteLine($"ARCOS de {cs[i].getId()}");
                int[] hormiguita = Program.fillArray(cantidadArcos,0);
                foreach(var x in cs[i].arcos){
                    //Console.WriteLine($"de {letras[x.Item1]} a {letras[x.Item2]} ");
                    bool fromTo = primerosArcos.Contains(x); //CHECA SI 'from' A 'to'
                    bool toFrom = primerosArcos.Contains(new Tuple<int, int>(x.Item2,x.Item1)); //CHECA SI 'to' A 'from'
                    int index = 0;
                    if( fromTo ){
                        index = primerosArcos.IndexOf(x);
                        hormiguita[index] = 1;
                    }else if(toFrom){
                        index = primerosArcos.IndexOf(new Tuple<int, int>(x.Item2,x.Item1));
                        hormiguita[index] = 1;
                    }
                }
                hormigas.Add(hormiguita);
                Program.imprimirArray(hormiguita);
            }
            return hormigas;
        }

        public static void Init(AdjacencyList grafo, List<CaminoOptimo> caminos){
            double[] nij = inicializarNIJ(grafo,caminos);
            List<int[]> hormigas = getHormigas(primerosArcos.Count,caminos);
        }

    }
}