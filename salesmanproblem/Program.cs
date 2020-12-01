using System;
using System.Linq;
using System.Collections.Generic;

namespace salesmanproblem
{
    class Program
    {
        static bool[] visitados;
        static AdjacencyList grafo;
        static List<int> camino;
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
        static int veces = 10;
        static int busqueda(int raiz,int nodo, int? nodoVecino = null){
            // veces--;
            // System.Console.WriteLine($"veces {veces}");
            //llego a un limite (no tiene vecinos restantes) 
            //y ya no puede seguir ese caminio, no es feasible
            //modificar a que cheque si el siguiente es el nodo inicial
            /* if(nodo == -1){
                Console.WriteLine( estanTodosVisitados() );
                return nodo;
            } */

            int nodoInicial = nodo;
            visitados[nodoInicial] = true;
            camino.Add(nodoInicial);
            var vecinos = grafo[nodoInicial];
            int costoMinimo = int.MaxValue;
            int nodoSiguiente = -1;
            if(nodoVecino is int){
                System.Console.WriteLine("si es entero");
                //return 1;
                int nv = nodoVecino ?? default(int);
                return busqueda(raiz,nv);
            }
            foreach(var i in vecinos){
                if(!visitados[i.Item1]){
                    //Console.WriteLine($" nodo : {i.Item1} costo: {i.Item2} visit? {costoMinimo}");
                    if(i.Item2 < costoMinimo){
                        costoMinimo = i.Item2;
                        nodoSiguiente = i.Item1;
                    }
                }
            }
            Console.WriteLine($" nodo siguiente : {nodoSiguiente}");
            imprimirCamino(camino);
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
                        return 200;
                    }
                }
                //HACER
                return 500;//RETURN "NO FEASIBLE"
            }
                
                    
                
            return busqueda(raiz,nodoSiguiente);
        }

        static void Main(string[] args)
        {
            grafo = CyclicGraph8Nodes();
            camino = new List<int>();
            inicializarArrVisitados(8);
            int nodoInicial = 3;
            //PRIMERA VEZ QUE SE MANDA A LLAMAR
            var res = busqueda(nodoInicial,nodoInicial);
            System.Console.WriteLine($"response {res}");
            if(res == -1){      //NECESITAS HACER OTRA VEZ LA BUSQUEDA
                inicializarArrVisitados(8);
                camino.Clear();
                res = busqueda(nodoInicial,nodoInicial, 1);
                System.Console.WriteLine($"response en menos uno{res}");
            }
            //}
            // Console.WriteLine("segunda vuelta");
            // visitados[nodoSiguiente] = true;
            // camino.Add(nodoSiguiente);
            // vecinos = grafo[nodoSiguiente];
            // costoMinimo = int.MaxValue;
            // nodoSiguiente = -1;
            // foreach(var i in vecinos){
            //     if(!visitados[i.Item1]){
            //         Console.WriteLine($" nodo : {i.Item1} costo: {i.Item2} visit? {costoMinimo}");
            //         if(i.Item2 < costoMinimo){
            //             costoMinimo = i.Item2;
            //             nodoSiguiente = i.Item1;
            //         }
            //     }
            // }
            // imprimirCamino(camino);

            Console.WriteLine("Hello World!");
        }
    }
}
