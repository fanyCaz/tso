using System;

namespace flujomaximo{
    class Misc{
        public static int obtenerNumero(string nombre){
            Console.WriteLine($"Ingrese {nombre}");
            int number = 0;
            try{
                number = int.Parse(Console.ReadLine());
                if(number < 0){
                    Console.WriteLine("Ingrese un número positivo");
                    return obtenerNumero(nombre);
                }
                return number;
            }catch(Exception){
                Console.WriteLine("Ingrese un número, porfavor");
                return obtenerNumero(nombre);
            }
        }

        public static int validarNodosRecorrido(AdjacencyListCapacity graph, string nombre){
            bool error = true;int nodo=-1;
            do{
                nodo = Misc.obtenerNumero($" el nodo {nombre} del recorrido");
                try{
                    var x = graph[nodo];
                    error = false;
                }catch(Exception){
                   continue;
                }
            }while(error);
            return nodo;
        }
    }
}