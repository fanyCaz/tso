using System;
using System.Collections.Generic;
using System.Linq;

namespace BoundedKnapsack
{
    class Mochila{
        private int capacidad;
        private Objeto[] objetos = new Objeto[];
        Mochila(int capacidad){
            this.capacidad = capacidad;
        }
        void setObjetos(Objeto o){
            objetos[this.objetos.Length+1] = o;
        }
    }
    class Objeto{
        private int peso;
        private int beneficio;
        Objeto(int peso, int beneficio){
            this.peso = peso;
            this.beneficio = beneficio;
        }
        void setPeso(int peso){
            this.peso = peso;
        }
        void getPeso(){
            return this.peso;
        }
        void setBeneficio(int beneficio){
            this.beneficio = beneficio;
        }
        void getBeneficio(){
            return this.beneficio;
        }
    }
    class Program
    {
        static void MostrarMochila(Dictionary<int,int[]> objetos){
            for(int i = 0; i < objetos.Count; i++){
                Console.WriteLine(objetos.Count);
            }
        }
        static void Main(string[] args)
        {
            int capacidad = 0;
            while(capacidad == 0){
                Console.WriteLine("Ingresa la capacidad de la mochila (1,2,...,n)");
                string c = Console.ReadLine();
                capacidad = (int.Parse(c) > 0) ? int.Parse(c) : 0;
            }

            int numeroObjetos = 0;
            while(numeroObjetos == 0){
                Console.WriteLine("Ingresa la cantidad de objetos disponibles(1,2,..,n)");
                string c = Console.ReadLine();
                numeroObjetos = (int.Parse(c) > 0) ? int.Parse(c) : 0;
            }

            Dictionary<int,int[]> objetos = new Dictionary<int, int[]>();
            for(int i = 1; i <= numeroObjetos; i++){
                int peso = 0,beneficio=0;
                while(peso == 0){
                    Console.WriteLine("Ingresa el peso del objeto {0}",i);
                    string c = Console.ReadLine();
                    peso = (int.Parse(c) > 0) ? int.Parse(c) : 0;
                }
                while(beneficio == 0){
                    Console.WriteLine("Ingresa el beneficio del objeto {0}",i);
                    string c = Console.ReadLine();
                    beneficio = (int.Parse(c) > 0) ? int.Parse(c) : 0;
                }
                //objetos.Add(i,new int[] {peso,beneficio});
                Objeto o = new Objeto(peso,beneficio);
                Console.WriteLine("objeto {0}",i);
            }

            MostrarMochila(objetos);

            
        }
    }
}