using System;
using System.Collections.Generic;
using System.Linq;

namespace mochilaBinaria
{
    public class CombinacionValida{
        public int b;
        public int p;
        public int[] combinacion;
        public CombinacionValida(int b, int p, int[] combinacion){
            this.b = b;
            this.p = p;
            this.combinacion = combinacion;
        }
    }
    class Program
    {
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
                    comb.Add(new CombinacionValida(benefComb,pesoComb,binario));
                }
            }
            return comb.OrderBy(x => x.p).OrderBy(y => y.b).Last();
        }

        static void multiunidad(int[] pesos, int[] beneficios, int capacidad, int numeroObjetos){
            int[] capacidadesMaximas = new int[numeroObjetos];
            for(int i = 0; i < numeroObjetos; i++){
                capacidadesMaximas[i] = (int)Math.Floor((double)(capacidad/pesos[i]));
                Console.WriteLine(capacidadesMaximas[i]);
            }
            //INCOMPLETO HOHO
        }
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
        static void Main(string[] args)
        {
            int numeroObjetos = 8;
            int capacidad = 600;
            
            char[] nombreArticulo = new char[]{'A','B','C','D','E','F','G','H'};
            int[] pesos = new int[] {15,12,10,13,22,16,10,14};
            int[] beneficios = new int[] {40,28,22,30,54,38,24,35};
            
            var ordered = new CombinacionValida(0,0,new int[]{0});
            Console.WriteLine($"Tipo Mochila : 1- Binaria , 2- Multiunidad");
            int tipoMochila = int.Parse(Console.ReadLine());

            switch(tipoMochila){
                case 1:
                    ordered = binaria(pesos,beneficios,capacidad,numeroObjetos);
                    break;
                case 2:
                    multiunidad(pesos,beneficios,capacidad,numeroObjetos);
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
