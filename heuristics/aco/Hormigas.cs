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
        static char[] letras = new char[]{'A','B','C','D','E','F','G','H','I'};
        static int q = 1;      //ESTE VALOR NO CAMBIA
        static double[] deltatij;
        static double[] feromonas;
        static double[] probabilidadArco;
        static double rho = 0.2;
        static int a = 1,b = 1;
        static double[] getNIJ(AdjacencyList g,List<CaminoOptimo> cs){
            bool[] visitados = new bool[g.tamanioGrafo()];
            
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
        static List<double[]> getdtijk(int cantArcos, int cantHormigas, List<int[]> h, int[] costosHormigas,bool first = true){
            deltatij = new double[cantArcos];
            feromonas = new double[cantArcos];
            probabilidadArco = new double[cantArcos];
            List<double[]> deltaArcos = new List<double[]>();
            for(int j = 0; j < cantArcos; j++){
                double[] tmpArc = new double[cantHormigas];
                for(int i = 0; i < cantHormigas; i++){
                    int m = (int)h[i].GetValue(j);
                    double x = ((double)q/(double)costosHormigas[i])*m;
                    tmpArc[i] = x;
                    //Console.WriteLine($"{j} -{i} valor: {h[i].GetValue(j)} val : {x} q:{q}");
                }
                deltaArcos.Add(tmpArc);
                deltatij[j] = tmpArc.Sum();
                var r = (first) ? 0 : rho;
                feromonas[j] = (1-r)*deltatij[j];
                probabilidadArco[j] = Math.Pow(feromonas[j],a);
            }
            return deltaArcos;
        }

        /*
        * cantArcos -> cantidad arcos iniciales
        * cantHormigas -> cantidad de hormigas
        * nij -> niu
        * h -> lista de hormigas
        */
        static List<double[]> getNuevaMatriz(int cantArcos,int cantHormigas,double[] nij,List<int[]> h){
            List<double[]> matriz = new List<double[]>();
            for(int i = 0; i < cantArcos; i++){
                //Console.WriteLine($"CAMINO NUEVO DE {i}");
                double[] tmpRow = new double[cantHormigas];
                for(int j = 0; j < cantHormigas; j++){
                    double nijToB = Math.Pow(nij[i],b);
                    double caminoxferomona = h[j].Zip(feromonas,(ho,fe)=>ho*fe).Sum();  //SUMA PRODUCTOf
                    var x = (Math.Pow(probabilidadArco[i],a))*(nijToB) / (nijToB * caminoxferomona);
                    x *= (int)h[j].GetValue(i);
                    tmpRow[j] = x;
                    //Console.WriteLine($"val : {x}");
                    //Console.WriteLine($"nij {nij[i]} probabilidades {probabilidadArco[i]} a {a} b {b} h {h[j].GetValue(4)} val :{x}");
                }
                matriz.Add(tmpRow);
            }
            return matriz;
        }
        static void generarNuevosCaminos(int cantArcos, int cantHormigas,List<double[]> matriz){
            /* for(int j = 0; j < matriz[0].Length; j++){
                Console.WriteLine($"nueva filea");
                for(int i = 0; i < matriz.Count; i++){
                    //Console.WriteLine($"{primerosArcos[i]}");
                    Console.WriteLine($"{matriz[i].GetValue(j)}");
                }
            } */

            bool[] visitados = new bool[cantHormigas];
            bool[] arcosVisitados = new bool[cantArcos];
            
            for(int j = 0; j < cantHormigas; j++){  //CICLO PARA CAMBIO DE COLUMNAS
                int columna = j;
                int nodo = columna;
                int nodoSiguiente = 0;//valor x por ahora
                Console.WriteLine("nueva columna");
                foreach(int i in Enumerable.Range(0,cantHormigas)){
                    visitados[i] = false;
                }
                for(int i = 0; i < cantArcos; i++){
                    arcosVisitados[i] = false;
                }
                for(int i = 0; i < cantHormigas; i++){  //CICLO PARA BÚSQUEDA DE CAMINO EN UNA COLUMNA
                    visitados[nodo] = true;
                    var x  =  primerosArcos.Where(x => x.Item1 == nodo || x.Item2 == nodo);   //OBTIENE TODOS LOS ARCOS CON ESE NODO DE 'FROM' A 'TO'
                    double valMaximo = double.MinValue; int indexNodo = int.MinValue;         //valor e indice del arco por el que se movera
                    //Console.WriteLine($"Nodo que busca en arcos :{nodo}"); 
                    foreach(var pA in x ){      //CICLO EN LOS ARCOS QUE CONTENGAN EL ARCO QUE BUSCO PARA PASAR
                        var y = primerosArcos.IndexOf(pA);      //INDEX DEL ARCO EN QUE ESTAMOS COMPARANDO
                        double valorPr = (double)matriz[y].GetValue(columna);
                        //Console.WriteLine($"v: {valorPr}");
                        if(valorPr > valMaximo && !arcosVisitados[y]){
                            valMaximo = valorPr;
                            indexNodo = y;
                        }
                    }
                    arcosVisitados[indexNodo] = true;
                    //CONOCER A QUE NODO PERTENECE LA SIGUIENTE VUELTA
                    //Console.WriteLine($"index Nodo : {indexNodo} ");
                    nodoSiguiente = (!visitados[primerosArcos[indexNodo].Item1]) ? primerosArcos[indexNodo].Item1 : primerosArcos[indexNodo].Item2;
                    /*Console.WriteLine($"valor de probabilidad: {matriz[indexNodo].GetValue(columna)}  \n"
                        + $" arco: {letras[primerosArcos[indexNodo].Item1]} y {letras[primerosArcos[indexNodo].Item2]} \n"
                        + $" nodoSiguiente: {letras[nodoSiguiente]}");
                    Console.WriteLine($"val minomo : {valMaximo}");*/
                    nodo = nodoSiguiente;
                }
                Program.imprimirArrayBool(arcosVisitados);
            }
        }
        public static void Init(AdjacencyList grafo, List<CaminoOptimo> caminos){
            double[] nij = getNIJ(grafo,caminos);
            List<int[]> hormigas = getHormigas(primerosArcos.Count,caminos);
            int[] costos = getCostos(hormigas,hormigas.Count);      //Costos de viajes de estas hormigas
            List<double[]> deltaArcos = getdtijk(nij.Length,hormigas.Count,hormigas,costos);
            /* Console.WriteLine("deltas");
            Program.imprimirArrayDouble(deltatij);
            Console.WriteLine("feromonas");
            Program.imprimirArrayDouble(feromonas);
            Console.WriteLine("probabilidades");
            Program.imprimirArrayDouble(probabilidadArco); */
            List<double[]> matrizCaminosNuevos = getNuevaMatriz(nij.Length,hormigas.Count,nij,hormigas);
            generarNuevosCaminos(nij.Length,hormigas.Count,matrizCaminosNuevos);
        }
    }
}