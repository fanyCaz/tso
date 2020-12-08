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
                    //el binario que se agrega es que objetos entran y que no
                    //Misc.imprimirArreglo(binario,"binario");
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
            int numDivisor = (int)Math.Pow((double)(numObj * 2),2);//256
            while(iteracion > (numDivisor*contador)){
                //contador indica en que bloque de numero esta
                //donde 0 a 255 es 1
                //numDivisor - 511 es 2 etc, para indicar el numero de articulos que puede llevar de cada uno
                contador+=1;
            }
            long send;
            if(iteracion < numDivisor){
                send = iteracion;
            }
            else{
                send = (iteracion%(numDivisor*contador)==0) ? iteracion-(numDivisor*contador) : iteracion-(numDivisor*(contador-1));
            }
            
            int[] u = binary((int)send,numObj);
            
            int contPosiblesMax = 0;
            for(int i = 0; i < numObj; i++){
                Console.WriteLine($"contador{contador} num objetos{numObj}");
                Misc.imprimirArreglo(cantPosible, "cant posible" );
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
            return u;
        }
        //Espera una iteracion min desde la que iniciará y max hasta donde ira
        static Object obj = new object();
        static int conttaa=0;
        static void ResultadoParcial(int numObjetos, int capacidad, long min, long max){
            lock(obj){
                conttaa+=1;
                //Console.WriteLine($"en res parcial {min}");
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
            
            //la cantidad de hilos que se puede llegar a hacer es ridicula, 
            //investigar por mejor forma,
            //por ahora, es la manera más ''rapida'' de hacerlo y que considere todas
            //todas hasta que alcance el máximo posible de cada objeto
            long dividendo = (numCombinaciones >= 1000000) ? (long)Math.Floor(numCombinaciones*.0000001) : numCombinaciones;
            long numHilos = numCombinaciones/dividendo;
            Console.WriteLine($"Número de combinaciones {numCombinaciones} hilos {numHilos}");
            long min=0, max=dividendo, temp1 = min, temp2 = max;
            for(int i=0; i < numHilos; i++){
                
                Thread hilo = new Thread(()=>{
                    ResultadoParcial(numeroObjetos,capacidad,temp1,temp2);
                });
                hilo.Start();
                hilo.Join();
                //Si al hacer el arreglo de unidades que puede llevar
                //se ha llegado ya al maximo en todas, 
                //entonces se detiene
                if(parada){
                    break;
                }
                
                temp1 = min + temp2;
                temp2 = (i == numHilos -1) ? numCombinaciones : max + temp2;
            }
            Console.WriteLine($"contador total {conttaa}");
            return comb.OrderBy(x=> x.p).OrderBy(y=>y.b).Last();
        }

        static CombinacionValida volumen(int[] pesos, int[] beneficios, int[] volumenes, int capPeso, int capVol, int numeroObjetos){
            //Beneficio sobre peso
            double bvol = 0;
            //Beneficio sobre volumen
            double bpeso = 0;
            //Media geométrica, para involucrar los dos puntos de interes
            double[] mg = new double[numeroObjetos];
            //Maximo por peso y máximo por volumen
            double maxPeso = 0, maxVol = 0;
            int[] cantPosible = new int[numeroObjetos];
            int numCombinaciones = 1;
            for(int i = 0; i < numeroObjetos; i++){
                bpeso = (double)beneficios[i]/pesos[i];
                bvol = (double)beneficios[i]/volumenes[i];
                mg[i] = Math.Sqrt((bpeso*bvol));
                maxPeso = (int)Math.Floor((double) capPeso/pesos[i]);
                maxVol = (int)Math.Floor((double) capVol/volumenes[i]);
                cantPosible[i] = (int)Math.Min(maxPeso,maxVol);
                numCombinaciones *= cantPosible[i];
                Console.WriteLine($" media geometrica {mg[i]:##.##} peso{maxPeso} vol{maxVol} cantidad minima {cantPosible[i]}");
            }
            Console.WriteLine($"num combinaciones {numCombinaciones}");
            for(int i = 0; i < numCombinaciones; i++){
                int[] cantUnidades = unidades(i,numeroObjetos);
                //Misc.imprimirArreglo(cantUnidades,"combis");
                //suma producto de pesos por el array de cantidades de esta combinacion
                /*pesoComb = pesos.Zip(cantUnidades, (p,u) => p*u).Sum();
                benefComb = beneficios.Zip(cantUnidades,(be,u) => be*u ).Sum();
                if(pesoComb <= capacidad){
                    //el binario que se agrega es que objetos entran y que no
                    //Console.WriteLine($" beneficio added {benefComb} ");
                    comb.Add(new CombinacionValida(benefComb,pesoComb,cantUnidades));
                }*/
            }
            //unidades();
            //tiene que hacer combinaciones multiunidades
            comb.Add(new CombinacionValida(20,20,new int[]{2,3,4,5,5,6}));
            return comb.OrderBy(x=> x.p).OrderBy(y=>y.b).Last();
        }

        static void Main(string[] args)
        {
            
            //pesos = new int[] {15,12,10,13,22,16,10,14}; //22,16,10,14
            //beneficios = new int[] {40,28,22,30,54,38,24,35};//,54,38,24,35
            CombinacionValida ordered = new CombinacionValida(0,0,new int[]{0});
            
            int numeroObjetos = Misc.obtenerCantidad("número de objetos posibles");
            int capacidad = Misc.obtenerCantidad("capacidad de mochila");
            int tipoMochila = Misc.obtenerTipoMochila();
            pesos = new int[] {240,200,250,180,235,190};//Misc.obtenerValoresArticulos(numeroObjetos, "pesos");
            beneficios = new int[] {144,160,188,162,118,152};//Misc.obtenerValoresArticulos(numeroObjetos, "beneficios");
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
                case 3:
                    int capacidadVolumen = Misc.obtenerCantidad("volumen");
                    //int[] volumenes = Misc.obtenerValoresArticulos(numeroObjetos, "volumenes");
                    int[] volumenes = new int[] {60,80,75,95,50,80};
                    ordered = volumen(pesos,beneficios,volumenes,capacidad,capacidadVolumen,numeroObjetos);
                    break;
                default:
                    Console.Write("hey");
                    break;
            }

            Console.WriteLine($"Se pueden llevar los articulos : ");
            for(int i = 0; i < numeroObjetos; i++){
                //si el articulo de esa posicion si esta en la combinacion, se muestra
                if(ordered.combinacion[i] > 0)
                    Console.WriteLine($"Articulo {nombreArticulo[i]} Cantidad : {ordered.combinacion[i]}");
            }
            Console.WriteLine($" Con el beneficio {ordered.b} y el peso {ordered.p} tiempo {tiempo}");
        }
    }
}
