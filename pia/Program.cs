using System;
using System.IO;
using System.Linq;
using System.Collections.Generic;

namespace pia
{
    public class CaminoOptimo{
        int id;
        public List<Tuple<int,int>> arcos;
        List<int> camino;
        public List<int> costos;
        int costoTotal =0;
        public int getId(){
            return this.id;
        }
        public void imprimirCamino(){
            Console.WriteLine($"Nodo Inicial : {id}");
            Program.imprimirCamino(this.camino);
            foreach(var i in this.costos)
            {
                Console.WriteLine($"Costo : {i}");
            }
            foreach (var i in this.arcos)
            {
                Console.WriteLine($"de {i.Item1} a {i.Item2}");
            }
            Console.WriteLine($"Costo de recorrido : {costoTotal}");
        }
        public void actualizaCosto(int c ){
            this.costoTotal += c;
        }
        public CaminoOptimo(int id,List<int> c, int cost, List<int> costs, List<Tuple<int,int>> arcs){
            this.camino = c;
            this.costos = costs;
            this.costoTotal = cost;
            this.id = id;
            this.arcos = arcs;
        }
    }
    
    class Program
    {
        static AdjacencyList grafo;
        static void leerGrafo(){
            string archivoTxt = Path.Combine(Directory.GetCurrentDirectory(),"adyacenciaCiclado.txt");
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
        public void construirMemeplexes(int ranas){
            List<int[]> memeplex = new List<int[]>();
            for(int i = 0; i < ranas; i++){
                var tmp = ;
                memeplex.Add(tmp);
            }
        }
        
        /*VECINO MÁS CERCANO
            * raiz -> nodo desde que se empieza la busqueda inicial
            * nodo -> nodo que continua con la busqueda
            * nodoVecino -> se manda cuando quieres que 'nodo' no busque desde el de menor costo
        */
        static int busqueda(int raiz,int nodo, int? nodoVecino = null, bool[] visitados){
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
            if( estanTodosVisitados() ){
                //CHECAR SI EL NODO INICIAL DEL GRAFO ESTA ENTRE LOS VECINOS
                var vecino = grafo[nodo];
                foreach(var i in vecinos){
                    //SI ESTA
                    //RETURN "FEASIBLE"
                    if(i.Item1 == raiz){
                        //AGREGA: -NODO INICIAL AL FINAL DEL CAMINO -ARCO FINAL -COSTO DEL ULTIMO ARCO
                        costos.Add(i.Item2);
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
            costos.Add(costoMinimo);//PARA COSTO DE ARCO
            var a = new Tuple<int, int>(nodoInicial,nodoSiguiente);//ARCO POR EL QUE ESTA PASANDO
            arcs.Add( a );
            return busqueda(raiz,nodoSiguiente);
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
            bool[] visitados = fillArrayBool();
            /**/
            int m = 4;
            int f = 200;
            int n = f/m;
            Console.WriteLine($" Numero de ranas : {f} .\n Numeros memeples {m} \n Numero de ranas por memeplex {n}");




        }
    }
}
