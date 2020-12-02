using System;
using System.Linq;
using System.IO;
using System.Collections.Generic;

namespace aco
{
    public class CaminoOptimo{
        int id;
        public List<Tuple<int,int>> arcos;
        List<int> camino;
        List<int> costos;
        int costoTotal =0;
        public int getId(){
            return this.id;
        }
        public void imprimirCamino(){
            Console.WriteLine($"Nodo Inicial : {id}");
            Program.imprimirCamino(this.camino);
            /* foreach(var i in this.costos)
            {
                Console.WriteLine($"Costo : {i}");
            } */
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
        static bool[] visitados;
        static AdjacencyList grafo;
        static List<Tuple<int,int>> arcs;
        static List<int> camino;
        static List<int> costos;
        static int costo = 0;

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
        static void inicializarArrVisitados(int n){
            visitados = new bool[n];
            for(int i =0; i < n;i++){
                visitados[i] = false;
            }
        }

        public static void imprimirCamino(List<int> camino){
            char[] letras = new char[]{'A','B','C','D','E','F','G','H','I'};
            foreach(var i in camino)
            {
                Console.WriteLine($"Nodo : {i} {letras[i]}");
            }        
        }

        static bool estanTodosVisitados(){
            //RETORNA VERDADERO CUANDO TODOS HAN SIDO VISITADOS,
            //FALSO, SI HAY AL MENOS UNO QUE SEA FALSO
            return visitados.All(x => x);
        }
        /*        //VECINO MÁS CERCANO
        * raiz -> nodo desde que se empieza la busqueda inicial
        * nodo -> nodo que continua con la busqueda
        * nodoVecino -> se manda cuando quieres que 'nodo' no busque desde el de menor costo
        */
        static int busqueda(int raiz,int nodo, int? nodoVecino = null){
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
                        //AGREGA: -NODO INICIAL AL FINAL DEL CAMINO -ARCO FINAL
                        //var a = new Tuple<int, int>(nodoInicial,nodoSiguiente);//ARCO POR EL QUE ESTA PASANDO
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
            //Console.WriteLine(visitados[nodoSiguiente]);
            //Console.WriteLine($"siguiente nodo {nodoSiguiente} . costo minimo {costoMinimo}");
            costo += costoMinimo;
            costos.Add(costoMinimo);
            var a = new Tuple<int, int>(nodoInicial,nodoSiguiente);//ARCO POR EL QUE ESTA PASANDO
            arcs.Add( a );
            return busqueda(raiz,nodoSiguiente);
        }
        static void Main(string[] args)
        {
            leerGrafo();
            grafo.mostrarListaAdyacencia();
            
            int raiz = 0;
            //SE BUSCA EN TODOS LOS VECINOS UN CAMINO
            var nodosVecinos = grafo[raiz];
            int nodoInicial = raiz;
            var res = 0;
            
            List<CaminoOptimo> caminos = new List<CaminoOptimo>();
            for(int i = 0; i < grafo.tamanioGrafo(); i++){
                raiz = i;
                camino = new List<int>();
                costos = new List<int>();
                arcs = new List<Tuple<int, int>>();
                costo = 0;
                inicializarArrVisitados(grafo.tamanioGrafo());
                visitados[raiz] = true;
                res = busqueda(raiz,raiz);
                if(res == 200){ //TRAJO UN CAMINO COMPLETO
                    caminos.Add(new CaminoOptimo(raiz,camino,costo,costos,arcs));
                }else if(res == 500){
                    return; //TERMINA PROGRAMA
                }
            }
            foreach(var i in caminos){
                i.imprimirCamino();
            }
            //CONTINUAR CON HORMIGAS
            AntColony.Init(grafo,caminos);
            Console.WriteLine("Hello World!");
        }
    }
}
