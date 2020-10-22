using System;
using System.Linq;
using System.Collections.Generic;

namespace flujomaximo{
    public class CaminoPosible{
        List<int> nodosVisitados = new List<int>();
        int costoTotal = 0;
        public void setCostoTotal(int costo){
            this.costoTotal += costo;
        }
        public int getCostoTotal(){
            return this.costoTotal;
        }
        public void addNodo(int nodo){
            this.nodosVisitados.Add(nodo);
        }
        public List<int> getNodosVisitados(){
            return this.nodosVisitados;
        }
    }
    class CaminoCorto{
        static AdjacencyList g;
        static int costo = 0;
        static bool[] visitados;
        static int?[] path;
        static void initializeArrays(int n){
            visitados = new bool[n];
            path = new int?[n];
            for(int i =0; i < n;i++){
                visitados[i] = false;
                path[i] = null;
            }
        }
        static List<Tuple<int,int>> camino = new List<Tuple<int, int>>();
        static int idCaminos = 0;
        static Dictionary<int,CaminoPosible> caminos = new Dictionary<int, CaminoPosible>();
        static void caminosVarios(int nI,int nF){
            if(nI == nF){
                Console.WriteLine("No puedes visitarte a ti mismo");
                return;
            }
            int at = nI;
            //Toma los vecinos de ese nodo
            var vecinos = g[at];
            foreach(var i in vecinos){
                Console.WriteLine($" nodo : {i.Item1} costo: {i.Item2} visit? {visitados[i.Item1]}");
                //verifica si ya se ha visitado ese nodo
                if(!visitados[i.Item1]){
                    visitados[i.Item1] = true;
                    CaminoPosible cp = new CaminoPosible();
                    cp.setCostoTotal(i.Item2);
                    cp.addNodo(i.Item1);
                    caminos.Add(idCaminos,cp);
                    idCaminos+=1;
                }
            }
            foreach(var i in caminos){
                Console.WriteLine($"--------camino {i.Key}------");
                List<int> n = i.Value.getNodosVisitados();
                Console.WriteLine($"costo {i.Value.getCostoTotal()}");
                foreach(var j in n){
                    Console.WriteLine($"nodo {j}");
                }
            }

            var w = caminos.OrderBy(x=>x.Value.getCostoTotal()).Last();
            Console.WriteLine( $"llave de costo mas alto {w.Key}" );
            caminos.Remove(w.Key);
            foreach(var i in caminos){
                Console.WriteLine($"--------camino {i.Key}------");
                List<int> n = i.Value.getNodosVisitados();
                Console.WriteLine($"costo {i.Value.getCostoTotal()}");
                foreach(var j in n){
                    Console.WriteLine($"nodo {j}");
                }
            }
        }
        static void greedySearch(int nI,int nF){
            if(nI == nF){
                Console.WriteLine("No puedes visitarte a ti mismo");
                return;
            }
            int at = nI,costoMinimo = int.MaxValue;
            //Toma los vecinos de ese nodo
            var vecinos = g[at];
            Tuple<int,int> cm = new Tuple<int, int>(0,0);
            foreach(var i in vecinos){
                Console.WriteLine($" nodo : {i.Item1} costo: {i.Item2} visit? {visitados[i.Item1]}");
                //verifica si ya se ha visitado ese nodo
                if(!visitados[i.Item1]){
                    //Si encuentra el valor primero, se detiene el programa y se agrega al camino
                    if(i.Item1 == nF){
                        camino.Add(new Tuple<int, int>(i.Item1,i.Item2));
                        return;
                    }
                    //marca el nodo como visitado
                    visitados[at] = true;
                    //verifica el costo menor
                    //Console.WriteLine($"costo Antes comparar {costoMinimo}");
                    if(i.Item2 < costoMinimo){
                        costoMinimo = i.Item2;
                        cm = new Tuple<int, int>(i.Item1,i.Item2);
                    }
                }
            }
            Console.WriteLine($"nodo {cm.Item1} costo {cm.Item2}");
            //se agrega al camino el nodo con el menor costo
            camino.Add(cm);
            at = camino.Last().Item1;
            greedySearch(at,nF);
        }
        public static void Init(){
            int nodes = 8,nInicial=4,nFinal=7;
            g = Graphs.CyclicGraph8Nodes();
            initializeArrays(nodes);
            caminosVarios(nInicial,nFinal);
            costo = 0;
            /* Console.WriteLine($"Recorrido");
            Console.WriteLine($"nodo : {nInicial} ");
            foreach(var i in camino){
                Console.WriteLine($"nodo : {i.Item1} peso: {i.Item2}");
                costo += i.Item2;
            }
            Console.WriteLine($"Costo Final {costo}"); */
        }
    }
}