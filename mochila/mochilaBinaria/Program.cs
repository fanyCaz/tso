using System;
using System.Collections.Generic;
using System.Linq;

namespace mochilaBinaria
{
    public class CombinacionValida{
        public int b;
        public int p;
        public string numComb;
        public CombinacionValida(int b, int p){
            this.b = b;
            this.p = p;
        }
    }
    class Program
    {
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
            int capacidad = 60;
            int[] pesos = new int[] {15,12,10,13,22,16,10,14};
            int[] beneficios = new int[] {40,28,22,30,54,38,24,35};
            int numCombinaciones = (int)Math.Pow((double)(numeroObjetos * 2),2);
            Console.WriteLine(numCombinaciones);
            List<CombinacionValida> comb = new List<CombinacionValida>();
            int[] benefiTotales = new int[numCombinaciones];
            for(int i = 0; i < numCombinaciones; i++){
                int[] binario = binary(i,numeroObjetos);
                //suma producto de pesos por el binario de esta combinacion
                int pesoComb = pesos.Zip(binario, (p,b) => p*b).Sum();
                benefiTotales[i] = beneficios.Zip(binario,(be,bi) => be*bi ).Sum();
                if(pesoComb <= capacidad){
                    comb.Add(new CombinacionValida(benefiTotales[i],pesoComb));
                }
            }
            
            var ordered = comb.OrderBy(x => x.p).OrderBy(y => y.b);
            foreach(var i in ordered){
                Console.WriteLine($"peso {i.p} y beneficio {i.b}");
            }
            
        }
    }
}
