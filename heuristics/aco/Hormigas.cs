using System;
using System.Linq;
using System.Collections.Generic;
namespace aco{
     /*    public class Arco{
        public Tuple<int,int> arco;
        public int costo;
        public Arco(Tuple<int,int> a, int c){
            this.arco = a;
            this.costo = c;
        }

    } */
    public class AntColony{
        //static int[] nij;
        static List<Tuple<int,int>> primerosArcos = new List<Tuple<int, int>>();
        static int[] costosPrimerosArcos;
        static int q = 1;      //ESTE VALOR NO CAMBIA
        static double[] getNIJ(AdjacencyList g,List<CaminoOptimo> cs){
            bool[] visitados = new bool[g.tamanioGrafo()];
            char[] letras = new char[]{'A','B','C','D','E','F','G','H','I'};
            foreach(int i in Enumerable.Range(0,g.tamanioGrafo())){
                visitados[i] = false;
            }
            //OBTENER ARREGLO CON nij
            List<int> tmpCosts = new List<int>();
            List<double> nij = new List<double>();
            for(int i = 0; i < g.tamanioGrafo() - 1; i++){
                visitados[i] = true;
                var vecinos = g[i];
                foreach(var j in vecinos){
                    if(!visitados[j.Item1]){
                        double x = (double)1/(double)j.Item2;
                        nij.Add(x);
                        primerosArcos.Add(new Tuple<int, int>(i,j.Item1) );
                        tmpCosts.Add(j.Item2);
                    }
                }
            }
            costosPrimerosArcos = tmpCosts.ToArray();
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
                //Program.imprimirArray(hormiguita);
            }
            return hormigas;
        }

        /*
        * h -> matriz de hormigas
        * cantHormigas -> pues eso, am i right?
        */
        static int[] getCostos(List<int[]> h,int cantHormigas){
            int[] costos = new int[cantHormigas];
            for(int i = 0; i < cantHormigas; i++){
                costos[i] = h[i].Zip(costosPrimerosArcos,(ho,co)=> ho*co).Sum();
            }
            return costos;
        }

//
        static List<double[]> getdtijk(int cantArcos, int cantHormigas, List<int[]> h, int[] costosHormigas){
            List<double[]> deltaArcos = new List<double[]>();
            for(int j = 0; j < cantArcos; j++){
                double[] tmpArc = new double[cantHormigas];
                Console.WriteLine($"deltassss");
                for(int i = 0; i < cantHormigas; i++){
                    int m = (int)h[i].GetValue(j);
                    double x = ((double)q/(double)costosHormigas[i])*m;
                    tmpArc[i] = x;
                    //Console.WriteLine($"{j} -{i} valor: {h[i].GetValue(j)} val : {x} q:{q}");
                }
                deltaArcos.Add(tmpArc);
            }
            return deltaArcos;
        }

        public static void Init(AdjacencyList grafo, List<CaminoOptimo> caminos){
            double[] nij = getNIJ(grafo,caminos);
            List<int[]> hormigas = getHormigas(primerosArcos.Count,caminos);
            int[] costos = getCostos(hormigas,hormigas.Count);      //Costos de viajes de estas hormigas
            List<double[]> deltaArcos = getdtijk(nij.Length,hormigas.Count,hormigas,costos);
            //Program.imprimirArray(costos);
        }

    }
}