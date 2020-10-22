using System;

namespace flujomaximo{
    class Graphs{
        //from,to,cost  ---> 0 -> 1 -> 2 ,3 -> 4,5,6 -> 7 
        public static AdjacencyList DAG8nodes(){
            AdjacencyList g = new AdjacencyList(8);
            g.agregaVertice(0,1,5);
            g.agregaVertice(1,2,11);
            g.agregaVertice(1,3,10);
            g.agregaVertice(2,4,14);
            g.agregaVertice(2,5,13);
            g.agregaVertice(3,6,11);
            g.agregaVertice(4,7,12);
            g.agregaVertice(5,7,5);
            g.agregaVertice(6,5,4);
            g.agregaVertice(6,7,9);
            return g;
        }
        //7->0,3,1->2,6->4,5
        public static AdjacencyList CyclicGraph8Nodes(){
            AdjacencyList h = new AdjacencyList(8);
            h.agregaVertice(0,1,3);
            h.agregaVertice(0,2,5);
            h.agregaVertice(0,3,2);
            h.agregaVertice(0,7,10);
            h.agregaVertice(1,0,3);
            h.agregaVertice(1,2,5);
            h.agregaVertice(1,4,4);
            h.agregaVertice(1,6,6);
            h.agregaVertice(1,3,8);
            h.agregaVertice(1,7,6);
            h.agregaVertice(2,0,5);
            h.agregaVertice(2,1,5);
            h.agregaVertice(2,6,9);
            h.agregaVertice(2,4,1);
            h.agregaVertice(2,5,7);
            h.agregaVertice(3,7,14);
            h.agregaVertice(3,0,2);
            h.agregaVertice(3,1,8);
            h.agregaVertice(3,4,12);
            h.agregaVertice(4,2,1);
            h.agregaVertice(4,1,4);
            h.agregaVertice(4,3,12);
            h.agregaVertice(4,6,15);
            h.agregaVertice(5,1,7);
            h.agregaVertice(5,7,9);
            h.agregaVertice(6,1,6);
            h.agregaVertice(6,2,9);
            h.agregaVertice(6,4,15);
            h.agregaVertice(6,7,3);
            h.agregaVertice(7,0,10);
            h.agregaVertice(7,5,9);
            h.agregaVertice(7,1,6);
            h.agregaVertice(7,6,3);
            h.agregaVertice(7,3,14);
            return h;
        }

        public static AdjacencyListCapacity Cyclic8Nodes(){
            AdjacencyListCapacity h = new AdjacencyListCapacity(8);
            h.agregaVertice(0,1,3,0);
            h.agregaVertice(0,2,5,0);
            h.agregaVertice(0,3,2,0);
            h.agregaVertice(0,7,10,0);
            h.agregaVertice(1,0,3,0);
            h.agregaVertice(1,2,5,0);
            h.agregaVertice(1,4,4,0);
            h.agregaVertice(1,6,6,0);
            h.agregaVertice(1,3,8,0);
            h.agregaVertice(1,7,6,0);
            h.agregaVertice(2,0,5,0);
            h.agregaVertice(2,1,5,0);
            h.agregaVertice(2,6,9,0);
            h.agregaVertice(2,4,1,0);
            h.agregaVertice(2,5,7,0);
            h.agregaVertice(3,7,14,0);
            h.agregaVertice(3,0,2,0);
            h.agregaVertice(3,1,8,0);
            h.agregaVertice(3,4,12,0);
            h.agregaVertice(4,2,1,0);
            h.agregaVertice(4,1,4,0);
            h.agregaVertice(4,3,12,0);
            h.agregaVertice(4,6,15,0);
            h.agregaVertice(5,2,7,0);
            h.agregaVertice(5,7,9,0);
            h.agregaVertice(6,1,6,0);
            h.agregaVertice(6,2,9,0);
            h.agregaVertice(6,4,15,0);
            h.agregaVertice(6,7,3,0);
            h.agregaVertice(7,0,10,0);
            h.agregaVertice(7,5,9,0);
            h.agregaVertice(7,1,6,0);
            h.agregaVertice(7,6,3,0);
            h.agregaVertice(7,3,14,0);
            return h;
        }
    }
}