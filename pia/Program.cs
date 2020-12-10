using System;
using System.IO;
using System.Linq;
using System.Diagnostics;
using System.Threading;
using System.Collections.Generic;

namespace pia
{
    public class Mememplex{
        public List<Rana> ranas;
        public Mememplex(List<Rana> r){
            this.ranas = r;
        }
    }
    public class Rana{
        public List<Camino> caminos;
        public Rana(List<Camino> c){
            this.caminos = c;
        }
    }
    public class Camino{
        public int[] camino;
        public double fitness;
        public int costo;
        public void setFitness(double fit){
            this.fitness = fit;
        }
        public void setCosto(int cst){
            this.costo = cst;
        }
        public Camino(int[] c,double f, int cost){
            this.camino = c;
            this.fitness = f;
            this.costo = cost;
        }
    }
    
    class Program
    {
        static AdjacencyList grafo;
        static List<Tuple<int,int>> arcs = new List<Tuple<int, int>>();
        static List<int> camino = new List<int>();
        static AdjacencyList leerGrafo(){
            AdjacencyList grafo;
            string archivoTxt = Path.Combine(Directory.GetCurrentDirectory(),"adyacenciaExamen.txt");
            string[] lines = File.ReadAllLines(archivoTxt);
            //Console.WriteLine(lines.Length);
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
                Console.Write($"{array[i]} -");
            }
            Console.WriteLine("");
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

        public static void imprimirCamino(Camino camino){
            char[] letras = new char[]{'A','B','C','D','E','F','G','H','I'};
            Console.WriteLine($"camino{camino.ToString()}");
            Console.WriteLine($"fitness {camino.fitness}");
            foreach(var i in camino.camino)
            {
                Console.Write($"{letras[i]} - ");
            }
        }
        /*
        * Hace recorrido del grafo para obtener costos de nodos
        * basado en los arcos por los que pasó
        */
        static int getCosto(int[] camino){
            int costoCamino = 0;
            for(int i = 0; i < camino.Length - 1; i++){
                var vecinos = grafo[camino[i]];
                var x = vecinos.Where(x => x.Item1 == camino[i+1]);
                costoCamino += x.First().Item2;
            }
            //Console.WriteLine($"costo {costoCamino}");
            return costoCamino;
        }
        static double getFitness(double costo){
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
        static int[] busqueda(int raiz,int nodo, bool[] visitados,int costo){
            int nodoInicial = nodo;
            visitados[nodoInicial] = true;
            camino.Add(nodoInicial);
            var vecinos = grafo[nodoInicial];
            int costoMinimo = int.MaxValue;
            int nodoSiguiente = -1;
            foreach(var i in vecinos){
                if(!visitados[i.Item1]){
                    if(i.Item2 < costoMinimo){
                        costoMinimo = i.Item2;
                        nodoSiguiente = i.Item1;
                    }
                }
            }
            //RETORNA 200 SI ES FEASIBLE Y EL COSTO
            //RETORNA 500 SI NO FEASIBLE Y UN COSTO DE 0
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
                        costo += i.Item2;
                        arcs.Add( new Tuple<int, int>(nodo,raiz) );
                        camino.Add(raiz);
                        return new int[]{200,costo};
                    }
                }
                Console.WriteLine("NO ES UN GRAFO COMPLETAMENTE CONECTADO");
                return new int[]{500,0};//RETURN "NO FEASIBLE"
            }
            
            if(nodoSiguiente == -1){
                Console.WriteLine("NO SE COMPLETA EL CAMINO");
                return new int[]{500,0};
            }
            costo += costoMinimo;   //PARA COSTO TOTAL
            //costos.Add(costoMinimo);//PARA COSTO DE ARCO
            var a = new Tuple<int, int>(nodoInicial,nodoSiguiente);//ARCO POR EL QUE ESTA PASANDO
            arcs.Add( a );
            return busqueda(raiz,nodoSiguiente,visitados,costo);
        }
        static Camino ActualizarPeorRana(Camino peor, Camino mejor){
            int nodoNuevo1 = peor.camino[0];
            int nodoNuevo2 = peor.camino[0];
            int cantNodos = mejor.camino.Length - 1;
            int indNuevo = 0, indNuevo2 = 0;
            //Console.WriteLine($"num nodos {mejor.camino.Length}");
            //con eso aseguras que no selecciones el principio y final del camino original
            //porque esos son la base del tsp
            while((nodoNuevo1 == peor.camino[0] || nodoNuevo2 == peor.camino[0]) ||
                    (indNuevo == 0 || indNuevo2 == 0) ){
                Random rnd = new Random();
                indNuevo = rnd.Next( cantNodos - 1 ); //tiene que evitar usar el primer y ultimo arco
                indNuevo2 = indNuevo + 1;
                nodoNuevo1 = mejor.camino[indNuevo];
                nodoNuevo2 = mejor.camino[indNuevo2];
            }
            int[] nuevoCamino = new int[cantNodos+1];
            nuevoCamino = peor.camino;
            int tmpInd1 = 0, tmpInd2 = 0; int nObj = 0, nObj2 = 0;
            //Para primer nodo
            for(int i = 0; i < cantNodos + 1;i++){
                //BUSCAR EL NODO NUEVO PARA PODER HACER EL INTERCAMBIO
                if(peor.camino[i] == nodoNuevo1){
                    tmpInd1 = i;
                    nObj = nuevoCamino[indNuevo];
                    //Console.WriteLine($" nodo obj1 {nObj} temp 1: {tmpInd1}");
                    nuevoCamino[tmpInd1] = nObj;
                    nuevoCamino[indNuevo] = nodoNuevo1;
                    break;
                }
            }
            //Para segundo nodo
            for(int i = 0; i < cantNodos + 1;i++){
                //BUSCAR EL NODO NUEVO PARA PODER HACER EL INTERCAMBIO
                if(nuevoCamino[i] == nodoNuevo2){
                    tmpInd2 = i;
                    nObj2 = nuevoCamino[indNuevo2];
                    nuevoCamino[tmpInd2] = nObj2;
                    nuevoCamino[indNuevo2] = nodoNuevo2;
                    break;
                }
            }
            int cst = getCosto(nuevoCamino);
            double fit = getFitness(cst);
            Camino c = new Camino(nuevoCamino,fit,cst);
            return c;
        }
        static List<Mememplex> localSearch(List<Mememplex> memes){
            //BUSCAR POR MEMEPLEX, HACER UNA BUSQUEDA DE LAS RANAS
                //ORDENAR POR FITNESS
                //LA PEOR, TIENE QUE SER ACTUALIZADA
            foreach(Mememplex meme in memes){
                Console.WriteLine("memeplex otro");
                foreach(Rana r in meme.ranas){
                    //A cada rana le acomoda sus caminos por fitness
                    var ordenados = r.caminos.OrderBy(y => y.fitness);
                    Camino peorRana = ordenados.First();
                    Camino mejorRana = r.caminos.Last();
                    int inx = r.caminos.IndexOf(peorRana);
                    r.caminos[inx] = ActualizarPeorRana(peorRana,mejorRana);
                    foreach(Camino c in r.caminos){     //actualiza costo
                        //imprimirArray(c.camino);
                        //Console.WriteLine($"antes: {c.costo}");
                        c.setCosto( getCosto(c.camino) );
                        c.setFitness( getFitness(c.costo) );
                        //Console.WriteLine($"after: {c.costo} {c.fitness}");
                    }
                }
            }
            return memes;
        }
        static object  ob = new object();
        /*
        * Obtener caminos que serán asignados a las ranas
        * q -> cantidad de caminos
        * caminos -> caminos posibles a elegir, ya son soluciones tsp
        */
        public static List<Camino> obtenerCaminos(int q, List<Camino> caminos){
            Random rnd = new Random();
            Camino[] cams = new Camino[q];
            int nC = caminos.Count();
            //var caminosRana = new List<Camino>();
            for(int j = 0; j < q; j++){
                int indexCamino = rnd.Next( nC );
                cams[j] = caminos[indexCamino];
            }
            return cams.ToList();
        }
        static void Main(string[] args)
        {
            //Initialize parameters
            //f -> number of frogs
            //m -> number of memeplexes, f is distributed in these memeplexes
            //Each memeplex consists of n frogs that F = mn
            //f(i) -> fitness
            //n -> number of frogs in each memeplex
            //q -> number of paths in a submemeplex of a frog
            //Px -> position of a frog
            //PB -> best solution
            //PW -> worst solution
            //S  -> the updated step size of the frog-leaping
            //Smax -> maximum step size allowed by a frog after being affected
            //i -> number of iterations
            //wth is U(i) -> vector de la rana i
            //Variables miscelaneas
            grafo = leerGrafo();
            bool[] visitados = fillArrayBool(grafo.tamanioGrafo());
            List<Camino> caminos = new List<Camino>();
            List<int> costos = new List<int>();
            //List<CaminoOptimo> caminos = new List<CaminoOptimo>();
            /*Variables algoritmo*/
            int m = 2;
            int f = 20;
            int n = f/m;
            int q = n/2;
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
                int costo = 0;
                visitados = fillArrayBool(cantNodos);
                int[] res = busqueda(i,i,visitados,costo);
                //costos.Add(costo);
                //Console.WriteLine($"costo : {res[0]}");
                if(res[0] == 200){
                    double fit = getFitness(costo);
                    costo = res[1];
                    //Console.WriteLine($"costo : {res[1]}");
                    Camino c = new Camino( camino.ToArray(),fit,costo );
                    caminos.Add( c  );
                }else if(res[0] == 500){
                    return;
                }
            }
            
            List<Rana> ranas = new List<Rana>();
            //GENERA F CANTIDAD DE RANAS
            for(int i = 0; i < f; i++){
                //a cada rana, asignarle una lista de caminos
                ranas.Add(new Rana(obtenerCaminos(q,caminos)) );
                //EVALUAR EL FITNESS ??? aqui ya hice el fitness de cada camino
            }
            Console.WriteLine($"genera ranas ya");
            List<Mememplex> memeplexes = new List<Mememplex>();
            
            //CONSTRUIR M MEMEPLEXES
            int cont = 0;
            for(int i = 0; i < m; i++){
                List<Rana> ranasSub = new List<Rana>();
                for(int j = 0; j < n; j++){
                    //Estas ranas van a este memeplex
                    ranasSub.Add(ranas[cont]);
                    cont++;
                }
                memeplexes.Add(new Mememplex( ranasSub ) );
            }

            foreach(var rns in memeplexes){
                Console.WriteLine($"memeplex anterior");
                foreach(var rn in rns.ranas){
                    foreach(var c in rn.caminos){
                        //imprimirArray(c.camino);
                        Console.WriteLine($"{c.costo}");
                    }
                }
            }
            //LOCAL EXPLORATION
            for(int i = 0; i < 10; i++){
                memeplexes = localSearch(memeplexes);
                foreach(var mms in memeplexes){ //ranas
                    Console.WriteLine($"memeplex nuevo");
                    foreach(var rana in mms.ranas){
                        /* foreach(var cms in rana.caminos){
                            Console.WriteLine($"fit :{ cms.fitness}");
                        } */
                        var menosFitness = rana.caminos.OrderBy(x => x.costo);
                        //var w = menosFitness.First(); //Menor fitness
                        var j = menosFitness.Last();  //Mejor fitness
                        //mejoresCaminos.Add(j);
                        Console.WriteLine($"r : {j.costo}");
                    }
                }
            }
            #region verificar resultados
            List<Camino> mejoresCaminos = new List<Camino>();
            foreach(var mms in memeplexes){ //ranas
                Console.WriteLine($"memeplex nuevo");
                foreach(var rana in mms.ranas){
                    /* foreach(var cms in rana.caminos){
                        Console.WriteLine($"fit :{ cms.fitness}");
                    } */
                    var menosFitness = rana.caminos.OrderBy(x => x.costo);
                    var i = menosFitness.First(); //Menor fitness
                    var j = menosFitness.Last();  //Mejor fitness
                    mejoresCaminos.Add(j);
                    //Console.WriteLine($"r : {i.fitness} _ {j.fitness}");
                }
            }
            foreach(var i in mejoresCaminos){
                Console.WriteLine("Camino :");
                imprimirArray(i.camino);
                Console.WriteLine($" costo camino {i.costo}");
            }
            #endregion
        }
    }
}
