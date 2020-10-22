using System;
using System.Linq;
using System.Collections.Generic;

namespace flujomaximo{
    public class CaminoPosible{
        List<int> nodosVisitados = new List<int>();
        int costoTotal = 0;
        int id;
        public CaminoPosible(int nodo,int costo){
            //this.id=id;
            this.nodosVisitados.Add(nodo);
            this.costoTotal = costo;
        }
        public void setId(int id){
            this.id = id;
        }
        public int getId(){
            return this.id;
        }
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
        static List<CaminoPosible> caminos = new List<CaminoPosible>();
        static void caminosVarios(int nI,int nF){
            if(nI == nF){
                Console.WriteLine("No puedes visitarte a ti mismo");
                return;
            }
            //caminos.Add(new CaminoPosible(nI,nI,0));
            Queue<int> q = new Queue<int>();
            q.Enqueue(nI); int nodoAnterior = -1;
            visitados[nI] = true;   //marcar el nodo inicial como visitado
            //while(q.Count > 0){
            for(int j = 0; j < 2; j++){
                //initializeArrays(8);
                if(nodoAnterior > -1){
                    visitados[nodoAnterior] = true;
                }
                int nodo = q.Dequeue();
                
                //Toma los vecinos de ese nodo
                var vecinos = g[nodo];
                foreach(var i in vecinos){
                    Console.WriteLine($" nodo : {i.Item1} costo: {i.Item2} visit? {nodoAnterior != i.Item1}");
                    //verifica si ya se ha visitado ese nodo
                    //if(!visitados[i.Item1]){
                    if(nodoAnterior != i.Item1){
                        q.Enqueue(i.Item1);
                        //visitados[i.Item1] = true;
                        //var w = caminos.Find(x => x.getId() == nodoAnterior );
                        
                        CaminoPosible w = new CaminoPosible(i.Item1,i.Item2);
                        /* cp.setCostoTotal(i.Item2);
                        cp.addNodo(i.Item1);
                        cp.setId(i.Item1); */
                        //caminos.Add(cp);
                        idCaminos+=1;
                    }
                }
                nodoAnterior = nodo;
                var nodoCostoMayor = caminos.OrderBy(x=>x.getCostoTotal()).Last();
                Console.WriteLine( $"llave de costo mas alto {nodoCostoMayor.getId()}" );
                caminos.Remove(nodoCostoMayor);
                foreach(var i in caminos){
                    Console.WriteLine($"--------camino {i.getId()}------");
                    List<int> n = i.getNodosVisitados();
                    Console.WriteLine($"costo {i.getCostoTotal()}");
                    foreach(var k in n){
                        Console.WriteLine($"nodo {k}");
                    }
                }
            }
            /* var t = caminos.Find(x => nodo == x.getNodosVisitados().Find(y=> y == nodo));
            if(t != null){
                //t.setId(nodo);
                t.addNodo(nodoAnterior);
            } */
            
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