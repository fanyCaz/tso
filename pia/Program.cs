using System;
using System.IO;
using System.Linq;
using System.Collections.Generic;

namespace pia
{
    public class Rana{
        int id;
        List<int[]> caminos;
        public int fitness;
        public int getId(){
            return this.id;
        }
        public void imprimirCamino(){
            Console.WriteLine($"Rana : {id}");
            foreach(var i in this.caminos){
                for(int j = 0; j < i.Length; j++){
                    Console.Write($"{i[j]}");
                }
                Console.WriteLine("");
            }
        }
        
        public Rana(int id,List<int[]> c){
            this.id = id;
            this.caminos = c;
        }
    }
    
    class Program
    {
        static AdjacencyList grafo;
        static List<Tuple<int,int>> arcs = new List<Tuple<int, int>>();
        static List<int> camino = new List<int>();
        //static List<int> costos = new List<int>();
        static int costo;
        
        static AdjacencyList leerGrafo(){
            AdjacencyList grafo;
            string archivoTxt = Path.Combine(Directory.GetCurrentDirectory(),"adyacenciaCompleto.txt");
            string[] lines = File.ReadAllLines(archivoTxt);
            Console.WriteLine(lines.Length);
            grafo = new AdjacencyList( int.Parse(lines[0]) );
            foreach(var i in lines){
                string[] j = i.Split(',');
                int x = int.Parse(j[0]);
                try{
                    grafo.agregaVertice(int.Parse(j[0]),int.Parse(j[1]),int.Parse(j[2]));
                }catch(Exception){      //PARA EVITAR LA PRIMERA LINEA QUE ES EL NUMERO DE NODOS
                    continue;
                }
            }
            return grafo;
        }
        /*
        *   cant -> tamaño del array
        *   n -> numero con que se llenara el array
        */
        public static int[] fillArray(int cant,int n){
            int[] array = new int[cant];
            for(int i=0; i < cant;i++){
                array[i] = n;
            }
            return array;
        }
        public static bool[] fillArrayBool(int cant){
            bool[] array = new bool[cant];
            for(int i=0; i < cant;i++){
                array[i] = false;
            }
            return array;
        }
        public static void imprimirArray(int[] array){
            for(int i = 0; i < array.Length; i++){
                Console.WriteLine($"{array[i]}");
            }
        }
        public static void imprimirArrayDouble(double[] array){
            for(int i = 0; i < array.Length; i++){
                Console.WriteLine($"{array[i]}");
            }
        }
        public static void imprimirArrayBool(bool[] array){
            for(int i = 0; i < array.Length; i++){
                Console.WriteLine($"{array[i]}");
            }
        }
        static void inicializarArrVisitados(int n){
            /* visitados = new bool[n];
            for(int i =0; i < n;i++){
                visitados[i] = false;
            } */
        }

        public static void imprimirCamino(List<int> camino){
            char[] letras = new char[]{'A','B','C','D','E','F','G','H','I'};
            foreach(var i in camino)
            {
                Console.WriteLine($"Nodo : {i} {letras[i]}");
            }
        }
        static int calcCosto(){
            return 1;
        }
        static int getFitness(int costo){
            return 1/costo;
        }
        static bool estanTodosVisitados(bool[] visitados){
            //RETORNA VERDADERO CUANDO TODOS HAN SIDO VISITADOS,
            //FALSO, SI HAY AL MENOS UNO QUE SEA NO HAYA SIDO VISITADO
            return visitados.All(x => x);
        }
        /*VECINO MÁS CERCANO
            * raiz -> nodo desde que se empieza la busqueda inicial
            * nodo -> nodo que continua con la busqueda
        */
        static int busqueda(int raiz,int nodo, bool[] visitados){
            int nodoInicial = nodo;
            visitados[nodoInicial] = true;
            camino.Add(nodoInicial);
            
            var vecinos = grafo[nodoInicial];
            int costoMinimo = int.MaxValue;
            int nodoSiguiente = -1;
            foreach(var i in vecinos){
                
                if(!visitados[i.Item1]){
                    //Console.WriteLine($" nodo : {i.Item1} costo: {i.Item2} costoMinimo? {costoMinimo}");
                    if(i.Item2 < costoMinimo){
                        costoMinimo = i.Item2;
                        nodoSiguiente = i.Item1;
                    }
                }
            }
            
            //RETORNA 200 SI ES FEASIBLE
            //RETORNA 500 SI NO FEASIBLE
            //ANTES DE HACER BUSQUEDA DE NUEVO , HAY QUE CHECAR SI TODOS LOS VECINOS ESTAN VISITADOS
            //SI TODOS LOS VECINOS VISITADOS
            if( estanTodosVisitados(visitados) ){
                //CHECAR SI EL NODO INICIAL DEL GRAFO ESTA ENTRE LOS VECINOS
                var vecino = grafo[nodo];
                foreach(var i in vecinos){
                    //SI ESTA
                    //RETURN "FEASIBLE"
                    if(i.Item1 == raiz){
                        //AGREGA: -NODO INICIAL AL FINAL DEL CAMINO -ARCO FINAL -COSTO DEL ULTIMO ARCO
                        //costos.Add(i.Item2);
                        arcs.Add( new Tuple<int, int>(nodo,raiz) );
                        camino.Add(raiz);
                        return 200;
                    }
                }
                Console.WriteLine("NO ES UN GRAFO COMPLETAMENTE CONECTADO");
                return 500;//RETURN "NO FEASIBLE"
            }
            
            if(nodoSiguiente == -1){
                Console.WriteLine("NO SE COMPLETA EL CAMINO");
                return 500;
            }
            costo += costoMinimo;   //PARA COSTO TOTAL
            //costos.Add(costoMinimo);//PARA COSTO DE ARCO
            var a = new Tuple<int, int>(nodoInicial,nodoSiguiente);//ARCO POR EL QUE ESTA PASANDO
            arcs.Add( a );
            return busqueda(raiz,nodoSiguiente,visitados);
        }
    
        static void Main(string[] args)
        {
            //Initialize parameters
            //f -> number of frogs
            //m -> number of memeplexes, f is distributed in these memeplexes
            //Each memeplex consists of n frogs that F = mn
            //f(i) -> fitness
            //n -> number of frogs in each memeplex
            //q -> number of frogs in a submemeplex
            //Px -> position of a frog
            //PB -> best solution
            //PW -> worst solution
            //S  -> the updated step size of the frog-leaping
            //Smax -> maximum step size allowed by a frog after being affected
            //i -> number of iterations
            //wth is U(i) -> vector de la hormiga i
            //Variables miscelaneas
            grafo = leerGrafo();
            bool[] visitados = fillArrayBool(grafo.tamanioGrafo());
            List<int[]> caminos = new List<int[]>();
            List<int> costos = new List<int>();
            //List<CaminoOptimo> caminos = new List<CaminoOptimo>();
            /*Variables algoritmo*/
            int m = 6;
            int f = 60;
            int n = f/m;
            int cantNodos = grafo.tamanioGrafo();
            Console.WriteLine($"{cantNodos}  Numero de ranas : {f} .\n Numeros memeples {m} \n Numero de ranas por memeplex {n}");

            //3.1 POSITION OF INDIVIDUAL FROG
            //CAMINOS OPTIMOS POSIBLES
            //int[] camino = new int[cantNodos];
            //OBTENER TODOS LOS CAMINOS POSIBLES
            //DE MANERA ALEATORIA ASIGNARLOS A DIFERENTES RANAS
            //CADA RANA AL PRINCIPIO TENDRÁ UN CAMINO IGUAL ->CADA SUBMEMEPLEX SERÁ IGUAL
            caminos.Clear();
            for(int i = 0; i < cantNodos; i++){
                camino.Clear();
                costo = 0;
                visitados = fillArrayBool(cantNodos);
                int res = busqueda(i,i,visitados);
                costos.Add(costo);
                if(res == 200){
                    caminos.Add(camino.ToArray() );
                }
            }
            Random rnd = new Random();
            Rana[] ranas = new Rana[f];
            //GENERA F CANTIDAD DE RANAS
            for(int i = 0; i < f; i++){
                //a cada rana, asignarle una lista de caminos
                int indexCamino = rnd.Next( caminos.Count() );
                var caminosRana = new List<int[]>();
                    caminosRana.Add(caminos[indexCamino].ToArray());
                ranas[i] = new Rana(i,caminosRana);
                //EVALUAR EL FITNESS
                ranas[i].fitness = getFitness(costos[indexCamino]);
            }
            List<Rana[]> memeplexes = new List<Rana[]>();
            
            //CONSTRUIR M MEMEPLEXES
            int cont = 0;
            for(int i = 0; i < m; i++){
                Rana[] tmpR = new Rana[n];
                for(int j = 0; j < n; j++){
                    tmpR[j] = ranas[cont];
                    cont++;
                }
                memeplexes.Add(tmpR);
            }
            //var rOrdered = ranas.OrderBy(x => x.fitness);

            foreach(var rns in memeplexes){
                Console.WriteLine($"memeplex ");
                foreach(var rn in rns){
                    rn.imprimirCamino();
                }
            }
        }
    }
}
