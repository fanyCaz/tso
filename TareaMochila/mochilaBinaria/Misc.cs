using System;

namespace mochilaBinaria{
    class Misc{
        public static void imprimirArreglo(int[] arr, string nombre){
            Console.WriteLine($"{nombre} length : {arr.Length}");
            for(int i = 0; i < arr.Length; i++){
                Console.Write($"{arr[i]} - ");
            }
            Console.WriteLine();
        }

        public static int[] obtenerValoresArticulos(int cantidad, string nombre){
            Console.WriteLine($"Ingresar {nombre}");
            int[] valores = new int[cantidad];
            //Console.WriteLine($"--- {cantidad} ---- {valores.Length}");
            for(int i = 0; i < cantidad; i++){
                valores[i] = obtenerCantidad($"el valor de objeto {i+1}");
            }
            return valores;
        }

        public static int obtenerTipoMochila(){
            Console.WriteLine($"Tipo Mochila : 1- Binaria , 2- Multiunidad , 3- Volumen");
            int tipoMochila = 0;
            try{
                tipoMochila = int.Parse(Console.ReadLine());
                if(tipoMochila < 1 || tipoMochila > 3){
                    Console.WriteLine("Ingrese un número dentro de las opciones, porfavor");
                    return obtenerTipoMochila();
                }
                return tipoMochila;
            }catch(Exception){
                Console.WriteLine("Ingrese un número, porfavor");
                return obtenerTipoMochila();
            }
        }

        public static int obtenerCantidad(string nombre){
            Console.WriteLine($"Ingrese {nombre} ");
            int cantObj = -1;
            try{
                cantObj = int.Parse(Console.ReadLine());
                if(cantObj < 0){
                    Console.WriteLine("Ingrese un número positivo");
                    return obtenerCantidad(nombre);
                }
                return cantObj;
            }catch(Exception){
                Console.WriteLine("Ingrese un número, porfavor");
                return obtenerCantidad(nombre);
            }
        }
    }
}