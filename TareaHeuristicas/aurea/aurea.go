package main

import (
	"fmt"
	"bufio"
	"os"
	"math"
)

func funcOfX(x float64) float64 {
	//in Go i have to make the numbers float
	return math.Sin(x) * 2 - (math.Pow(x,2.0)/10)
}

func funcSinCosOfX(x float64) float64 {
	return math.Sin(x) * (1 + math.Cos(x)*4)
}

func xValue(xmx , xmn , r float64, one bool) float64 {
	res := r*(xmx - xmn)
	if one {	//if this is the x1
		return xmx - res
	}			//this is the x2
	return xmn + res
}

func solve(problem int){
	const r float64 = 0.618
	const errMargin float64 = 0.0000005

	var xmin float64 = 0
	var xmax float64 = 4
	
	x1 := 0.0
	x2 := 0.0
	
	fx1 := 0.0
	fx2 := math.MaxFloat64
	//Assign the type of function to execute
	//this is probably a bad practice, so I don't wanna use it in the future
	//because you make the code more complex than necessary
	fx := funcSinCosOfX
	if problem == 1{
		fx = funcOfX
	}
	i := 0	//number of iterations
	actualErr := math.Abs(fx1 - fx2)
	/*while the difference between the two functions
	* is bigger than the margin error
	*/
	for actualErr > errMargin {
		x1 = xValue(xmax,xmin,r,true)
		x2 = xValue(xmax,xmin,r,false)
		fx1 = fx(x1)
		fx2 = fx(x2)
		if fx1 > fx2 {
			//x min remains the same value
			xmax = x2
		} else{
			xmin = x1
			//x max remains the same value
		}
		actualErr = math.Abs(fx1 - fx2)
		i++
	}
	fmt.Printf("Final Results \n")
	fmt.Printf("Err Margin -> %v \nIterations -> %d \n",errMargin,i)
	fmt.Printf("x1 = %f \n",x1)
	fmt.Printf("x2 = %f \n",x2)
	fmt.Printf("f(x1) = %f \n",fx1)
	fmt.Printf("f(x2) = %f \n",fx2)
	fmt.Printf("x min = %f \n",xmin)
	fmt.Printf("x max = %f \n",xmax)
}

func main()  {

	fmt.Println("Function 1 : Sin(x) * 2 - (x^2/10)")
	fmt.Println("Function 2 : Sin(x) * (1 + Cos(x) * 4)")
	fmt.Println("Choose the function 1 or 2")

	reader := bufio.NewReader(os.Stdin)
	choosed, _, err := reader.ReadRune()

	if err != nil {
		fmt.Println(err)
	}

	switch choosed {
		case '1':
			solve(1)
			break
		case '2':
			solve(2)
			break
		default:
			fmt.Println("Choose 1 or 2, please")
	}
}