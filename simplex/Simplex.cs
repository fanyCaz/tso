using System;
using System.Collections.Generic;

namespace simplex{
    class Constraint{
        public enum Type
        {
            LessOrEqual,
            GreaterOrEqual,
            Equal
        }
        public int[] constants;
        public int result;
        public Type typeR;
        public Constraint(int[] constants, int result, Type type){
            this.constants = constants;
            this.result = result;
            this.typeR = type;
        }
    }
    class Simplex{
        static int[] constraints;
        static void Init(){
            List<Constraint> constraintsList = new List<Constraint>();
            constraintsList.Add(new Constraint(new int[]{2,1}, 18, Constraint.Type.LessOrEqual));
            constraintsList.Add(new Constraint(new int[]{2,3},42, Constraint.Type.LessOrEqual));
            constraintsList.Add(new Constraint(new int[]{3,1}, 24, Constraint.Type.LessOrEqual));
        }
    }
}