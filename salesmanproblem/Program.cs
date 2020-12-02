using System;
using System.Linq;
using System.IO;
using System.Threading;
using System.Collections.Generic;

namespace salesmanproblem
{
    class Program
    {
        static bool[] visitados;
        static AdjacencyList grafo;
        static List<int> camino;
        static int costo = 0;
        //7->0,3,1->2,6->4,5
        public static AdjacencyList CyclicGraph8Nodes(){
            AdjacencyList h = new AdjacencyList(8);
            h.agregaVertice(0,1,3);
            h.agregaVertice(0,2,5);
            h.agregaVertice(0,3,2);
            h.agregaVertice(0,7,10);
            h.agregaVertice(1,0,3);
            h.agregaVertice(1,2,5);
            h.agregaVertice(1,4,4);
            h.agregaVertice(1,6,6);
            h.agregaVertice(1,3,8);
            h.agregaVertice(1,7,6);
            h.agregaVertice(2,0,5);
            h.agregaVertice(2,1,5);
            h.agregaVertice(2,6,9);
            h.agregaVertice(2,4,1);
            h.agregaVertice(2,5,7);
            h.agregaVertice(3,7,14);
            h.agregaVertice(3,0,2);
            h.agregaVertice(3,1,8);
            h.agregaVertice(3,4,12);
            h.agregaVertice(4,2,1);
            h.agregaVertice(4,1,4);
            h.agregaVertice(4,3,12);
            h.agregaVertice(4,6,15);
            h.agregaVertice(5,1,7);
            h.agregaVertice(5,7,9);
            h.agregaVertice(6,1,6);
            h.agregaVertice(6,2,9);
            h.agregaVertice(6,4,15);
            h.agregaVertice(6,7,3);
            h.agregaVertice(7,0,10);
            h.agregaVertice(7,5,9);
            h.agregaVertice(7,1,6);
            h.agregaVertice(7,6,3);
            h.agregaVertice(7,3,14);
            h.mostrarListaAdyacencia();
            return h;
        }
        static void leerGrafo(){
            string archivoTxt = Path.Combine(Directory.GetCurrentDirectory(),"adyacencia.txt");
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
        static void inicializarArrVisitados(int n){
            visitados = new bool[n];
            for(int i =0; i < n;i++){
                visitados[i] = false;
            }
        }

        static void imprimirCamino(List<int> camino){
            char[] letras = new char[]{'A','B','C','D','E','F','G','H','I'};
            foreach(var i in camino)
            {
                Console.WriteLine($"Nodo : {i} {letras[i]}");
            }        
        }

        static bool estanTodosVisitados(){
            //RETORNA VERDADERO CUANDO TODOS HAN SIDO VISITADOS,
            //FALSO, SI NO HAY AL MENOS UNO QUE SEA FALSO
            return visitados.All(x => x);
        }
        /*
        * raiz -> nodo desde que se empieza la busqueda inicial
        * nodo -> nodo que continua con la busqueda
        * nodoVecino -> se manda cuando quieres que 'nodo' no busque desde el de menor costo
        */
        //VECINO MÁS CERCANO
        static int busqueda(int raiz,int nodo, int? nodoVecino = null){
            int nodoInicial = nodo;
            visitados[nodoInicial] = true;
            camino.Add(nodoInicial);
            var vecinos = grafo[nodoInicial];
            int costoMinimo = int.MaxValue;
            int nodoSiguiente = -1;
            foreach(var i in vecinos){
                if(!visitados[i.Item1]){
                    Console.WriteLine($" nodo : {i.Item1} costo: {i.Item2} visit? {costoMinimo}");
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
            Console.WriteLine($"siguiente nodo {nodoSiguiente} . costo minimo {costoMinimo}");
            costo += costoMinimo;
            return busqueda(raiz,nodoSiguiente);
        }

        static void Main(string[] args)
        {
            leerGrafo();
            grafo.mostrarListaAdyacencia();
            //grafo = CyclicGraph8Nodes();
            camino = new List<int>();
            inicializarArrVisitados(8);
            int raiz = 1;
            //SE BUSCA EN TODOS LOS VECINOS UN CAMINO
            var nodosVecinos = grafo[raiz];
            int nodoInicial = raiz;
            var res = 0;
            camino.Clear();
            visitados[raiz] = true;
            res = busqueda(raiz,raiz);
            if(res == 200){
                imprimirCamino(camino);
                Console.WriteLine($"Costo del camino : {costo}");
            }
            Console.WriteLine("Hello World!");
        }
    }
}
/* adjacencyList[0] -> 1(3)2(5)3(2)7(10)
adjacencyList[1] -> 0(3)2(5)4(4)6(6)3(8)7(6)
adjacencyList[2] -> 0(5)1(5)6(9)4(1)5(7)
adjacencyList[3] -> 7(14)0(2)1(8)4(12)
adjacencyList[4] -> 2(1)1(4)3(12)6(15)
adjacencyList[5] -> 1(7)7(9)
adjacencyList[6] -> 1(6)2(9)4(15)7(3)
adjacencyList[7] -> 0(10)5(9)1(6)6(3)3(14) */