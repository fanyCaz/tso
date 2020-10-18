using System;
using System.Collections.Generic;
using System.Linq;

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
                u[i] = u[i] * contador;
            }
            return u;
        }
        
        static void multiunidad(List<Articulo> articulos, int capacidad, int numeroObjetos){
            //cantidad posible para cada objeto
            int[] cantPosible = new int[numeroObjetos];
            long numCombinaciones = 1;
            for(int i = 0; i < numeroObjetos; i++){
                //el redondeo es hacia abajo para llevar objetos enteros + la posibilidad de no llevar alguno
                cantPosible[i] = (int)Math.Floor((double) capacidad/articulos[i].peso) + 1;
                //obtener el numero de combinaciones
                numCombinaciones *= cantPosible[i];
            }
            Console.WriteLine($"num combinaciones {numCombinaciones}");
            var res = unidades(767,articulos.Count);
            foreach(var i in res){
                Console.Write($" {i} - ");
            }
            Console.Write($"\n");

        }
        
        static void Main(string[] args)
        {
            int numeroObjetos = 8;
            int capacidad = 600;
            
            int[] pesos = new int[] {15,12,10,13,22,16,10,14};
            int[] beneficios = new int[] {40,28,22,30,54,38,24,35};
            
            //Articulos
            List<Articulo> articulos = new List<Articulo>();
            articulos.Add(new Articulo(40,15,'A'));
            articulos.Add(new Articulo(28,12,'B'));
            articulos.Add(new Articulo(22,10,'C'));
            articulos.Add(new Articulo(30,13,'D'));
            articulos.Add(new Articulo(54,22,'E'));
            articulos.Add(new Articulo(38,16,'F'));
            articulos.Add(new Articulo(24,10,'G'));
            articulos.Add(new Articulo(35,14,'H'));

            var ordered = new CombinacionValida(0,0,new int[]{0});
            Console.WriteLine($"Tipo Mochila : 1- Binaria , 2- Multiunidad");
            int tipoMochila = 2;//int.Parse(Console.ReadLine());

            switch(tipoMochila){
                case 1:
                    ordered = binaria(pesos,beneficios,capacidad,numeroObjetos);
                    break;
                case 2:
                    multiunidad(articulos,capacidad,numeroObjetos);
                    break;
                default:
                    Console.Write("hey");
                    break;
            }

            Console.WriteLine($"Se pueden llevar los articulos : ");
            for(int i = 0; i < ordered.combinacion.Length; i++){
                if(ordered.combinacion[i] == 1)
                    Console.Write(nombreArticulo[i]);   
            }
            Console.WriteLine($" Con el beneficio {ordered.b} y el peso {ordered.p}");
        }
    }
}
