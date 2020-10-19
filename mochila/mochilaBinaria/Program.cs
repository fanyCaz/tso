using System;
using System.Linq;
using System.Threading;
using System.Diagnostics;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace mochilaBinaria
{
    public class Articulo{
        public int beneficio;
        public int peso;
        public char identificador;
        public Articulo(int b,int p, char i){
            this.beneficio = b;
            this.peso = p;
            this.identificador = i;
        }
    }
    public class CombinacionValida{
        public int b;       //beneficio
        public int p;       //pesos
        public int[] combinacion;
        public CombinacionValida(int b, int p, int[] combinacion){
            this.b = b;
            this.p = p;
            this.combinacion = combinacion;
        }
    }
    class Program
    {
        static char[] nombreArticulo = new char[]{'A','B','C','D','E','F','G','H'};
        static int[] pesos;
        static int[] beneficios;
        static int[] cantPosible;
        static List<CombinacionValida> comb = new List<CombinacionValida>();
        static bool parada = false;
        static CombinacionValida binaria(int[] pesos, int[] beneficios,int capacidad, int numeroObjetos){
            
            int numCombinaciones = (int)Math.Pow((double)(numeroObjetos * 2),2);
            Console.WriteLine($" objetos {numeroObjetos} capacidad {capacidad} combis {numCombinaciones}");
            int pesoComb = 0,benefComb = 0;
            for(int i = 0; i < numCombinaciones; i++){
                int[] binario = binary(i,numeroObjetos);
                //suma producto de pesos por el binario de esta combinacion
                pesoComb = pesos.Zip(binario, (p,b) => p*b).Sum();
                benefComb = beneficios.Zip(binario,(be,bi) => be*bi ).Sum();
                if(pesoComb <= capacidad){
                    Misc.imprimirArreglo(binario, "binario");
                    //el binario que se agrega es que objetos entran y que no
                    comb.Add(new CombinacionValida(benefComb,pesoComb,binario));
                }
            }
            return comb.OrderBy(x => x.p).OrderBy(y => y.b).Last();
        }
        //Crea un array con el numero binario del parámetro number
        //Se basa en el número de objetos para saber la longitud
        static int[] binary(int number,int numObj){
            Stack<int> s = new Stack<int>();
            int quotient = number,remainder = 0;
            do{
                remainder = quotient%2;
                quotient = quotient/2;
                s.Push(remainder);
            }while(quotient > 0);
            //rellenar numeros que faltan
            for(int i = s.Count; i < numObj; i++){
                s.Push(0);
            }
            return s.ToArray();
        }

        static int[] unidades(long iteracion, int numObj){
            int contador=1;
            while(iteracion > (256*contador)){
                //contador indica en que bloque de numero esta
                //donde 0 a 255 es 1
                //256 - 511 es 2 etc, para indicar el numero de articulos que puede llevar de cada uno
                contador+=1;
            }
            long send;
            if(iteracion < 256){
                send = iteracion;
            }
            else{
                send = (iteracion%(256*contador)==0) ? iteracion-(256*contador) : iteracion-(256*(contador-1));
            }
            
            int[] u = binary((int)send,numObj);
            
            int contPosiblesMax = 0;
            //Console.WriteLine($"longitud u {u.Length} objeto {numObj} itetacion {send} contador {contador}");
            for(int i = 0; i < u.Length; i++){
                //Console.WriteLine($"valor u {u[i]}");
                //valida que no pueda llevar mas objetos de ese articulo de los que puede
                if(contador > cantPosible[i]){
                    u[i] = pesos[i];
                    contPosiblesMax++;
                    
                }
                else{
                    u[i] *= contador;
                }
            }
            //Si ya ha encontrado la cantidad maxima de objetos en todos, entonces activa la bandera
            if(contPosiblesMax == numObj){
                parada=true;
            }
            //Console.WriteLine($" iteracion {iteracion} ");
            return u;
        }
        //Espera una iteracion min desde la que iniciará y max hasta donde ira
        static Object obj = new object();
        static void ResultadoParcial(int numObjetos, int capacidad, long min, long max){
            lock(obj){
                
                //if(parada) return;
               // List<CombinacionValida> localList = new List<CombinacionValida>();
                int pesoComb = 0,benefComb = 0;
                for(long i = min; i < max; i++){
                    int[] cantUnidades = unidades(i,numObjetos);
                    //suma producto de pesos por el array de cantidades de esta combinacion
                    pesoComb = pesos.Zip(cantUnidades, (p,u) => p*u).Sum();
                    benefComb = beneficios.Zip(cantUnidades,(be,u) => be*u ).Sum();
                    if(pesoComb <= capacidad){
                        //el binario que se agrega es que objetos entran y que no
                        //Console.WriteLine($" beneficio added {benefComb} ");
                        comb.Add(new CombinacionValida(benefComb,pesoComb,cantUnidades));
                    }
                }
                //return localList.OrderBy(x=> x.b).OrderBy(y=>y.p).Last();
            }
        }
        
        static CombinacionValida multiunidad(int[] pesos, int[] beneficios, int capacidad, int numeroObjetos){
            //cantidad posible para cada objeto
            cantPosible = new int[numeroObjetos];
            long numCombinaciones = 1;
            for(int i = 0; i < numeroObjetos; i++){
                //el redondeo es hacia abajo para llevar objetos enteros + la posibilidad de no llevar alguno
                cantPosible[i] = (int)Math.Floor((double) capacidad/pesos[i]) + 1;
                //obtener el numero de combinaciones
                numCombinaciones *= cantPosible[i];
            }
            //validar el dividendo
            long numHilos = numCombinaciones/10000000;
            Console.WriteLine($"num combinaciones {numCombinaciones} hilos {numHilos}");
            long min=0, max=10000000, temp1 = min, temp2 = max;
            for(int i=0; i < numHilos; i++){
                
                Thread hilo = new Thread(()=>{
                    ResultadoParcial(numeroObjetos,capacidad,temp1,temp2);
                });
                hilo.Start();
                hilo.Join();
                if(parada) break;
                
                temp1 = min + temp2;
                temp2 = (i == numHilos -1) ? numCombinaciones : max + temp2;
            }
            return comb.OrderBy(x=> x.p).OrderBy(y=>y.b).Last();
            //return comb.First();
        }

        static void Main(string[] args)
        {
            
            //pesos = new int[] {15,12,10,13,22,16,10,14};
            //beneficios = new int[] {40,28,22,30,54,38,24,35};
            CombinacionValida ordered = new CombinacionValida(0,0,new int[]{0});
            
            int numeroObjetos = Misc.obtenerCantidad("número de objetos");
            int capacidad = Misc.obtenerCantidad("capacidad");
            int tipoMochila = Misc.obtenerTipoMochila();
            //pesos = Misc.obtenerValoresArticulos(numeroObjetos, "pesos");
            pesos = new int[] {15,12,10,13};
            beneficios = new int[] {40,28,22,30};
            //beneficios = Misc.obtenerValoresArticulos(numeroObjetos, "beneficios");
            var tiempo = 0;
            switch(tipoMochila){
                case 1:
                    ordered = binaria(pesos,beneficios,capacidad,numeroObjetos);
                    break;
                case 2:
                    Stopwatch sw = new Stopwatch();
                    sw.Start();
                    ordered = multiunidad(pesos,beneficios,capacidad,numeroObjetos);
                    sw.Stop();
                    tiempo = sw.Elapsed.Milliseconds;
                    break;
                default:
                    Console.Write("hey");
                    break;
            }

            Console.WriteLine($"Se pueden llevar los articulos : ");
            for(int i = 0; i < ordered.combinacion.Length; i++){
                //si el articulo de esa posicion si esta en la combinacion, se muestra
                if(ordered.combinacion[i] > 0)
                    Console.WriteLine($"Articulo {nombreArticulo[i]} Cantidad : {ordered.combinacion[i]}");
            }
            Console.WriteLine($" Con el beneficio {ordered.b} y el peso {ordered.p}");
        }
    }
}
