using System;
using System.Linq;
using System.Threading;
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
        static CombinacionValida binaria(int[] pesos, int[] beneficios,int capacidad, int numeroObjetos){
            int numCombinaciones = (int)Math.Pow((double)(numeroObjetos * 2),2);
            int pesoComb = 0,benefComb = 0;
            List<CombinacionValida> comb = new List<CombinacionValida>();
            for(int i = 0; i < numCombinaciones; i++){
                int[] binario = binary(i,numeroObjetos);
                //suma producto de pesos por el binario de esta combinacion
                pesoComb = pesos.Zip(binario, (p,b) => p*b).Sum();
                benefComb = beneficios.Zip(binario,(be,bi) => be*bi ).Sum();
                if(pesoComb <= capacidad){
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

        static int[] unidades(int iteracion, int numObj){
            int contador=1;
            while(iteracion > (256*contador)){
                //contador indica en que bloque de numero esta
                //donde 0 a 255 es 1
                //256 - 511 es 2 etc, para indicar el numero de articulos que puede llevar de cada uno
                contador+=1;
            }
            int send;
            if(iteracion < 256){
                send = iteracion;
            }
            else{
                send = (iteracion%(256*contador)==0) ? iteracion-(256*contador) : iteracion-(256*(contador-1));
            }
            
            int[] u = binary(send,numObj);
            //Console.WriteLine($"longitud u {u.Length} objeto {numObj} itetacion {send} contador {contador}");
            for(int i = 0; i < u.Length; i++){
                //Console.WriteLine($"valor u {u[i]}");
                if(contador > pesos[i])
                    u[i] = pesos[i];
                else
                    u[i] *= contador;
            }
            return u;
        }
        //Espera una iteracion min desde la que iniciará y max hasta donde ira
        static CombinacionValida ResultadoParcial(int numObjetos, int capacidad, int min, int max){
            int pesoComb = 0,benefComb = 0;
            List<CombinacionValida> comb = new List<CombinacionValida>();
            for(int i = min; i < max; i++){
                int[] cantUnidades = unidades(i,numObjetos);
                //suma producto de pesos por el binario de esta combinacion
                pesoComb = pesos.Zip(cantUnidades, (p,u) => p*u).Sum();
                benefComb = beneficios.Zip(cantUnidades,(be,u) => be*u ).Sum();
                if(pesoComb <= capacidad){
                    //el binario que se agrega es que objetos entran y que no
                    comb.Add(new CombinacionValida(benefComb,pesoComb,cantUnidades));
                }
            }
            return comb.OrderBy(x => x.p).OrderBy(y => y.b).Last();
        }
        
        static CombinacionValida multiunidad(int[] pesos, int[] beneficios, int capacidad, int numeroObjetos){
            //cantidad posible para cada objeto
            int[] cantPosible = new int[numeroObjetos];
            long numCombinaciones = 1;
            for(int i = 0; i < numeroObjetos; i++){
                //el redondeo es hacia abajo para llevar objetos enteros + la posibilidad de no llevar alguno
                cantPosible[i] = (int)Math.Floor((double) capacidad/pesos[i]) + 1;
                //obtener el numero de combinaciones
                numCombinaciones *= cantPosible[i];
            }
            Console.WriteLine($"num combinaciones {numCombinaciones}");
            

            
        }
        
        static void Main(string[] args)
        {
            int numeroObjetos = 8;
            int capacidad = 600;
            
            pesos = new int[] {15,12,10,13,22,16,10,14};
            beneficios = new int[] {40,28,22,30,54,38,24,35};
            
            
            var ordered = new CombinacionValida(0,0,new int[]{0});
            Console.WriteLine($"Tipo Mochila : 1- Binaria , 2- Multiunidad");
            int tipoMochila = int.Parse(Console.ReadLine());

            switch(tipoMochila){
                case 1:
                    ordered = binaria(pesos,beneficios,capacidad,numeroObjetos);
                    break;
                case 2:
                    ordered = multiunidad(pesos,beneficios,capacidad,numeroObjetos);
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
