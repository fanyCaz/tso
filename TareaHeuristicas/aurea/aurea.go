package main

import (
	"fmt"
	"math"
)

func funcOfX(x float64) float64 {
	//in Go i have to make the numbers float
	return math.Sin(x) * 2 - (math.Pow(x,2.0)/10)
}

func xValue(xmx , xmn , r float64, one bool) float64 {
	res := r*(xmx - xmn)
	if one {	//if this is the x1
		return xmx - res
	}			//this is the x2
	return xmn + res
}

func solve(){
	const r float64 = 0.618

	var xmin float64 = 0
	var xmax float64 = 4

	x1 := xValue(xmax,xmin,r,true)
	x2 := xValue(xmax,xmin,r,false)

	fmt.Printf("x1 = %f \n",x1)
	fmt.Printf("x2 = %f \n",x2)
	fx1 := funcOfX(x1)
	fx2 := funcOfX(x2)
	fmt.Printf("f(x1) = %f \n",fx1)
	fmt.Printf("f(x2) = %f \n",fx2)
}

func main()  {
	//First, the function
	//Second, the user enter values
	solve()
}