using System;
using System.Collections.Generic;
using System.Linq;

namespace gatos
{
  public class Cat{
    int id;
    int x;
    int y;
    string type;
    int smp;
    int srd;
    int cdc;
    int spc;
    int vx;
    int vy;
    public Cat(int id, int x, int y){
      this.id = id;
      this.x = x;
      this.y = y;
    }

    public void setType(string type){
      this.type = type;
    }

    public void printCat(){
      Console.WriteLine($"{this.id} : [{this.x},{this.y}]\nType: {this.type}");
    }
  }
  class Program
  {
    static void Main(string[] args)
    {
      int numberCats = 5;
      int[] ySpace = new int[]{0,10};
      int[] xSpace = new int[]{0,10};
      float motionRatio = 2/3;
      Random rnd = new Random();
      List<Cat> cats = new List<Cat>();
      for(int i = 0; i < numberCats; i++){
        int x = rnd.Next(xSpace.Min(), xSpace.Max());
        int y = rnd.Next(ySpace.Min(), ySpace.Max());
        cats.Add(new Cat(i,x,y));
        //Change this to make it dependant of the number of cats left
        //and make it after minute 20
        var typeCat = (rnd.NextDouble() < motionRatio) ? "search" : "trace";
        cats[i].setType(typeCat);
        Console.WriteLine(i);
        cats[i].printCat();
      }

      /*
      Espacio de solución
      X E [0,10]
			Y E [0,10]
      La velocidad máxima debe estar dentro de los límites de las coordenadas del espacio de solución
			Para seleccionar cuáles están en modo de búsqueda y cuáles en rastreo
			Se tiene un motion ratio, de acuerdo a este, se dividen las probabilidades de que sea búsqueda o rastreo:
			Por ejemplo, si el MR es 2/3 = 0.666, cuando se tire el dado:
			- si el valor va de 0 -> 0.4 es modo búsqueda,
			- si es > 0.4 entonces es modo de rastreo
				Se obtiene un aleatorio x,
				1. modo búsqueda -> si caen dentro del 0 a 1-mr
				2. modo rastreo -> si son mayores a 1-mr
				Después se itera de nuevo de acuerdo a cuántos hay ahora, hasta que tengas todos, pero el MR siempre se debe cumplir
			*/

    }
  }
}
