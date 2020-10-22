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
    public class Camino{
        int costo = 0;
        List<int> nodos = new List<int>();
        public void agregarNodo(int nodo){
            this.nodos.Add(nodo);
        }
        public void agregarCosto(int costo){
            this.costo += costo;
        }
        public void mostrarCamino(){
            this.nodos.Reverse();
            foreach(var i in this.nodos){
                if(i == this.nodos.Last()){
                    Console.Write($"{i}");
                }
                else{
                    Console.Write($"{i} -> ");
                }
            }
            Console.WriteLine();
        }
        public void mostrarCosto(){
            Console.WriteLine($"Costo del Camino {this.costo}");
        }
    }
    class CaminoCorto{
        static AdjacencyList g;
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
        //pathethic 
        //trato de modificar el bfs para almacenar los caminos
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
                        CaminoPosible w = new CaminoPosible(i.Item1,i.Item2);
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
        static List<Vertice> inicializarPred(int nodos){
            int?[] res = new int?[nodos];
            List<Vertice> l = new List<Vertice>();
            
            for(int i=0; i < nodos; i++){
                res[i] = null;
                l.Add(null);
            }
            return l;
        }
        
        static void EdmondsKarp(AdjacencyListCapacity graph,int source, int sink, int nodos){
            if(source == sink){
                Console.WriteLine("No puedes visitarte a ti mismo");
                return;
            }
            List<Vertice> pred = inicializarPred(nodos);
            int flujo = 0;
            Camino camino = new Camino();
            //esto es bfs pero se agrega la decision de reconstruccion
            do{
                Queue<int> q = new Queue<int>();
                q.Enqueue(source);
                
                //graph.mostrarListaAdyacencia();
                while(q.Count > 0){
                    int cur = q.Dequeue();
                    int[] x =new int[]{1,2,1,3,5};
                    var vecinos = graph[cur];
                    foreach(var edge in vecinos){
                        if( pred[edge.sink] == null && edge.sink != source && edge.capacidad > edge.flujo ){
                            pred[edge.sink] = edge;
                            q.Enqueue(edge.sink);
                        }
                    }
                }
                
                if(pred[sink] != null){
                    int df = int.MaxValue;
                    //Se busca en el pred[nullables] los nodos del camino
                    //El df es el flujo/costo que puede pasar aka la resta de lo que lleva con lo que puede
                    for(Vertice e = pred[sink]; e!=null; e = pred[e.source]){
                        df = Math.Min(df,e.capacidad - e.flujo);
                    }
                    for(Vertice e = pred[sink]; e != null; e = pred[e.source]){
                        //Actualiza el flujo que pasa por este vertice
                        e.flujo = e.flujo + df;
                        //Actualiza el camino
                        camino.agregarNodo(e.sink);
                        camino.agregarCosto(e.capacidad);
                    }
                    //Se suma al flujo total
                    flujo += df;
                }
            }while(pred[sink] == null);
            camino.agregarNodo(source);
            
            camino.mostrarCamino();
            camino.mostrarCosto();
            //Console.WriteLine($"Flujo Máximo {flujo}");
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
            AdjacencyListCapacity graph = Graphs.Cyclic8Nodes();
            initializeArrays(nodes);
            // :) //
            EdmondsKarp(graph,nInicial,nFinal,nodes);
        }
    }
}